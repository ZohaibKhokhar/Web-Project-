using Domain.Entities;
using Domain.Interfaces;
using Domain.ServiceInterfaces;
namespace Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public void Add(Appointment appointment)
        {
            // Business logic before adding an appointment
            if (!string.IsNullOrEmpty(appointment.Name) && !string.IsNullOrEmpty(appointment.Email))
            {
                _appointmentRepository.Add(appointment);
            }
            else
            {
                throw new ArgumentException("Name and Email cannot be empty.");
            }
        }

        public void Update(Appointment appointment)
        {
            // Business logic before updating an appointment
            _appointmentRepository.Update(appointment);
        }

        public void DeleteById(int id)
        {
            // Business logic before deleting an appointment by ID
            _appointmentRepository.DeleteById(id);
        }

        public void DeleteByName(string name)
        {
            // Business logic before deleting an appointment by Name
            _appointmentRepository.DeleteByName(name);
        }

        public List<Appointment> GetByEmail(string email)
        {
            // Business logic before fetching appointments by email
            if (!string.IsNullOrEmpty(email))
            {
                return _appointmentRepository.GetByEmail(email);
            }
            else
            {
                throw new ArgumentException("Email cannot be empty.");
            }
        }

        public List<Appointment> GetAll()
        {
            // Maybe add sorting or filtering logic here
            return _appointmentRepository.GetAll();
        }
    }
}
