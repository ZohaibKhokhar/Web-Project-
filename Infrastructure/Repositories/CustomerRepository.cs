using Microsoft.Data.SqlClient;
using Domain.Interfaces;
using Domain.Entities;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private const string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=thisdb;Integrated Security=True;";

        public async Task AddCustomer(Customer customer)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "INSERT INTO Customers (CustomerId, Name, Address, PhoneNumber, Email) " +
                               "VALUES (@CustomerId, @Name, @Address, @PhoneNumber, @Email)";
                await conn.ExecuteAsync(query, customer);
            }
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "SELECT * FROM Customers";
                return (await conn.QueryAsync<Customer>(query)).AsList();
            }
        }

        public async Task<int> getLastId()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "SELECT MAX(CustomerId) FROM Customers";
                return await conn.ExecuteScalarAsync<int>(query);
            }
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "SELECT * FROM Customers WHERE CustomerId = @Id";
                return await conn.QuerySingleOrDefaultAsync<Customer>(query, new { Id = id });
            }
        }

        public async Task<List<int>> GetCustomerId(string currentUserName)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "SELECT CustomerId FROM Customers WHERE Email = @Email";
                return (await conn.QueryAsync<int>(query, new { Email = currentUserName })).AsList();
            }
        }

        public async Task deleteByCustomerId(int customerId)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "DELETE FROM Customers WHERE CustomerId = @CustomerId";
                await conn.ExecuteAsync(query, new { CustomerId = customerId });
            }
        }
    }
}
