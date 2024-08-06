using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.Services;
using WebApplication1.Models;


namespace WebApplication1.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ISanitizationHelper _sanitizer;
        private readonly ILogger<HomeController> _logger;

        public AppointmentController(ILogger<HomeController> logger,IAppointmentService appointmentService, ISanitizationHelper sanitizer)
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
        public IActionResult Add(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                appointment.Name = _sanitizer.SanitizeString(appointment.Name);
                appointment.Reason = _sanitizer.SanitizeString(appointment.Reason);
                appointment.Email =User.Identity.Name;
                _appointmentService.Add(appointment);
                return RedirectToAction("Success");
            }
            else
                return View(appointment);    
        }
        public IActionResult Success()
        {
            return View();
        }
        public IActionResult MyAppointments()
        {
            return View(_appointmentService.GetByEmail(User.Identity.Name));
        }
        public IActionResult Delete(int id)
        {
            _appointmentService.DeleteById(id);
            return RedirectToAction("ShowAll", "AdminPanel");
        }
    }
}
        