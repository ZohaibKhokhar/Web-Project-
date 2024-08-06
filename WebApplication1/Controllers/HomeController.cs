using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models.Services;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;
        private readonly ICustomerService _customerService;
        private readonly IFeedBackRepository _feedBackRepository;
        private readonly ISanitizationHelper _sanitizer;
        public HomeController(ILogger<HomeController> logger,ISanitizationHelper sanitizer, IProductService productService, IOrderService orderService, IOrderItemService orderItemService, ICustomerService customerService,IFeedBackRepository feedBackRepository)
        {
            _logger = logger;
            _productService = productService;
            _orderService = orderService;
            _orderItemService = orderItemService;
            _customerService = customerService;
            _feedBackRepository=feedBackRepository;
            _sanitizer = sanitizer;

        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View(_productService.GetAll());
        }

        [AllowAnonymous]
        public IActionResult AboutUs()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ContactUs()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ContactUs(FeedBack feedBack)
        {
            if (ModelState.IsValid)
            {
                feedBack.Message = _sanitizer.SanitizeString(feedBack.Message);
                _feedBackRepository.Add(feedBack);
                return RedirectToAction("feedBackSuccess");
            }
            else
            {
                return View(feedBack);
            }
        }
        public IActionResult feedBackSuccess()
        {
            return View();
        }
        public IActionResult SearchProduct()
        {
            _logger.LogInformation("The user is in searchproducts");
            
            return View(_productService.GetAll());
        }
       
        public IActionResult Search(string x)
        {
            _logger.LogInformation("The user is in search");
            List<Products> list = new List<Products>();
            if (x == "all" || x == "ALL" || x == "All")
                list = _productService.GetAll();
            else
                list.Add(_productService.GetByName(x));
            if (list.Count == 0)
                ViewBag.Message = "No Item Found";
            
            
            return PartialView("_SearchPartial",list);
        }
        public IActionResult MyOrder()
        {
            List<int> customerIds = new List<int>();

            customerIds = _customerService.GetCustomerId(User.Identity.Name);
            List<Order> listOrders = new List<Order>();
            if (customerIds.Count != 0)
            {

                foreach (var cusId in customerIds)
                {
                    int orderId = _orderService.GetOrderIdByCustomerId(cusId);
                    Order order = new Order();
                    order = _orderService.getOrderById(orderId);
                    listOrders.Add(order);
                }

                return View(listOrders);
            }
            else
            {

                return View(listOrders);
            }
        }
        public IActionResult YourOrder(int id)
        {
            List<OrderItem> items = new List<OrderItem>();
            items = _orderItemService.GetAllByOrderId(id);
            return View(items);
        }
        public IActionResult SearchInRange()
        {
            return View();
        }
        public JsonResult GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            List<Products> products = _productService.GetAll();
            List<Products> filtered = new List<Products>();
            foreach (var product in products)
            {
                if (product.DiscountedPrice >= minPrice&&product.DiscountedPrice<=maxPrice)
                {
                    filtered.Add(product);
                }
            }
            return Json(filtered);
        }
        public IActionResult YourDetail(int id)
        {
            return View(_customerService.GetCustomerById(id));
        }
        public IActionResult ProductDetail(int id)
        {
            return View(_productService.Get(id));
        }
    }
}