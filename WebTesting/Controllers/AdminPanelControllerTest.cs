using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using WebApplication1.Controllers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using WebApplication1.Models;

namespace MyWebAppTesting.Controllers
{
    [TestFixture]
    public class AdminPanelControllerTests
    {
        private Mock<ILogger<AdminPanelController>> _mockLogger;
        private Mock<IWebHostEnvironment> _mockEnv;
        private Mock<IProductService> _mockProductService;
        private Mock<IAppointmentService> _mockAppointmentService;
        private Mock<ICustomerService> _mockCustomerService;
        private Mock<IOrderService> _mockOrderService;
        private Mock<IOrderItemService> _mockOrderItemService;
        private Mock<IFeedBackRepository> _mockFeedBackRepository;
        private AdminPanelController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger<AdminPanelController>>();
            _mockEnv = new Mock<IWebHostEnvironment>();
            _mockProductService = new Mock<IProductService>();
            _mockAppointmentService = new Mock<IAppointmentService>();
            _mockCustomerService = new Mock<ICustomerService>();
            _mockOrderService = new Mock<IOrderService>();
            _mockOrderItemService = new Mock<IOrderItemService>();
            _mockFeedBackRepository = new Mock<IFeedBackRepository>();

            _controller = new AdminPanelController(
                _mockLogger.Object,
                _mockFeedBackRepository.Object,
                _mockEnv.Object,
                _mockProductService.Object,
                _mockAppointmentService.Object,
                _mockCustomerService.Object,
                _mockOrderService.Object,
                _mockOrderItemService.Object
            );
        }


        [TearDown]
        public void TearDown()
        {
            // Dispose of the controller if it implements IDisposable
            _controller?.Dispose();
        }

        [Test]
        public void Index_ReturnsViewResult_WithProducts()
        {
            // Arrange
            var products = new List<Products>
            {
                new Products { ID = 1, PName = "Product1" },
                new Products { ID = 2, PName = "Product2" }
            };
            _mockProductService.Setup(service => service.GetAll()).Returns(products);

            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as List<Products>;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count);
        }

        [Test]
        public void Edit_Get_ReturnsViewResult_WithProduct()
        {
            // Arrange
            var product = new Products { ID = 1, PName = "Product1" };
            _mockProductService.Setup(service => service.Get(1)).Returns(product);

            // Act
            var result = _controller.Edit(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as Products;
            Assert.IsNotNull(model);
            Assert.AreEqual(product.ID, model.ID);
        }

        [Test]
        public void Edit_Post_ValidProduct_ReturnsRedirectToActionResult()
        {
            // Arrange
            var product = new Products { ID = 1, PName = "UpdatedProduct" };
            var file = new Mock<IFormFile>();
            _mockEnv.Setup(env => env.WebRootPath).Returns("wwwroot");

            // Act
            var result = _controller.Edit(product, file.Object) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            _mockProductService.Verify(service => service.Update(It.IsAny<Products>()), Times.Once);
        }

        [Test]
        public void Remove_ValidId_ReturnsRedirectToActionResult()
        {
            // Act
            var result = _controller.Remove(1) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("DeleteSuccess", result.ActionName);
            _mockProductService.Verify(service => service.DeleteById(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void DeleteSuccess_ReturnsViewResult()
        {
            // Act
            var result = _controller.DeleteSuccess() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void Add_ReturnsViewResult()
        {
            // Act
            var result = _controller.Add() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }



        [Test]
        public void AllOrders_ReturnsViewResult_WithOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { OrderId = 1 },
                new Order { OrderId = 2 }
            };
            _mockOrderService.Setup(service => service.GetAll()).Returns(orders);

            // Act
            var result = _controller.AllOrders() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as List<Order>;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count);
        }

        [Test]
        public void OrderDetail_ReturnsViewResult_WithOrderItems()
        {
            // Arrange
            var orderItems = new List<OrderItem>
            {
                new OrderItem { Id = 1 },
                new OrderItem { Id = 2 }
            };
            _mockOrderItemService.Setup(service => service.GetAllByOrderId(1)).Returns(orderItems);

            // Act
            var result = _controller.OrderDetail(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as List<OrderItem>;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count);
        }

        [Test]
        public void CustomerDetail_ReturnsViewResult_WithCustomer()
        {
            // Arrange
            var customer = new Customer { CustomerId = 1, Name = "John Doe" };
            _mockCustomerService.Setup(service => service.GetCustomerById(1)).Returns(customer);

            // Act
            var result = _controller.CustomerDetail(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as Customer;
            Assert.IsNotNull(model);
            Assert.AreEqual(customer.CustomerId, model.CustomerId);
        }

        [Test]
        public void ProductDetail_ReturnsViewResult_WithProduct()
        {
            // Arrange
            var product = new Products { ID = 1, PName = "Product1" };
            _mockProductService.Setup(service => service.Get(1)).Returns(product);

            // Act
            var result = _controller.ProductDetail(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as Products;
            Assert.IsNotNull(model);
            Assert.AreEqual(product.ID, model.ID);
        }

        [Test]
        public void AllCustomers_ReturnsViewResult_WithCustomers()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { CustomerId = 1, Name = "Customer1" },
                new Customer { CustomerId = 2, Name = "Customer2" }
            };
            _mockCustomerService.Setup(service => service.GetAllCustomers()).Returns(customers);

            // Act
            var result = _controller.AllCustomers() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as List<Customer>;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count);
        }

        [Test]
        public void ShowAll_ReturnsViewResult_WithAppointments()
        {
            // Arrange
            var appointments = new List<Appointment>
            {
                new Appointment { Id = 1 },
                new Appointment { Id = 2 }
            };
            _mockAppointmentService.Setup(service => service.GetAll()).Returns(appointments);

            // Act
            var result = _controller.ShowAll() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as List<Appointment>;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count);
        }

        [Test]
        public void DeleteOrder_ValidId_RedirectsToOrderDeleteSuccess()
        {
            // Arrange
            _mockOrderService.Setup(service => service.getCustomerIdByOrderId(It.IsAny<int>())).Returns(1);

            // Act
            var result = _controller.DeleteOrder(1) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("OrderDeleteSuccess", result.ActionName);
            _mockOrderItemService.Verify(service => service.deleteByOrderId(It.IsAny<int>()), Times.Once);
            _mockOrderService.Verify(service => service.deleteOrderById(It.IsAny<int>()), Times.Once);
            _mockCustomerService.Verify(service => service.deleteByCustomerId(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void OrderDeleteSuccess_ReturnsViewResult()
        {
            // Act
            var result = _controller.OrderDeleteSuccess() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void OrderDelivered_ValidId_RedirectsToAllOrders()
        {
            // Act
            var result = _controller.OrderDelivered(1) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("AllOrders", result.ActionName);
            _mockOrderItemService.Verify(service => service.deleteByOrderId(It.IsAny<int>()), Times.Once);
            _mockOrderService.Verify(service => service.deleteOrderById(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void Success_ReturnsViewResult()
        {
            // Act
            var result = _controller.Success() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void Messages_ReturnsViewResult_WithFeedback()
        {
            // Arrange
            var feedbacks = new List<FeedBack>
            {
                new FeedBack { Id = 1, Message = "Feedback 1" },
                new FeedBack { Id = 2, Message = "Feedback 2" }
            };
            _mockFeedBackRepository.Setup(repo => repo.GetAll()).Returns(feedbacks);

            // Act
            var result = _controller.Messages() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as List<FeedBack>;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count);
        }
    }
}
