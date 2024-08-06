﻿using WebApplication1.Models;


namespace WebApplication1.Models
{
    public interface IAppointmentService
    {
        public void Add(Appointment appointment);

        public void Update(Appointment appointment);

        public void DeleteById(int id);

        public void DeleteByName(string name);

        public List<Appointment> GetByEmail(string email);

        public List<Appointment> GetAll();
    }
}
