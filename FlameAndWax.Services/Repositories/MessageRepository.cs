using FlameAndWax.Data.Constants;
using FlameAndWax.Data.Models;
using FlameAndWax.Services.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        public async Task<int> Add(MessageModel Data)
        {
            using SqlConnection connection = new SqlConnection(Constants.DB_CONNECTION_STRING);
            await connection.OpenAsync();
            var queryString = "INSERT INTO MessageTable(Name,Email,PhoneNumber,Message)" +
                "VALUES(@name,@email,@number,@msg)";
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@name", Data.Name);
            command.Parameters.AddWithValue("@email", Data.Email);
            command.Parameters.AddWithValue("@number", Data.PhoneNumber);
            command.Parameters.AddWithValue("@msg", Data.Message);
            await command.ExecuteNonQueryAsync();
            return Data.MessageId;
        }

        public Task Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<MessageModel> Fetch(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<MessageModel>> FetchAll()
        {
            throw new System.NotImplementedException();
        }

        public Task Update(MessageModel data, int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
