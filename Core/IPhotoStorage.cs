using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vega.Core
{
    public interface IPhotoStorage
    {
        Task<string> StorePhoto(string uploadsFolderPath, IFormFile file);
    }
}
