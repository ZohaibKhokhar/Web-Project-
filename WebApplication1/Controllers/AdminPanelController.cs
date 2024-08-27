using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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

        public AdminPanelController(
            ILogger<AdminPanelController> logger,
            IFeedBackService feedBackService,
            IWebHostEnvironment env,
            IProductService productService,
            IAppointmentService appointmentService,
            ICustomerService customerService,
            IOrderService orderService,
            IOrderItemService orderItemService)
        {
            _logger = logger;
            _env = env;
            _productService = productService;
            _appointmentService = appointmentService;
            _customerService = customerService;
            _orderService = orderService;
            _orderItemService = orderItemService;
            _feedBackService = feedBackService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAll();
            return View(products);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.Get(id);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Products product, IFormFile ImageUrl)
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
                    await ImageUrl.CopyToAsync(fileStream);
                }

                product.ImageUrl = $"/ProductImages/{ImageUrl.FileName}";
            }

            await _productService.Update(product);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int id)
        {
            await _productService.DeleteById(id);
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
        public async Task<IActionResult> AjaxUpload(Products product, IFormFile img)
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
                    await img.CopyToAsync(fileStream);
                }
            }

            string imageUrl = $"/ProductImages/{img.FileName}";
            product.ImageUrl = imageUrl;
            await _productService.Add(product);

            return Json(new { success = true, message = "Product added successfully." });
        }

        public async Task<IActionResult> AllOrders()
        {
            var orders = await _orderService.GetAll();
            return View(orders);
        }

        public async Task<IActionResult> OrderDetail(int id)
        {
            var orderItems = await _orderItemService.GetAllByOrderId(id);
            return View(orderItems);
        }

        public async Task<IActionResult> CustomerDetail(int id)
        {
            var customer = await _customerService.GetCustomerById(id);
            return View(customer);
        }

        public async Task<IActionResult> ProductDetail(int id)
        {
            var product = await _productService.Get(id);
            return View(product);
        }

        public async Task<IActionResult> AllCustomers()
        {
            var customers = await _customerService.GetAllCustomers();
            return View(customers);
        }

        public async Task<IActionResult> ShowAll()
        {
            var appointments = await _appointmentService.GetAll();
            return View(appointments);
        }

        public async Task<IActionResult> DeleteOrder(int id)
        {
            int customerId = await _orderService.getCustomerIdByOrderId(id);
            await _orderItemService.deleteByOrderId(id);
            await _orderService.deleteOrderById(id);
            await _customerService.deleteByCustomerId(customerId);
            return RedirectToAction("OrderDeleteSuccess");
        }

        public IActionResult OrderDeleteSuccess()
        {
            return View();
        }

        public async Task<IActionResult> OrderDelivered(int id)
        {
            await _orderItemService.deleteByOrderId(id);
            await _orderService.deleteOrderById(id);
            return RedirectToAction("AllOrders");
        }

        public IActionResult Success()
        {
            return View();
        }

        public async Task<IActionResult> Messages()
        {
            var feedbacks = await _feedBackService.GetAll();
            return View(feedbacks);
        }
    }
}
