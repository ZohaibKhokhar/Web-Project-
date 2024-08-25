using Microsoft.Data.SqlClient;
using Dapper;
using Domain.Interfaces;
using Domain.Entities;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        public const string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=thisdb;Integrated Security=True;";

        public void AddOrderItem(OrderItem orderItem)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "INSERT INTO OrderItem (OrderId, ProductId, Quantity, Price) VALUES (@OrderId, @ProductId, @Quantity, @Price)";
                connection.Execute(query, orderItem);
            }
        }

        public List<OrderItem> GetAllOrderItems()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "SELECT * FROM OrderItem";
                return connection.Query<OrderItem>(query).AsList();
            }
        }

        public List<OrderItem> GetAllByOrderId(int orderId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "SELECT * FROM OrderItem WHERE OrderId = @OrderId";
                return connection.Query<OrderItem>(query, new { OrderId = orderId }).AsList();
            }
        }

        public void deleteByOrderId(int orderId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "DELETE FROM OrderItem WHERE OrderId = @OrderId";
                connection.Execute(query, new { OrderId = orderId });
            }
        }
    }
}
