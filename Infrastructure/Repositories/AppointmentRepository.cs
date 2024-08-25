using Domain.Interfaces;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private const string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=thisdb;Integrated Security=True;";

        public void Add(Appointment appointment)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Appointments (Name, PhoneNumber, PetType, AppointmentDateTime, ReasonForAppointment, Email) " +
                               "VALUES (@Name, @Phone, @PetType, @PreferredDateTime, @Reason, @Email)";
                conn.Execute(query, appointment);
            }
        }

        public void Update(Appointment appointment)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Appointments SET Name = @Name, PhoneNumber = @Phone, PetType = @PetType, " +
                               "AppointmentDateTime = @PreferredDateTime, ReasonForAppointment = @Reason, Email = @Email " +
                               "WHERE ID = @Id";
                conn.Execute(query,appointment);
            }
        }

        public void Delete(Appointment appointment)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Appointments WHERE ID = @Id";
                conn.Execute(query, new { appointment.Id });
            }
        }

        public void DeleteAll()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "TRUNCATE TABLE Appointments";
                conn.Execute(query);
            }
        }

        public void DeleteById(int id)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Appointments WHERE ID = @Id";
                conn.Execute(query, new { Id = id });
            }
        }

        public void DeleteByName(string name)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Appointments WHERE Name = @Name";
                conn.Execute(query, new { Name = name });
            }
        }

        public List<Appointment> GetAll()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Appointments";
                return conn.Query<Appointment>(query).AsList();
            }
        }

        public List<Appointment> GetByEmail(string email)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Appointments WHERE Email = @Email";
                return conn.Query<Appointment>(query, new { Email = email }).AsList();
            }
        }
    }
}
