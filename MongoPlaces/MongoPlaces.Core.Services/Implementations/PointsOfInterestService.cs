using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoPlaces.Core.Contracts.Dto;
using MongoPlaces.Core.DataAccess.Entities;
using MongoPlaces.Core.DataAccess.Implementations;
using MongoPlaces.Core.DataAccess.Interfaces;
using MongoPlaces.Core.Services.Interfaces;

namespace MongoPlaces.Core.Services.Implementations
{
    public class PointsOfInterestService : IPointsOfInterestService
    {
        private readonly IPointsOfInterestRepository _pointsOfInterestRepository;
        private readonly IUsersRepository _usersRepository;
        public PointsOfInterestService()
        {
            _usersRepository = new UsersRepository();
            _pointsOfInterestRepository = new PointsOfInterestRepository();
        }

        public async Task AddPointOfInterestAsync(PointOfInterestDto p)
        {
            await _pointsOfInterestRepository.CreateAsync(new PointOfInterest
            {
                Description = p.Description,
                Location = new Location
                {
                    X = p.Location.Longitude,
                    Y = p.Location.Latitude
                },
                Type = p.Type,
                Name = p.Name
            });
        }

        public async Task<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(string email, bool includeFavorites = false)
        {
            IEnumerable<PointOfInterestDto> points = (await _pointsOfInterestRepository.GetPointsOfInterestForUser(email, includeFavorites)).Select(p => new PointOfInterestDto()
            {
                Id = p.Id.ToString(),
                Location = new LocationDto()
                {
                    Longitude = p.Location.X,
                    Latitude = p.Location.Y
                },
                Description = p.Description,
                IsFavorite = p.IsFavorite,
                Type = p.Type,
                Name = p.Name
            });
            return points;
        }

        public async Task<IEnumerable<PointOfInterestDto>> GetFavoritePointsOfInterest(string email)
        {
            User user = await _usersRepository.FindByEmailAsync(email);
            IEnumerable<PointOfInterestDto> points = (await _pointsOfInterestRepository.GetFavoritePointsOfInterestForUser(user)).Select(p => new PointOfInterestDto()
            {
                Id = p.Id.ToString(),
                Location = new LocationDto
                {
                    Longitude = p.Location.X,
                    Latitude = p.Location.Y
                },
                Description = p.Description,
                IsFavorite = p.IsFavorite,
                Type = p.Type,
                Name = p.Name

            });
            return points;
        }

        public async Task AddToFavoritesAsync(string username, string id)
        {
            await _usersRepository.AddToFavoritesAsync(username, id);
            await _pointsOfInterestRepository.IncrementFavoritesCount(id);
        }

        public async Task<IEnumerable<PointOfInterestDto>> GetPointsOfInterest()
        {
            IEnumerable<PointOfInterestDto> points = (await _pointsOfInterestRepository.GetAll()).Select(p => new PointOfInterestDto()
            {
                Id = p.Id.ToString(),
                Location = new LocationDto()
                {
                    Longitude = p.Location.X,
                    Latitude = p.Location.Y
                },
                Description = p.Description,
                IsFavorite = p.IsFavorite,
                Type = p.Type,
                FavoritesCount = p.FavoritsCount,
                Name = p.Name
            });
            return points;
        }

        public async Task<IEnumerable<PointOfInterestDto>> GetNearstPointsOfInterest(double latitude, double longitude, int take)
        {
            IEnumerable<PointOfInterestDto> points = (await _pointsOfInterestRepository.GetNearestPointsOfInterest(latitude, longitude, take)).Select(p => new PointOfInterestDto()
            {
                Id = p.Id.ToString(),
                Location = new LocationDto()
                {
                    Longitude = p.Location.X,
                    Latitude = p.Location.Y
                },
                Description = p.Description,
                IsFavorite = p.IsFavorite,
                Type = p.Type,
                Name = p.Name
            });
            return points;
        }

        public async Task RemoveFromFavorites(string email, string id)
        {
            await _usersRepository.RemoveFromFavorites(email, id);
            await _pointsOfInterestRepository.DecrementFavoritesCount(id);
        }

        public async Task<IEnumerable<PointOfInterestDto>> GetByType(string type)
        {
            var points = (await _pointsOfInterestRepository.GetByType(type)).Select(p => new PointOfInterestDto()
            {
                Id = p.Id.ToString(),
                Location = new LocationDto()
                {
                    Longitude = p.Location.X,
                    Latitude = p.Location.Y
                },
                Description = p.Description,
                IsFavorite = p.IsFavorite,
                Type = p.Type,
                Name = p.Name
            });
            return points;
        }
    }
}
