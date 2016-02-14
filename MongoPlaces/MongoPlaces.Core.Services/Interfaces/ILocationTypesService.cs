using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoPlaces.Core.Services.Interfaces
{
    public interface ILocationTypesService
    {
        Task<IEnumerable<string>> GetAllAsync();
    }
}
