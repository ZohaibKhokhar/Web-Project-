using Microsoft.Data.SqlClient;
using Dapper;
using Domain.Interfaces;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class FeedBackRepository : IFeedBackRepository
    {
        private readonly string _connectionString;

        public FeedBackRepository()
        {
            _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=thisdb;Integrated Security=True;";
        }

        public async Task Add(FeedBack feedback)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO FeedBack (Name, Email, Message) VALUES (@Name, @Email, @Message)";
                await connection.ExecuteAsync(query, feedback);
            }
        }

        public async Task<IEnumerable<FeedBack>> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT Id, Name, Email, Message FROM FeedBack";
                return await connection.QueryAsync<FeedBack>(query);
            }
        }
    }
}
