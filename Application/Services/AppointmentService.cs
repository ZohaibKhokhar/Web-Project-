using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task Add(Appointment appointment)
        {
            if (!string.IsNullOrEmpty(appointment.Name) && !string.IsNullOrEmpty(appointment.Email))
            {
                await _appointmentRepository.Add(appointment);
            }
            else
            {
                throw new ArgumentException("Name and Email cannot be empty.");
            }
        }

        public async Task Update(Appointment appointment)
        {
            await _appointmentRepository.Update(appointment);
        }

        public async Task DeleteById(int id)
        {
            await _appointmentRepository.DeleteById(id);
        }

        public async Task DeleteByName(string name)
        {
            await _appointmentRepository.DeleteByName(name);
        }

        public async Task<List<Appointment>> GetByEmail(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                return await _appointmentRepository.GetByEmail(email);
            }
            else
            {
                throw new ArgumentException("Email cannot be empty.");
            }
        }

        public async Task<List<Appointment>> GetAll()
        {
            return await _appointmentRepository.GetAll();
        }

        public async Task<Appointment> GetById(int id)
        {
            return await _appointmentRepository.GetById(id);
        }
    }
}
