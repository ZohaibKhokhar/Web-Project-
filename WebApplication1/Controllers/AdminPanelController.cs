using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using Application.Services;
using Domain.Entities;
using Domain.ServiceInterfaces;

namespace WebApplication1.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    public class AdminPanelController : Controller
    {
        private readonly ILogger<AdminPanelController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;
        private readonly IAppointmentService _appointmentService;
        private readonly ICustomerService _customerService;
        private readonly IFeedBackService _feedBackService;
        public AdminPanelController(ILogger<AdminPanelController> logger,IFeedBackService feedBackService, IWebHostEnvironment env,IProductService productService, IAppointmentService appointmentService,ICustomerService customerService,IOrderService orderService,IOrderItemService orderItemService)
        {
            _logger = logger;
            _env = env;
            _productService = productService;
            _appointmentService = appointmentService;
            _customerService = customerService;
            _orderService = orderService;
            _orderItemService= orderItemService;
            _appointmentService = appointmentService;
            _feedBackService = feedBackService;

        }

        public IActionResult Index()
        {
            List<Products> products = _productService.GetAll();
            return View(products);
        }

        public IActionResult Edit(int id)
        {
            return View(_productService.Get(id));
        }
        [HttpPost]
        public IActionResult Edit(Products product, IFormFile ImageUrl)
        {
            if (ImageUrl != null && ImageUrl.Length > 0)
            {
                string wwwrootPath = _env.WebRootPath;
                string path = Path.Combine(wwwrootPath, "ProductImages");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string filePath = Path.Combine(path, ImageUrl.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    ImageUrl.CopyTo(fileStream);
                }

                product.ImageUrl = $"/ProductImages/{ImageUrl.FileName}";
            }
            
            _productService.Update(product);
            return RedirectToAction("Index", "AdminPanel");
        }

        public IActionResult Remove(int id)
        {
            _productService.DeleteById(id);
            return RedirectToAction("DeleteSuccess");
        }
        public IActionResult DeleteSuccess()
        {
            return View();
        }
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AjaxUpload(Products product, IFormFile img)
        {
            string wwwrootPath = _env.WebRootPath;
            string path = Path.Combine(wwwrootPath, "ProductImages");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (img != null && img.Length > 0)
            {
                string filePath = Path.Combine(path, img.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    img.CopyTo(fileStream);
                }
            }

            string imageUrl = $"/ProductImages/{img.FileName}";
            product.ImageUrl = imageUrl;
            _productService.Add(product);

            return Json(new { success = true, message = "Product added successfully." });
        }

        public IActionResult AllOrders()
        {
            List<Order> orders = _orderService.GetAll();
            return View(orders);
        }

        public IActionResult OrderDetail(int id)
        {
            List<OrderItem> items = new List<OrderItem>();
            items = _orderItemService.GetAllByOrderId(id);
            return View(items);
        }

        public IActionResult CustomerDetail(int id)
        {
         
            return View(_customerService.GetCustomerById(id));
        }

        public IActionResult ProductDetail(int id)
        {
            return View(_productService.Get(id));
        }
        public IActionResult AllCustomers()
        {
            return View(_customerService.GetAllCustomers());
        }

        public IActionResult ShowAll()
        {
            return View(_appointmentService.GetAll());
        }
        public IActionResult DeleteOrder(int id)
        {
            int customerId=_orderService.getCustomerIdByOrderId(id);
            _orderItemService.deleteByOrderId(id);
            _orderService.deleteOrderById(id);
            _customerService.deleteByCustomerId(customerId);
            return RedirectToAction("OrderDeleteSuccess", "AdminPanel");
        }
        public IActionResult OrderDeleteSuccess()
        {
            return View();
        }
        public IActionResult OrderDelivered(int id)
        {
            _orderItemService.deleteByOrderId(id);
            _orderService.deleteOrderById(id);
            return RedirectToAction("AllOrders", "AdminPanel");
        }
        public IActionResult Success()
        {
            return View();
        }
        public IActionResult Messages()
        {
            return View(_feedBackService.GetAll());
        }

    }
}
