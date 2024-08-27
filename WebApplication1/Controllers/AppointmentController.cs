using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.ServiceInterfaces;
using Domain.Entities;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ISanitizationHelper _sanitizer;
        private readonly ILogger<AppointmentController> _logger;

        public AppointmentController(
            ILogger<AppointmentController> logger,
            IAppointmentService appointmentService,
            ISanitizationHelper sanitizer)
        {
            _logger = logger;
            _appointmentService = appointmentService;
            _sanitizer = sanitizer;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                appointment.Name = await _sanitizer.SanitizeString(appointment.Name);
                appointment.Reason = await _sanitizer.SanitizeString(appointment.Reason);
                appointment.Email = User.Identity.Name;
                await _appointmentService.Add(appointment);
                return RedirectToAction("Success");
            }
            else
                return View(appointment);
        }

        public async Task<IActionResult> GetAll()
        {
            var appointments = await _appointmentService.GetAll();
            return View(appointments);
        }

        public async Task<JsonResult> FetchAll()
        {
            var appointments = await _appointmentService.GetAll();
            return Json(appointments);
        }

        public IActionResult Success()
        {
            return View();
        }

        public async Task<IActionResult> MyAppointments()
        {
            var appointments = await _appointmentService.GetByEmail(User.Identity.Name);
            return View(appointments);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _appointmentService.DeleteById(id);
            return RedirectToAction("ShowAll", "AdminPanel");
        }
    }
}
