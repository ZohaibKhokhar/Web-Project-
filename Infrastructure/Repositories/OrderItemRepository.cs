using Microsoft.Data.SqlClient;
using Dapper;
using Domain.Interfaces;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        public const string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=thisdb;Integrated Security=True;";

        public async Task AddOrderItem(OrderItem orderItem)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "INSERT INTO OrderItem (OrderId, ProductId, Quantity, Price) VALUES (@OrderId, @ProductId, @Quantity, @Price)";
                await connection.ExecuteAsync(query, orderItem);
            }
        }

        public async Task<List<OrderItem>> GetAllOrderItems()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "SELECT * FROM OrderItem";
                return (await connection.QueryAsync<OrderItem>(query)).AsList();
            }
        }

        public async Task<List<OrderItem>> GetAllByOrderId(int orderId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "SELECT * FROM OrderItem WHERE OrderId = @OrderId";
                return (await connection.QueryAsync<OrderItem>(query, new { OrderId = orderId })).AsList();
            }
        }

        public async Task deleteByOrderId(int orderId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "DELETE FROM OrderItem WHERE OrderId = @OrderId";
                await connection.ExecuteAsync(query, new { OrderId = orderId });
            }
        }
    }
}
