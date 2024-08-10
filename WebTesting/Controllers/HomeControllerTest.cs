using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication1.Controllers;
using Domain.ServiceInterfaces;
using System.Collections.Generic;
using Domain.Entities;

namespace MyWebAppTesting.Controllers // Test namespace
{
    [TestFixture]
    public class HomeControllerTests
    {
        private Mock<ILogger<HomeController>> _mockLogger;
        private Mock<IProductService> _mockProductService;
        private Mock<IOrderService> _mockOrderService;
        private Mock<IOrderItemService> _mockOrderItemService;
        private Mock<ICustomerService> _mockCustomerService;
        private Mock<IFeedBackService> _mockFeedBackService;
        private Mock<ISanitizationHelper> _mockSanitizer;
        private HomeController _controller;

        [SetUp]
        public void Setup()
        {
            // Set up mocks
            _mockLogger = new Mock<ILogger<HomeController>>();
            _mockProductService = new Mock<IProductService>();
            _mockOrderService = new Mock<IOrderService>();
            _mockOrderItemService = new Mock<IOrderItemService>();
            _mockCustomerService = new Mock<ICustomerService>();
            _mockFeedBackService = new Mock<IFeedBackService>();
            _mockSanitizer = new Mock<ISanitizationHelper>();
            _mockFeedBackService = new Mock<IFeedBackService>();

            // Initialize the controller with mock dependencies
            _controller = new HomeController(
                _mockLogger.Object,
                _mockSanitizer.Object,
                _mockProductService.Object,
                _mockOrderService.Object,
                _mockOrderItemService.Object,
                _mockCustomerService.Object,
                _mockFeedBackService.Object
            );
        }


        [TearDown]
        public void TearDown()
        {
            // Dispose of the controller if it implements IDisposable
            _controller?.Dispose();
        }

        [Test]
        public void Index_ReturnsViewResult_WithListOfProducts()
        {
            // Arrange
            var products = new List<Products>
            {
                new Products { ID = 1, PName = "Laptop", DiscountedPrice = 500 },
                new Products { ID = 2, PName = "Phone", DiscountedPrice = 300 }
            };
            _mockProductService.Setup(service => service.GetAll()).Returns(products);

            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.IsInstanceOf<List<Products>>(result.Model);
            Assert.AreEqual(2, ((List<Products>)result.Model).Count);
        }

        [Test]
        public void ContactUs_Post_ValidFeedBack_RedirectsToSuccess()
        {
            // Arrange
            var feedback = new FeedBack { Message = "Test feedback" };
            _mockSanitizer.Setup(s => s.SanitizeString(It.IsAny<string>())).Returns((string msg) => msg);

            // Act
            var result = _controller.ContactUs(feedback) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("feedBackSuccess", result.ActionName);
            _mockFeedBackService.Verify(repo => repo.Add(It.IsAny<FeedBack>()), Times.Once);
        }

        [Test]
        public void Search_AllProducts_ReturnsPartialViewWithProducts()
        {
            // Arrange
            var products = new List<Products>
            {
                new Products { ID = 1, PName = "Laptop" },
                new Products { ID = 2, PName = "Phone" }
            };
            _mockProductService.Setup(service => service.GetAll()).Returns(products);

            // Act
            var result = _controller.Search("all") as PartialViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<Products>>(result.Model);
            Assert.AreEqual("_SearchPartial", result.ViewName);
            Assert.AreEqual(2, ((List<Products>)result.Model).Count);
        }

        [Test]
        public void Search_ProductByName_ReturnsPartialViewWithOneProduct()
        {
            // Arrange
            var product = new Products { ID = 1, PName = "Laptop" };
            _mockProductService.Setup(service => service.GetByName("Laptop")).Returns(product);

            // Act
            var result = _controller.Search("Laptop") as PartialViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<Products>>(result.Model);
            Assert.AreEqual("_SearchPartial", result.ViewName);
            Assert.AreEqual(1, ((List<Products>)result.Model).Count);
        }
    }
}
