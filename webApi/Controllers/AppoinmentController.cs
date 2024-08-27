using Domain.Entities;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        // GET: api/Appointment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAll()
        {
            var appointments = await _appointmentService.GetAll();
            return Ok(appointments);
        }

        // GET: api/Appointment/ByEmail/{email}
        [HttpGet("ByEmail/{email}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetByEmail(string email)
        {
            var appointments = await _appointmentService.GetByEmail(email);
            if (appointments == null || appointments.Count == 0)
            {
                return NotFound("No appointments found with the provided email.");
            }
            return Ok(appointments);
        }

        // POST: api/Appointment
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                await _appointmentService.Add(appointment);
                return CreatedAtAction(nameof(GetByEmail), new { email = appointment.Email }, appointment);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/Appointment/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return BadRequest("Appointment ID mismatch");
            }

            var existingAppointment = await _appointmentService.GetById(appointment.Id);
            if (existingAppointment==null)
            {
                return NotFound();
            }

            await _appointmentService.Update(appointment);
            return NoContent();
        }

        // DELETE: api/Appointment/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var existingAppointment = await _appointmentService.GetAll();
            if (existingAppointment == null)
            {
                return NotFound();
            }

            await _appointmentService.DeleteById(id);
            return NoContent();
        }

        // DELETE: api/Appointment/ByName/{name}
        [HttpDelete("ByName/{name}")]
        public async Task<ActionResult> DeleteByName(string name)
        {
            var existingAppointment = await _appointmentService.GetAll();
            if (existingAppointment == null)
            {
                return NotFound();
            }

            await _appointmentService.DeleteByName(name);
            return NoContent();
        }
    }
}
