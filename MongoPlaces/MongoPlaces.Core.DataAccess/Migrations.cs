using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoPlaces.Core.DataAccess.Entities;

namespace MongoPlaces.Core.DataAccess
{
    static class Migrations
    {
        static readonly IMongoDatabase Database = new MongoClient(ConfigurationManager.ConnectionStrings["MongoPlacesConnection"].ConnectionString).GetDatabase("MongoPlaces");

        public static void CreatePointsOfInterest()
        {
            var collection = Database.GetCollection<BsonDocument>("PointsOfInterest");
            if (collection == null)
            {
                Database.CreateCollection("PointsOfInterest");
            }
        }

        public static void CreateUsers()
        {
            var collection = Database.GetCollection<BsonDocument>("Users");
            if (collection == null)
            {
                Database.CreateCollection("Users");
            }
        }

        public static void CreateLocationTypes()
        {
            var collection = Database.GetCollection<LocationType>("LocationTypes");

            if (collection.Find(l => l.Name == "Restaurant").FirstOrDefault() == null)
            {
                collection.InsertOne(new LocationType() { Name = "Restaurant" });
            }

            if (collection.Find(l => l.Name == "Hotel").FirstOrDefault() == null)
            {
                collection.InsertOne(new LocationType() { Name = "Hotel" });
            }

            if (collection.Find(l => l.Name == "School").FirstOrDefault() == null)
            {
                collection.InsertOne(new LocationType() { Name = "School" });
            }
        }

        public static void EnsureUniqueEmailIndex()
        {
            var collection = Database.GetCollection<User>("Users");
            var keys = Builders<User>.IndexKeys.Ascending(u => u.Email);
            collection.Indexes.CreateOne(keys, new CreateIndexOptions() { Unique = true, Name = "unique_email_1" });
        }

        public static void EnsureGeospacialIndex()
        {
            var collection = Database.GetCollection<PointOfInterest>("PointsOfInterest");
            var keys = Builders<PointOfInterest>.IndexKeys.Geo2D(u => u.Location);
            collection.Indexes.CreateOne(keys);
        }
    }
}
