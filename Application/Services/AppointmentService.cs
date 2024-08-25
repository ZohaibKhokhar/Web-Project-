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
           
            _appointmentRepository.Update(appointment);
        }

        public void DeleteById(int id)
        {
      
            _appointmentRepository.DeleteById(id);
        }

        public void DeleteByName(string name)
        {
          
            _appointmentRepository.DeleteByName(name);
        }

        public List<Appointment> GetByEmail(string email)
        {
         
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
          
            return _appointmentRepository.GetAll();
        }
    }
}
