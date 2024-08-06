using Microsoft.Data.SqlClient;
using System.Data;

namespace WebApplication1.Models
{
    public class FeedBackRepository : IFeedBackRepository
    {
        private readonly string _connectionString;

        public FeedBackRepository()
        {
            _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=thisdb;Integrated Security=True;";
        }

        public void Add(FeedBack feedback)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO FeedBack (Name, Email, Message) VALUES (@Name, @Email, @Message)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", feedback.Name);
                    command.Parameters.AddWithValue("@Email", feedback.Email);
                    command.Parameters.AddWithValue("@Message", feedback.Message);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<FeedBack> GetAll()
        {
            var feedbackList = new List<FeedBack>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT Id, Name, Email, Message FROM FeedBack";
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var feedback = new FeedBack
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Email = reader.GetString(2),
                                Message = reader.GetString(3)
                            };
                            feedbackList.Add(feedback);
                        }
                    }
                }
            }

            return feedbackList;
        }
    }
}
