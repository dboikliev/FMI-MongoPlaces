using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace MongoPlaces.Web.Models
{
    public class PointOfInterestViewModel
    {
        public string Id { get; set; }
        [Required]
        public LocationViewModel Location { get; set; }
        public string Label { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Required]
        public string SelectedType { get; set; }
        [Required]
        public string Name { get; set; }
        public IEnumerable<string> Types { get; set; }
        public int FavoritesCount { get; set; }
    }
}