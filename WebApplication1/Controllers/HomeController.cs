using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Domain.ServiceInterfaces;
using Domain.Entities;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

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
        private readonly IFeedBackService _feedBackService;
        private readonly IMemoryCache _cache;
        private readonly ISanitizationHelper _sanitizer;

        public HomeController(
            ILogger<HomeController> logger,
            ISanitizationHelper sanitizer,
            IProductService productService,
            IOrderService orderService,
            IOrderItemService orderItemService,
            ICustomerService customerService,
            IFeedBackService feedBackService,
            IMemoryCache cache)
        {
            _logger = logger;
            _productService = productService;
            _orderService = orderService;
            _orderItemService = orderItemService;
            _customerService = customerService;
            _feedBackService = feedBackService;
            _sanitizer = sanitizer;
            _cache = cache;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAll();
            return View(products);
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
        public async Task<IActionResult> ContactUs(FeedBack feedBack)
        {
            if (ModelState.IsValid)
            {
                feedBack.Message = await _sanitizer.SanitizeString(feedBack.Message);
                await _feedBackService.Add(feedBack);
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

        public async Task<IActionResult> SearchProduct()
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


        public async Task<IActionResult> Search(string x)
        {
            List<Products> list = new List<Products>();

            if (x.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                list = await _productService.GetAll();
            }
            else
            {
                var product = await _productService.GetByName(x);
                if (product != null)
                {
                    list.Add(product);
                }
            }

            if (list.Count == 0)
            {
                ViewBag.Message = "No Item Found";
            }

            return PartialView("_SearchPartial", list);
        }

        public async Task<IActionResult> MyOrder()
        {
            var customerIds = await _customerService.GetCustomerId(User.Identity.Name);
            var listOrders = new List<Order>();

            if (customerIds.Count != 0)
            {
                foreach (var cusId in customerIds)
                {
                    var orderId = await _orderService.GetOrderIdByCustomerId(cusId);
                    var order = await _orderService.getOrderById(orderId);
                    listOrders.Add(order);
                }
            }

            return View(listOrders);
        }

        public async Task<IActionResult> YourOrder(int id)
        {
            var items = await _orderItemService.GetAllByOrderId(id);
            return View(items);
        }


        public async Task<IActionResult> YourDetail(int id)
        {
            var customer = await _customerService.GetCustomerById(id);
            return View(customer);
        }

        public async Task<IActionResult> ProductDetail(int id)
        {
            var product = await _productService.Get(id);
            return View(product);
        }
    }
}
