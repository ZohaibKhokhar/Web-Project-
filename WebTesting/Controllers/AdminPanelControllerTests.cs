using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Application.Services;
using Domain.Entities;
using Domain.ServiceInterfaces;
using WebApplication1.Controllers;
using System.Collections.Generic;

namespace MyWebAppTesting.Controllers
{
    [TestFixture]
    public class AdminPanelControllerTests
    {
        private Mock<ILogger<AdminPanelController>> _loggerMock;
        private Mock<IWebHostEnvironment> _envMock;
        private Mock<IProductService> _productServiceMock;
        private Mock<IOrderService> _orderServiceMock;
        private Mock<IOrderItemService> _orderItemServiceMock;
        private Mock<IAppointmentService> _appointmentServiceMock;
        private Mock<ICustomerService> _customerServiceMock;
        private Mock<IFeedBackService> _feedBackServiceMock;
        private AdminPanelController _controller;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<AdminPanelController>>();
            _envMock = new Mock<IWebHostEnvironment>();
            _productServiceMock = new Mock<IProductService>();
            _orderServiceMock = new Mock<IOrderService>();
            _orderItemServiceMock = new Mock<IOrderItemService>();
            _appointmentServiceMock = new Mock<IAppointmentService>();
            _customerServiceMock = new Mock<ICustomerService>();
            _feedBackServiceMock = new Mock<IFeedBackService>();

            _controller = new AdminPanelController(
                _loggerMock.Object,
                _feedBackServiceMock.Object,
                _envMock.Object,
                _productServiceMock.Object,
                _appointmentServiceMock.Object,
                _customerServiceMock.Object,
                _orderServiceMock.Object,
                _orderItemServiceMock.Object
            );
        }


        [TearDown]
        public void TearDown()
        {
            // Dispose of the controller if it implements IDisposable
            _controller?.Dispose();
        }

        [Test]
        public void Index_ReturnsViewWithProducts()
        {
            // Arrange
            var products = new List<Products>
            {
                new Products { ID = 1, PName = "Product1" },
                new Products { ID = 2, PName = "Product2" }
            };
            _productServiceMock.Setup(service => service.GetAll()).Returns(products);

            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(products, result.Model);
        }

        [Test]
        public void Edit_Get_ReturnsViewWithProduct()
        {
            // Arrange
            var product = new Products { ID = 1, PName = "Product1" };
            _productServiceMock.Setup(service => service.Get(1)).Returns(product);

            // Act
            var result = _controller.Edit(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(product, result.Model);
        }

        [Test]
        public void Remove_DeletesProductAndRedirectsToDeleteSuccess()
        {
            // Act
            var result = _controller.Remove(1) as RedirectToActionResult;

            // Assert
            _productServiceMock.Verify(service => service.DeleteById(1), Times.Once);
            Assert.AreEqual("DeleteSuccess", result.ActionName);
        }

        [Test]
        public void AllOrders_ReturnsViewWithOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { OrderId = 1, CustomerId = 1 },
                new Order { OrderId = 2, CustomerId =2 }
            };
            _orderServiceMock.Setup(service => service.GetAll()).Returns(orders);

            // Act
            var result = _controller.AllOrders() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(orders, result.Model);
        }

        [Test]
        public void Messages_ReturnsViewWithFeedback()
        {
            // Arrange
            var feedback = new List<FeedBack>
            {
                new FeedBack { Id = 1, Message = "Feedback1" },
                new FeedBack { Id = 2, Message = "Feedback2" }
            };
            _feedBackServiceMock.Setup(service => service.GetAll()).Returns(feedback);

            // Act
            var result = _controller.Messages() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(feedback, result.Model);
        }
    }
}
