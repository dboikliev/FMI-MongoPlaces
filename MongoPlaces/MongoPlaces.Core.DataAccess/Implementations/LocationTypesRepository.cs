using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoPlaces.Core.DataAccess.Entities;
using MongoPlaces.Core.DataAccess.Interfaces;

namespace MongoPlaces.Core.DataAccess.Implementations
{
    public class LocationTypesRepository : AbstractRepository<LocationType>, ILocationTypesRepository
    {
        protected override string CollectionName => "LocationTypes";
        public async Task<IEnumerable<string>> FindAll()
        {
            return await Collection.Find(FilterDefinition<LocationType>.Empty).Project(loc => loc.Name).ToListAsync();
        }
    }
}
