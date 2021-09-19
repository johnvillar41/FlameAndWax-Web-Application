using FlameAndWax.Data.Models;
using FlameAndWax.Services.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        public async Task<int> Add(MessageModel Data, string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using SqlCommand command = new SqlCommand("AddNewMessage", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@name", Data.Name);
            command.Parameters.AddWithValue("@email", Data.Email);
            command.Parameters.AddWithValue("@number", Data.PhoneNumber);
            command.Parameters.AddWithValue("@msg", Data.Message);
            await command.ExecuteNonQueryAsync();
            return Data.MessageId;
        }

        public Task Delete(int id,string connectionString)
        {
            throw new System.NotImplementedException();
        }

        public Task<MessageModel> Fetch(int id, string connectionString)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<MessageModel>> FetchPaginatedResult(int pageNumber, int pageSize, string connectionString)
        {
            throw new System.NotImplementedException();
        }

        public Task Update(MessageModel data, int id, string connectionString)
        {
            throw new System.NotImplementedException();
        }
    }
}
