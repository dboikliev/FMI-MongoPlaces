using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoPlaces.Core.DataAccess.Entities
{
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonRequired]
        public string Email { get; set; }

        [BsonRequired]
        public string PasswordHash { get; set; }
        [BsonRequired]
        public string SaltHash { get; set; }

        public ObjectId[] FavoritePlaces { get; set; }
    }
}
