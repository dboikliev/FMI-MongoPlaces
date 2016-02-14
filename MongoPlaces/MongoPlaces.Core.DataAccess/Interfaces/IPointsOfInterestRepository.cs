using System.Collections.Generic;
using System.Threading.Tasks;
using MongoPlaces.Core.DataAccess.Entities;

namespace MongoPlaces.Core.DataAccess.Interfaces
{
    public interface IPointsOfInterestRepository
    {
        Task CreateAsync(PointOfInterest poi);
        Task<IEnumerable<PointOfInterest>> FindNear(PointOfInterest poi, double? maxDistance = null);
        Task<PointOfInterest> Find(PointOfInterest poi);
        Task<IEnumerable<PointOfInterest>> GetAll();
        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForUser(string username, bool includeFavorites = false);
        Task<IEnumerable<PointOfInterest>> GetFavoritePointsOfInterestForUser(string username);
        Task SetIsFavorite(string id, string username, bool isFavorite);
        Task<IEnumerable<PointOfInterest>> GetNearestPointsOfInterest(double latitude, double longitude, int take);
    }
}
