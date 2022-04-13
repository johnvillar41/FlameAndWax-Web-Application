using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FlameAndWax.Services.Services.Interfaces
{
    public interface IImagesService
    {
        Task AddImageAsync(string imageName);
        Task DeleteImageAsync(string imageName);
    }
}
