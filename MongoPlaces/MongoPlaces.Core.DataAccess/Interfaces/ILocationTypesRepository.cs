using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoPlaces.Core.DataAccess.Interfaces
{
    public interface ILocationTypesRepository
    {
        Task<IEnumerable<string>> FindAll();
    }
}
