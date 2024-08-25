using Microsoft.Data.SqlClient;
using Domain.Interfaces;
using Domain.Entities;
using Dapper;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public class CustomerRepository :ICustomerRepository
    {
        private const string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=thisdb;Integrated Security=True;";

        public void AddCustomer(Customer customer)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Customers (CustomerId, Name, Address, PhoneNumber, Email) " +
                               "VALUES (@CustomerId, @Name, @Address, @PhoneNumber, @Email)";
                conn.Execute(query, customer);
            }
        }

        public List<Customer> GetAllCustomers()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Customers";
                return conn.Query<Customer>(query).AsList();
            }
        }

        public int getLastId()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT MAX(CustomerId) FROM Customers";
                return conn.ExecuteScalar<int>(query);
            }
        }

        public Customer GetCustomerById(int id)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Customers WHERE CustomerId = @Id";
                return conn.QuerySingleOrDefault<Customer>(query, new { Id = id });
            }
        }

        public List<int> GetCustomerId(string currentUserName)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT CustomerId FROM Customers WHERE Email = @Email";
                return conn.Query<int>(query, new { Email = currentUserName }).AsList();
            }
        }

        public void deleteByCustomerId(int customerId)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Customers WHERE CustomerId = @CustomerId";
                conn.Execute(query, new { CustomerId = customerId });
            }
        }
    }
}
