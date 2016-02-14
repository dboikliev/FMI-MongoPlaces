using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoPlaces.Core.DataAccess.Implementations;
using MongoPlaces.Core.DataAccess.Interfaces;
using MongoPlaces.Core.Services.Interfaces;

namespace MongoPlaces.Core.Services.Implementations
{
    public class LocationTypesService : ILocationTypesService
    {
        private readonly ILocationTypesRepository _locationTypesRepository;

        public LocationTypesService()
        {
            _locationTypesRepository = new LocationTypesRepository();
        }

        public async Task<IEnumerable<string>> GetAllAsync()
        {
            return await _locationTypesRepository.FindAll();
        }
    }
}
