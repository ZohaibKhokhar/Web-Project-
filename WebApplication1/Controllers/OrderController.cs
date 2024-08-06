using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication1.Models;
using WebApplication1.Models.Services;

[Authorize]
public class OrderController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IOrderService _orderService;
    private readonly IOrderItemService _orderItemService;
    private readonly ICustomerService _customerService;
    private readonly IProductService _productService;
    private readonly ISanitizationHelper _sanitizer;
    public OrderController(IHttpContextAccessor httpContextAccessor, IOrderService orderService, IOrderItemService orderItemService, ICustomerService customerService,IProductService productService,ISanitizationHelper sanitizer)
    {
        _httpContextAccessor = httpContextAccessor;
        _orderService = orderService;
        _orderItemService = orderItemService;
        _customerService = customerService;
        _productService = productService;
        _sanitizer = sanitizer;
    }

    public int CartCount
    {
        get
        {
            return GetCartItemsFromSession().Count;
        }
    }

    public IActionResult Index()
    {
       
        return View(_productService.GetAll());
     
    }

    [HttpPost]

    public JsonResult AddToCart(int productId, int quantity)
    {
        try
        {
            var cartItems = GetCartItemsFromSession();
            var existingItem = cartItems.Find(item => item.ProductId == productId);

            if (existingItem != null)
            {
                var prod = _productService.Get(productId);

                if ((existingItem.Quantity + quantity) <= prod.Quantity)
                {
                    existingItem.Quantity += quantity;
                }
                else
                {
                    return Json(new { success = false, message = "Not enough stock available." });
                }
            }
            else
            {
                var prod = _productService.Get(productId);

                if (prod != null && prod.Quantity != 0)
                {
                    cartItems.Add(new CartItem
                    {
                        ProductId = productId,
                        Quantity = quantity,
                        Product = prod
                    });
                }
                else
                {
                    return Json(new { success = false, message = "Product not available." });
                }
            }

            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cartItems));


            // Calculate updated cart count
            int updatedCartCount = cartItems.Count;

            return Json(new { success = true, count = updatedCartCount });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "An error occurred: " + ex.Message });
        }
    }


    [HttpGet]
    public IActionResult PlaceOrder()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PlaceOrder(Customer customer)
    {
        if (ModelState.IsValid)
        {
            customer.Name = _sanitizer.SanitizeString(customer.Name);
            customer.Address = _sanitizer.SanitizeString(customer.Address);
            int id = _customerService.getLastId();
            customer.CustomerId = ++id;


            customer.Email = User.Identity.Name;
            _customerService.AddCustomer(customer);

            id = _orderService.GetMaxOrderId();
            id++;

            Order order = new Order
            {
                OrderId = id,
                CustomerId = customer.CustomerId,
                OrderDate = DateTime.Now,
                TotalPrice = CalculateTotalPrice() // Calculate the total price of the order
            };

            _orderService.AddOrder(order);

            foreach (var item in GetCartItemsFromSession())
            {
                OrderItem orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Product.DiscountedPrice
                };

                _productService.UpdateQuantity(orderItem.ProductId, orderItem.Quantity);
                _orderItemService.AddOrderItem(orderItem);
            }

            HttpContext.Session.Remove("Cart");
            return RedirectToAction("OrderSuccess", "Order");
        }
        else
        {
            return View(customer);
        }
  
    }
    public IActionResult OrderSuccess()
    {
        return View();
    }


    // all cart things using session
    public IActionResult Cart()
    {
        List<CartItem> cartItems = GetCartItemsFromSession(); // or GetCartItemsFromDatabase()
        return View(cartItems);
    }
    [HttpPost]
    public JsonResult RemoveFromCart(int productId)
    {
        try
        {
            // Update cart count
            var cartItems = GetCartItemsFromSession();
            var itemToRemove = cartItems.Find(item => item.ProductId == productId);

            if (itemToRemove != null)
            {
                cartItems.Remove(itemToRemove);
                HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cartItems));

                // Calculate updated cart count
                int updatedCartCount = cartItems.Count;

                return Json(new { success = true, count = updatedCartCount });
            }
            else
            {
                return Json(new { success = false, message = "Item not found in cart." });
            }
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "An error occurred: " + ex.Message });
        }
    }
  


    private List<CartItem> GetCartItemsFromSession()
    {
        var cartItems = new List<CartItem>();
        var session = HttpContext.Session;
        var cartSession = session.GetObject<List<CartItem>>("Cart");
        if (cartSession != null)
        {
            cartItems = cartSession;
        }
        return cartItems;
    }
    private decimal CalculateTotalPrice()
    {
        decimal total = 0;
        foreach (var item in GetCartItemsFromSession())
        {
            total += item.Quantity * item.Product.DiscountedPrice;
        }
        return total;
    }
    [HttpGet]
    public JsonResult GetCartCount()
    {
        try
        {
            int count = GetCartItemsFromSession().Sum(item => item.Quantity);
            return Json(new { count });
        }
        catch (Exception ex)
        {
            return Json(new { count = 0, message = "An error occurred: " + ex.Message });
        }
    }


}


