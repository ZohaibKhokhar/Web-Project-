using Microsoft.Data.SqlClient;
using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Data;

namespace Infrastructure.Repositories
{
    public class ProductsRepository : IProductRepository
    {
        public const string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=thisdb;Integrated Security=True;";

        public void Add(Products product)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Products (PName, Price, DiscountedPrice, Quantity, ImageUrl) VALUES (@PName, @Price, @DiscountedPrice, @Quantity, @ImageUrl)";
                conn.Execute(query, product);
            }
        }

        public Products GetByName(string name)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Products WHERE PName = @PName";
                return conn.QuerySingleOrDefault<Products>(query, new { PName = name });
            }
        }

        public void Update(Products product)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Products SET PName = @PName, Price = @Price, DiscountedPrice = @DiscountedPrice, Quantity = @Quantity, ImageUrl = @ImageUrl WHERE ID = @ID";
                conn.Execute(query, product);
            }
        }

        public void DeleteById(int id)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Products WHERE ID = @ID";
                conn.Execute(query, new { ID = id });
            }
        }

        public void UpdateQuantity(int id, int minusQuantity)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Products SET Quantity = Quantity - @MinusQuantity WHERE ID = @ID";
                conn.Execute(query, new { ID = id, MinusQuantity = minusQuantity });
            }
        }

        public Products Get(int id)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Products WHERE ID = @ID";
                return conn.QuerySingleOrDefault<Products>(query, new { ID = id });
            }
        }

        public List<Products> GetAll()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Products";
                return conn.Query<Products>(query).AsList();
            }
        }
    }
}
