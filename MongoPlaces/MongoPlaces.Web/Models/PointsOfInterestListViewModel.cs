using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoPlaces.Web.Models
{
    public class PointsOfInterestListViewModel
    {
        public IEnumerable<PointOfInterestViewModel> NonFavoritePoints { get; set; }
        public IEnumerable<PointOfInterestViewModel> FavoritePoints { get; set; }
    }
}