using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication1.Controllers;
using WebApplication1.Models.Services;
using System.Collections.Generic;
using WebApplication1.Models;

namespace MyWebAppTesting.Controllers
{
    [TestFixture]
    public class AppointmentControllerTests
    {
        private Mock<IAppointmentService> _mockAppointmentService;
        private Mock<ISanitizationHelper> _mockSanitizer;
        private Mock<ILogger<HomeController>> _mockLogger;
        private AppointmentController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockSanitizer = new Mock<ISanitizationHelper>();
            _mockAppointmentService = new Mock<IAppointmentService>();
            _mockLogger = new Mock<ILogger<HomeController>>();

            _controller = new AppointmentController(
                _mockLogger.Object,
                _mockAppointmentService.Object,
                _mockSanitizer.Object
            );
        }


        [TearDown]
        public void TearDown()
        {
            // Dispose of the controller if it implements IDisposable
            _controller?.Dispose();
        }

        [Test]
        public void Index_ReturnsViewResult()
        {
            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void Add_Get_ReturnsViewResult()
        {
            // Act
            var result = _controller.Add() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

     
        [Test]
        public void Add_Post_InvalidAppointment_ReturnsViewResult()
        {
            // Arrange
            var appointment = new Appointment();
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = _controller.Add(appointment) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(appointment, result.Model);
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
        public void MyAppointments_ReturnsViewResult_WithAppointments()
        {
            // Arrange
            var appointments = new List<Appointment>
            {
                new Appointment { Id = 1, Name = "Test Appointment 1", Reason = "Test", Email = "test1@example.com" },
                new Appointment { Id = 2, Name = "Test Appointment 2", Reason = "Test", Email = "test2@example.com" }
            };
            _mockAppointmentService.Setup(service => service.GetByEmail(It.IsAny<string>())).Returns(appointments);

            // Act
            var result = _controller.MyAppointments() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as List<Appointment>;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count);
        }

        [Test]
        public void Delete_ValidId_RedirectsToShowAll()
        {
            // Act
            var result = _controller.Delete(1) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("ShowAll", result.ActionName);
            Assert.AreEqual("AdminPanel", result.ControllerName);
            _mockAppointmentService.Verify(service => service.DeleteById(It.IsAny<int>()), Times.Once);
        }
    }
}
