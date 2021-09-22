using System.Threading.Tasks;
using FlameAndWax.Data.Models;

namespace FlameAndWax.Services.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        Task<int> Add(MessageModel Data, string connectionString);
    }
}
