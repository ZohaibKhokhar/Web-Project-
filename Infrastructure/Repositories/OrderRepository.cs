using Microsoft.Data.SqlClient;
using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public const string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=thisdb;Integrated Security=True;";

        public async Task AddOrder(Order order)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "INSERT INTO Orders (OrderId, CustomerId, OrderDate, TotalPrice) VALUES (@OrderId, @CustomerId, @OrderDate, @TotalPrice)";
                await connection.ExecuteAsync(query, order);
            }
        }

        public async Task<List<Order>> GetAll()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "SELECT * FROM Orders";
                return (await connection.QueryAsync<Order>(query)).AsList();
            }
        }

        public async Task<int> GetMaxOrderId()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "SELECT MAX(OrderId) FROM Orders";
                return await connection.ExecuteScalarAsync<int>(query);
            }
        }

        public async Task<int> GetOrderIdByCustomerId(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "SELECT OrderId FROM Orders WHERE CustomerId = @CustomerId";
                return await connection.ExecuteScalarAsync<int>(query, new { CustomerId = id });
            }
        }

        public async Task<Order> getOrderById(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "SELECT * FROM Orders WHERE OrderId = @OrderId";
                return await connection.QuerySingleOrDefaultAsync<Order>(query, new { OrderId = id });
            }
        }

        public async Task deleteOrderById(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "DELETE FROM Orders WHERE OrderId = @OrderId";
                await connection.ExecuteAsync(query, new { OrderId = id });
            }
        }

        public async Task<int> getCustomerIdByOrderId(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "SELECT CustomerId FROM Orders WHERE OrderId = @OrderId";
                return await connection.ExecuteScalarAsync<int>(query, new { OrderId = id });
            }
        }
    }
}
