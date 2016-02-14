using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoPlaces.Core.DataAccess.Entities
{
    public class LocationType
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonRequired]
        public string Name { get; set; }
    }
}
