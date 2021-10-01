using System;
using System.Threading.Tasks;
using FlameAndWax.Data.Models;
using FlameAndWax.Services.Services.Models;

namespace FlameAndWax.Services.Services.Interfaces
{
    public interface IContactUsService
    {
        Task<ServiceResult<Boolean>> SendMessageAsync(MessageModel newMessage, string connectionString);
    }
}