using Microsoft.Data.SqlClient;
using Domain.Entities;
using Domain.Interfaces;
using System.Data;

namespace Infrastructure.Repositories
{
    public class ProductsRepository : IProductRepository
    {
        public const string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=thisdb;Integrated Security=True;";
        public void Add(Products product)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "insert into Products (PName,Price,DiscountedPrice,Quantity,ImageUrl) values(@pname,@price,@dprice,@quantity,@url)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(product.PName))
                    {
                        cmd.Parameters.Add("@pname", SqlDbType.NVarChar).Value = product.PName;
                    }
                    else
                    {
                        cmd.Parameters.Add("@pname", SqlDbType.NVarChar).Value = "zohaib";
                    }
                    cmd.Parameters.Add("@price", SqlDbType.Decimal).Value = product.Price;
                    cmd.Parameters.Add("@dprice", SqlDbType.Decimal).Value = product.DiscountedPrice;
                    cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = product.Quantity;
                    cmd.Parameters.Add("@url", SqlDbType.NVarChar, int.MaxValue).Value = product.ImageUrl;
                    int rows = cmd.ExecuteNonQuery();
                }
            }
        }

        public Products GetByName(string name)
        {
            Products product = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "select * from Products where PName = @pname";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@pname", SqlDbType.NVarChar).Value = name;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            product = new Products
                            {
                                ID = reader.GetInt32(0),
                                PName = reader["PName"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                DiscountedPrice = Convert.ToDecimal(reader["DiscountedPrice"]),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                ImageUrl = reader["ImageUrl"].ToString()
                            };
                        }
                    }
                }
            }
            return product;
        }

        public void Update(Products product)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Products SET PName = @pname, Price = @price, DiscountedPrice = @dprice, Quantity = @quantity, ImageUrl = @url WHERE ID = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = product.ID;
                    cmd.Parameters.Add("@pname", SqlDbType.NVarChar).Value = product.PName;
                    cmd.Parameters.Add("@price", SqlDbType.Decimal).Value = product.Price;
                    cmd.Parameters.Add("@dprice", SqlDbType.Decimal).Value = product.DiscountedPrice;
                    cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = product.Quantity;
                    cmd.Parameters.Add("@url", SqlDbType.NVarChar).Value = product.ImageUrl;
                    int rows = cmd.ExecuteNonQuery();
                }
            }
        }


        public void DeleteById(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Products WHERE ID = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateQuantity(int id, int minusQuantity)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Products SET Quantity = Quantity - @minusQuantity WHERE ID = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@minusQuantity", SqlDbType.Int).Value = minusQuantity;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        //public void Delete(Products Products) { }
        //public void DeleteAll() { }

        //public void DeleteByName(string name) { }
        //public Products Get()
        //{
        //    return new Products();
        //}
        public Products Get(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Products WHERE ID = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        Products product = new Products
                        {
                            ID = reader.GetInt32(0),
                            PName = reader.GetString(1),
                            Price = reader.GetDecimal(2),
                            DiscountedPrice = reader.GetDecimal(3),
                            Quantity = reader.GetInt32(4),
                            ImageUrl = reader.GetString(5)
                        };
                        return product;
                    }
                }
            }
            return null;
        }
        public List<Products> GetAll()
        {
            List<Products> list = new List<Products>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "Select * from Products"; // Corrected table name
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Products()
                            {
                                ID = int.Parse(reader["ID"].ToString()),
                                PName = reader["PName"].ToString(),
                                Price = decimal.Parse(reader["Price"].ToString()),
                                DiscountedPrice = decimal.Parse(reader["DiscountedPrice"].ToString()),
                                Quantity = int.Parse(reader["Quantity"].ToString()),
                                ImageUrl = reader["ImageUrl"].ToString()
                            }); ;
                        }
                    }
                }
            }
            return list;
        }
    }
}
