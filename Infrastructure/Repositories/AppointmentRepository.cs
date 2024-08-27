using Domain.Interfaces;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private const string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=thisdb;Integrated Security=True;";

        public async Task Add(Appointment appointment)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "INSERT INTO Appointments (Name, PhoneNumber, PetType, AppointmentDateTime, ReasonForAppointment, Email) " +
                               "VALUES (@Name, @Phone, @PetType, @PreferredDateTime, @Reason, @Email)";
                await conn.ExecuteAsync(query, appointment);
            }
        }

        public async Task Update(Appointment appointment)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "UPDATE Appointments SET Name = @Name, PhoneNumber = @Phone, PetType = @PetType, " +
                               "AppointmentDateTime = @PreferredDateTime, ReasonForAppointment = @Reason, Email = @Email " +
                               "WHERE ID = @Id";
                await conn.ExecuteAsync(query, appointment);
            }
        }

        public async Task Delete(Appointment appointment)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "DELETE FROM Appointments WHERE ID = @Id";
                await conn.ExecuteAsync(query, new { appointment.Id });
            }
        }

        public async Task DeleteAll()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "TRUNCATE TABLE Appointments";
                await conn.ExecuteAsync(query);
            }
        }

        public async Task DeleteById(int id)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "DELETE FROM Appointments WHERE ID = @Id";
                await conn.ExecuteAsync(query, new { Id = id });
            }
        }

        public async Task DeleteByName(string name)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "DELETE FROM Appointments WHERE Name = @Name";
                await conn.ExecuteAsync(query, new { Name = name });
            }
        }

        public async Task<List<Appointment>> GetAll()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "SELECT * FROM Appointments";
                return (await conn.QueryAsync<Appointment>(query)).AsList();
            }
        }

        public async Task<List<Appointment>> GetByEmail(string email)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "SELECT * FROM Appointments WHERE Email = @Email";
                return (await conn.QueryAsync<Appointment>(query, new { Email = email })).AsList();
            }
        }

        public async Task<Appointment> GetById(int id)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "SELECT * FROM Appointments WHERE Id = @id";
                return await conn.QueryFirstOrDefaultAsync<Appointment>(query, new { id });
            }
        }

    }
}
