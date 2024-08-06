using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WebApplication1.Controllers;
using WebApplication1.Models.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebApplication1.Models;

namespace MyWebAppTesting.Controllers
{
    [TestFixture]
    public class OrderControllerTests
    {
        private Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private Mock<IOrderService> _mockOrderService;
        private Mock<IOrderItemService> _mockOrderItemService;
        private Mock<ICustomerService> _mockCustomerService;
        private Mock<IProductService> _mockProductService;
        private Mock<ISanitizationHelper> _mockSanitizer;
        private Mock<ISession> _mockSession;
        private OrderController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockOrderService = new Mock<IOrderService>();
            _mockOrderItemService = new Mock<IOrderItemService>();
            _mockCustomerService = new Mock<ICustomerService>();
            _mockProductService = new Mock<IProductService>();
            _mockSanitizer = new Mock<ISanitizationHelper>();
            _mockSession = new Mock<ISession>();

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(ctx => ctx.Session).Returns(_mockSession.Object);
            _mockHttpContextAccessor.Setup(accessor => accessor.HttpContext).Returns(mockHttpContext.Object);

            _controller = new OrderController(
                _mockHttpContextAccessor.Object,
                _mockOrderService.Object,
                _mockOrderItemService.Object,
                _mockCustomerService.Object,
                _mockProductService.Object,
                _mockSanitizer.Object
            );
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up any resources if necessary
            _controller?.Dispose();

        }

        [Test]
        public void Index_ReturnsViewResult_WithAllProducts()
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

       
    }
}
