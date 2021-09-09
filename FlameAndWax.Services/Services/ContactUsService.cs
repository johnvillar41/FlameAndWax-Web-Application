using System.Threading.Tasks;
using FlameAndWax.Data.Models;
using FlameAndWax.Services.Helpers;
using FlameAndWax.Services.Repositories.Interfaces;
using FlameAndWax.Services.Services.Interfaces;

namespace FlameAndWax.Services.Services
{
    public class ContactUsService : IContactUsService
    {
        private readonly IMessageRepository _messageRepository;
        public ContactUsService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        public async Task<ServiceResult<bool>> SendMessage(MessageModel newMessage, string connectionString)
        {
            if (newMessage == null)
                return ServiceHelper.BuildServiceResult<bool>(false, true, "Empty Message!");
            try
            {
                var messageRepositoryResult = await _messageRepository.Add(newMessage, connectionString);
                if (messageRepositoryResult == -1) return ServiceHelper.BuildServiceResult<bool>(false, true, "Error Adding Message");

                return ServiceHelper.BuildServiceResult<bool>(true, false, null);
            }
            catch (System.Data.SqlClient.SqlException) { return ServiceHelper.BuildServiceResult<bool>(false, true, "Please fill up all missing fields!"); }
        }
    }
}