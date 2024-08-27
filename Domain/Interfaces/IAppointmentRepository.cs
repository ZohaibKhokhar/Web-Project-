using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IAppointmentRepository
    {
        Task Add(Appointment appointment);

        Task Update(Appointment appointment);

        Task DeleteById(int id);

        Task DeleteByName(string name);

        Task<List<Appointment>> GetByEmail(string email);

        Task<Appointment> GetById(int id );

        Task<List<Appointment>> GetAll();
    }
}
