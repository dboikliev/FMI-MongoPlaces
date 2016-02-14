using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using MongoPlaces.Core.Contracts.Dto;
using MongoPlaces.Core.DataAccess.Entities;
using MongoPlaces.Core.DataAccess.Interfaces;

namespace MongoPlaces.Core.DataAccess.Implementations
{
    public class PointsOfInterestRepository : AbstractRepository<PointOfInterest>, IPointsOfInterestRepository
    {
        protected override string CollectionName => "PointsOfInterest";

        public async Task CreateAsync(PointOfInterest poi)
        {
            await Collection.InsertOneAsync(poi);
        }

        public async Task<IEnumerable<PointOfInterest>> FindNear(PointOfInterest poi, double? maxDistance = null)
        {
            var cursor = await Collection.FindAsync(Builders<PointOfInterest>.Filter.Near(p => p.Location, poi.Location.X, poi.Location.Y, maxDistance));
            return await cursor.ToListAsync();
        }

        public async Task<PointOfInterest> Find(PointOfInterest poi)
        {
            var cursor = await Collection.FindAsync(Builders<PointOfInterest>.Filter.Eq(p => p.Location, poi.Location));
            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetAll()
        {
            var cursor = await Collection.FindAsync(FilterDefinition<PointOfInterest>.Empty);
            return await cursor.ToListAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForUser(string username, bool includeFavorites = false)
        {
            var cursor = await Collection.FindAsync(Builders<PointOfInterest>.Filter.And(
                Builders<PointOfInterest>.Filter.Eq(p => p.User, username), Builders<PointOfInterest>.Filter.Eq(p => p.IsFavorite, includeFavorites)));
            return await cursor.ToListAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetFavoritePointsOfInterestForUser(string username)
        {
            var cursor = await Collection.FindAsync(Builders<PointOfInterest>.Filter.And(Builders<PointOfInterest>.Filter.Eq(p => p.IsFavorite, true)));
            return await cursor.ToListAsync();
        }

        public async Task SetIsFavorite(string id, string username, bool isFavorite)
        {
            await Collection.UpdateOneAsync(Builders<PointOfInterest>.Filter.And(
                Builders<PointOfInterest>.Filter.Eq(p => p.Id, new ObjectId(id)),
                Builders<PointOfInterest>.Filter.Eq(p => p.User, username)),
                Builders<PointOfInterest>.Update.Set(p => p.IsFavorite, isFavorite));
        }

        public async Task<IEnumerable<PointOfInterest>> GetNearestPointsOfInterest(double latitude, double longitude, int take)
        {
            //var cursor = Collection.Aggregate().Match($"{{$geoNear: {{ near: [{longitude}, {latitude}]}}}}").Limit(take);
            var cursor = await
                Collection.FindAsync(Builders<PointOfInterest>.Filter.Near(p => p.Location, latitude, longitude));
            return (await cursor.ToListAsync()).Take(take);
        }
    }
}
