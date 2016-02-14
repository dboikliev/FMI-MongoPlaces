using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoPlaces.Core.Contracts.Dto;

namespace MongoPlaces.Core.Services.Interfaces
{
    public interface IPointsOfInterestService
    {
        Task AddPointOfInterestAsync(PointOfInterestDto poi);
        Task<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(string email, bool includeFavorites = false);
        Task<IEnumerable<PointOfInterestDto>> GetFavoritePointsOfInterest(string email);
        Task AddToFavoritesAsync(string username, string id);
        Task<IEnumerable<PointOfInterestDto>> GetPointsOfInterest();
        Task<IEnumerable<PointOfInterestDto>> GetNearstPointsOfInterest(double latitude, double longitude, int take);
        Task RemoveFromFavorites(string name, string id);
    }
}
