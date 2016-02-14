using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace MongoPlaces.Core.DataAccess.Entities
{
    public class PointOfInterest
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string User { get; set; }

        public string Name { get; set; }

        public int FavoritsCount { get; set; }

        public bool IsFavorite { get; set; }

        public Location Location { get; set; }
        public string Type { get; set; }

        public string Description { get; set; }

        public WorkingPeriod WorkingPeriod { get; set; }
    }
}
