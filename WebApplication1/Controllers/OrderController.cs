using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Domain.ServiceInterfaces;
using Domain.Entities;
using WebApplication1.Models;
using Microsoft.AspNetCore.SignalR;
using WebApplication1.Hubs;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Caching.Memory;

[Authorize]
public class OrderController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IOrderService _orderService;
    private readonly IOrderItemService _orderItemService;
    private readonly ICustomerService _customerService;
    private readonly IProductService _productService;
    private readonly ISanitizationHelper _sanitizer;
    private readonly IHubContext<OrderHub> _hubContext;
    private readonly IMemoryCache _cache;

    public OrderController(
        IHttpContextAccessor httpContextAccessor,
        IOrderService orderService,
        IOrderItemService orderItemService,
        ICustomerService customerService,
        IProductService productService,
        ISanitizationHelper sanitizer,
        IHubContext<OrderHub> hubContext,
        IMemoryCache cache)
    {
        _httpContextAccessor = httpContextAccessor;
        _orderService = orderService;
        _orderItemService = orderItemService;
        _customerService = customerService;
        _productService = productService;
        _sanitizer = sanitizer;
        _hubContext = hubContext;
        _cache = cache;
    }

    public int CartCount => GetCartItemsFromSession().Count;

    public async Task<IActionResult> Index()
    {
        const string cacheKey = "productsCache";
        if (!_cache.TryGetValue(cacheKey, out List<Products> products))
        {
            products = await _productService.GetAll();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));


            _cache.Set(cacheKey, products, cacheOptions);
        }
        return View(products);
    }

    [HttpPost]
    public async Task<JsonResult> AddToCart(int productId, int quantity)
    {
        try
        {
            var cartItems = GetCartItemsFromSession();
            var existingItem = cartItems.Find(item => item.ProductId == productId);
            var prod = await _productService.Get(productId);

            if (existingItem != null)
            {
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
    public async Task<IActionResult> PlaceOrder(Customer customer)
    {
        if (ModelState.IsValid)
        {
            customer.Name = await _sanitizer.SanitizeString(customer.Name);
            customer.Address = await _sanitizer.SanitizeString(customer.Address);
            int id = await _customerService.getLastId();
            customer.CustomerId = ++id;
            customer.Email = User.Identity.Name;
            await _customerService.AddCustomer(customer);

            id = await _orderService.GetMaxOrderId();
            id++;

            var order = new Order
            {
                OrderId = id,
                CustomerId = customer.CustomerId,
                OrderDate = DateTime.Now,
                TotalPrice = CalculateTotalPrice()
            };

            await _orderService.AddOrder(order);

            foreach (var item in GetCartItemsFromSession())
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Product.DiscountedPrice
                };

                await _productService.UpdateQuantity(orderItem.ProductId, orderItem.Quantity);
                await _orderItemService.AddOrderItem(orderItem);
            }

            HttpContext.Session.Remove("Cart");
            await _hubContext.Clients.All.SendAsync("ReceiveOrderNotification");

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

    public async Task<IActionResult> Cart()
    {
        var cartItems = GetCartItemsFromSession(); // or GetCartItemsFromDatabase()
        return View(cartItems);
    }

    [HttpPost]
    public async Task<JsonResult> RemoveFromCart(int productId)
    {
        try
        {
            var cartItems = GetCartItemsFromSession();
            var itemToRemove = cartItems.Find(item => item.ProductId == productId);

            if (itemToRemove != null)
            {
                cartItems.Remove(itemToRemove);
                HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cartItems));
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
            int count = GetCartItemsFromSession().Count;
            return Json(new { count });
        }
        catch (Exception ex)
        {
            return Json(new { count = 0, message = "An error occurred: " + ex.Message });
        }
    }
}
