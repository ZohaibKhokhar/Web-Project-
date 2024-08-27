using Microsoft.Data.SqlClient;
using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductsRepository : IProductRepository
    {
        public const string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=thisdb;Integrated Security=True;";

        public async Task Add(Products product)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var query = "INSERT INTO Products (PName, Price, DiscountedPrice, Quantity, ImageUrl) VALUES (@PName, @Price, @DiscountedPrice, @Quantity, @ImageUrl)";
                await conn.ExecuteAsync(query, product);
            }
        }

        public async Task<Products> GetByName(string name)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var query = "SELECT * FROM Products WHERE PName = @PName";
                return await conn.QuerySingleOrDefaultAsync<Products>(query, new { PName = name });
            }
        }

        public async Task Update(Products product)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var query = "UPDATE Products SET PName = @PName, Price = @Price, DiscountedPrice = @DiscountedPrice, Quantity = @Quantity, ImageUrl = @ImageUrl WHERE ID = @ID";
                await conn.ExecuteAsync(query, product);
            }
        }

        public async Task DeleteById(int id)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var query = "DELETE FROM Products WHERE ID = @ID";
                await conn.ExecuteAsync(query, new { ID = id });
            }
        }

        public async Task UpdateQuantity(int id, int minusQuantity)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var query = "UPDATE Products SET Quantity = Quantity - @MinusQuantity WHERE ID = @ID";
                await conn.ExecuteAsync(query, new { ID = id, MinusQuantity = minusQuantity });
            }
        }

        public async Task<Products> Get(int id)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var query = "SELECT * FROM Products WHERE ID = @ID";
                return await conn.QuerySingleOrDefaultAsync<Products>(query, new { ID = id });
            }
        }

        public async Task<List<Products>> GetAll()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var query = "SELECT * FROM Products";
                return (await conn.QueryAsync<Products>(query)).AsList();
            }
        }
    }
}
