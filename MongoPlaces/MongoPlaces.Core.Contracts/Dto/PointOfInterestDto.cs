using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoPlaces.Core.Contracts.Dto
{
    public class PointOfInterestDto
    {
        public LocationDto Location { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string User { get; set; }
        public bool IsFavorite { get; set; }
        public string Id { get; set; }
        public int FavoritesCount { get; set; }
    }
}
