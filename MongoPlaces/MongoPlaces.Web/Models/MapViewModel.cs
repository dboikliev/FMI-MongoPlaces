using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoPlaces.Web.Models
{
    public class MapViewModel
    {
        public IEnumerable<PointOfInterestViewModel> Points { get; set; }
    }
}