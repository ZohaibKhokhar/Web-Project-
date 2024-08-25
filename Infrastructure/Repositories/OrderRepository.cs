using Microsoft.Data.SqlClient;
using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public const string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=thisdb;Integrated Security=True;";

        public void AddOrder(Order order)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "INSERT INTO Orders (OrderId, CustomerId, OrderDate, TotalPrice) VALUES (@OrderId, @CustomerId, @OrderDate, @TotalPrice)";
                connection.Execute(query, order);
            }
        }

        public List<Order> GetAll()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "SELECT * FROM Orders";
                return connection.Query<Order>(query).AsList();
            }
        }

        public int GetMaxOrderId()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "SELECT MAX(OrderId) FROM Orders";
                return connection.ExecuteScalar<int>(query);
            }
        }

        public int GetOrderIdByCustomerId(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "SELECT OrderId FROM Orders WHERE CustomerId = @CustomerId";
                return connection.ExecuteScalar<int>(query, new { CustomerId = id });
            }
        }

        public Order getOrderById(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "SELECT * FROM Orders WHERE OrderId = @OrderId";
                return connection.QuerySingleOrDefault<Order>(query, new { OrderId = id });
            }
        }

        public void deleteOrderById(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "DELETE FROM Orders WHERE OrderId = @OrderId";
                connection.Execute(query, new { OrderId = id });
            }
        }

        public int getCustomerIdByOrderId(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "SELECT CustomerId FROM Orders WHERE OrderId = @OrderId";
                return connection.ExecuteScalar<int>(query, new { OrderId = id });
            }
        }
    }
}
