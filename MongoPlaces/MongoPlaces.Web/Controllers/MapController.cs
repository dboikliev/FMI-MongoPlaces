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
                },
                FavoritesCount = p.FavoritesCount,
                Name = p.Name,
                SelectedType = p.Type
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
                Type = model.SelectedType,
                Name = model.Name
            });
            return RedirectToAction("Index");
        }
        [Authorize]
        public async Task<ActionResult> List()
        {
            var points = (await _pointsOfInterestService.GetPointsOfInterest()).Select(p => new PointOfInterestViewModel
            {
                Id = p.Id,
                Description = p.Description,
                Location = new LocationViewModel { Latitude = p.Location.Latitude, Longitude = p.Location.Longitude },
                SelectedType = p.Type,
                FavoritesCount = p.FavoritesCount,
                Name = p.Name
            });
            return View(points);
        }

        [Authorize]
        public async Task<ActionResult> ListFavorites()
        {
            var points = (await _pointsOfInterestService.GetFavoritePointsOfInterest(User.Identity.Name)).Select(p => new PointOfInterestViewModel
            {
                Id = p.Id,
                Description = p.Description,
                Location = new LocationViewModel { Latitude = p.Location.Latitude, Longitude = p.Location.Longitude },
                SelectedType = p.Type,
                Name = p.Name
            });
            return View(points);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddToFavorites(string id)
        {
            await _pointsOfInterestService.AddToFavoritesAsync(User.Identity.Name, id);
            return RedirectToAction("ListFavorites");
        }

        public async Task<ActionResult> Nearest(double latitude, double longitude, int take = 5)
        {
            var points = (await _pointsOfInterestService.GetNearstPointsOfInterest(latitude, longitude, take)).Select(p => new PointOfInterestViewModel
            {
                Id = p.Id,
                Description = p.Description,
                Location = new LocationViewModel { Latitude = p.Location.Latitude, Longitude = p.Location.Longitude },
                SelectedType = p.Type,
                Name = p.Name
            });
            return View("List", points);
        }

        public ActionResult Remove(string id)
        {
            _pointsOfInterestService.RemoveFromFavorites(User.Identity.Name, id);
            return RedirectToAction("ListFavorites");
        }

        public async Task<ActionResult> ListSimilar(string type)
        {
            var points = (await _pointsOfInterestService.GetByType(type)).Select(p => new PointOfInterestViewModel
            {
                Id = p.Id,
                Description = p.Description,
                Location = new LocationViewModel { Latitude = p.Location.Latitude, Longitude = p.Location.Longitude },
                SelectedType = p.Type,
                Name = p.Name
            });
            return View("List", points);
        }
    }
}