using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MongoPlaces.Core.Contracts.Dto;
using MongoPlaces.Core.Services.Interfaces;
using MongoPlaces.Web.Models;

namespace MongoPlaces.Web.Controllers
{
    public class MapController : Controller
    {
        private readonly IPointsOfInterestService _pointsOfInterestService;
        private readonly ILocationTypesService _locationTypesService;

        public MapController(IPointsOfInterestService pointsOfInterestService, ILocationTypesService locationTypesService)
        {
            _pointsOfInterestService = pointsOfInterestService;
            _locationTypesService = locationTypesService;
        }

        public async Task<ActionResult> Index()
        {
            IEnumerable<PointOfInterestViewModel> points = (await _pointsOfInterestService.GetPointsOfInterest()).Select(p => new PointOfInterestViewModel()
            {
                Id = p.Id,
                Description = p.Description,
                Location = new LocationViewModel
                {
                    Latitude = p.Location.Latitude,
                    Longitude = p.Location.Longitude
                }
            });
            ViewData["LocationTypes"] = await _locationTypesService.GetAllAsync();
            return View("Index", points);
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddPointOfInterest(PointOfInterestViewModel model)
        {
            await _pointsOfInterestService.AddPointOfInterestAsync(new PointOfInterestDto
            {
                Description = model.Description,
                Location = new LocationDto
                {
                    Latitude = model.Location.Latitude,
                    Longitude = model.Location.Longitude
                },
                User = User.Identity.Name,
                Type = model.SelectedType
            });
            return RedirectToAction("Index");
        }
        [Authorize]
        public async Task<ActionResult> List()
        {
            var points = (await _pointsOfInterestService.GetPointsOfInterest(User.Identity.Name, includeFavorites: false)).Select(p => new PointOfInterestViewModel
            {
                Id = p.Id,
                Description = p.Description,
                Location = new LocationViewModel { Latitude = p.Location.Latitude, Longitude = p.Location.Longitude },
                SelectedType = p.Type
            });
            return View(points);
        }

        [Authorize]
        public async Task<ActionResult> Favorites()
        {
            var points = (await _pointsOfInterestService.GetFavoritePointsOfInterest(User.Identity.Name)).Select(p => new PointOfInterestViewModel
            {
                Id = p.Id,
                Description = p.Description,
                Location = new LocationViewModel { Latitude = p.Location.Latitude, Longitude = p.Location.Longitude },
                SelectedType = p.Type
            });
            return View(points);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddToFavorites(string id)
        {
            await _pointsOfInterestService.AddToFavoritesAsync(User.Identity.Name, id);
            return RedirectToAction("List");
        }

        public async Task<ActionResult> Nearest(double latitude, double longitude, int take = 5)
        {
            var points = (await _pointsOfInterestService.GetNearstPointsOfInterest(latitude, longitude, take)).Select(p => new PointOfInterestViewModel
            {
                Id = p.Id,
                Description = p.Description,
                Location = new LocationViewModel { Latitude = p.Location.Latitude, Longitude = p.Location.Longitude },
                SelectedType = p.Type
            });
            return View("List", new PointsOfInterestListViewModel() { NonFavoritePoints = points, FavoritePoints = Enumerable.Empty<PointOfInterestViewModel>()});
        }
    }
}