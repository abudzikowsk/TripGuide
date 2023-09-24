using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TripGuide.Data.Entities;
using TripGuide.Data.Repositories;
using TripGuide.Models;

namespace TripGuide.Controllers;

public class HomeController : Controller
{
    private readonly TripRepository _tripRepository;
    private readonly FavoriteRepository _favoriteRepository;

    public HomeController(TripRepository tripRepository, FavoriteRepository favoriteRepository)
    {
        _tripRepository = tripRepository;
        _favoriteRepository = favoriteRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index(List<string> citiesToFilter, List<string> countriesToFilter)
    {
        var allTrips = await _tripRepository.GetAllPublicTrips(citiesToFilter, countriesToFilter);
        
        var result = new IndexViewModel();
        result.Trips = new List<IndexTripViewModel>();
        result.AllCitites = new List<SelectListItem>();
        result.AllCountries = new List<SelectListItem>();

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        List<Favorite> userFavorites = null;
        if (userId != null)
        {
            userFavorites = await _favoriteRepository.GetAllFavoritesByUserIdAsync(userId);
        }
        
        foreach (var trip in allTrips)
        {
            var indexTripViewModel = trip.MapToIndexTripViewModel();
            indexTripViewModel.IsCreatedByCurrentUser = trip.UserId == userId;
            indexTripViewModel.FavoriteCount = await _favoriteRepository.CountTripFavoritesAsync(trip.Id);
            if (userFavorites != null)
            {
                var favorite = userFavorites.SingleOrDefault(f => f.TripId == trip.Id);
                indexTripViewModel.IsAlreadyCurrentUserFavorite = favorite != null;
            }
            result.Trips.Add(indexTripViewModel);
        }

        var allCitites = await _tripRepository.GetAllTripCitiesAsync();
        foreach (var city in allCitites)
        {
            result.AllCitites.Add(new SelectListItem(city,city.ToLower()));
        }

        var allCountries = await _tripRepository.GetAllTripCountriesAsync();
        foreach (var country in allCountries)
        {
            result.AllCountries.Add(new SelectListItem(country, country.ToLower()));
        }
        
        var tripsSorted = result.Trips.OrderByDescending(f => f.FavoriteCount).ToList();
        result.Trips = tripsSorted;
        return View(result);
        
    }
    
    [HttpGet]
    public IActionResult Privacy()
    {
        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult> HomeTripDetails(int id)
    {
        var trip = await _tripRepository.GetTripByIdAsync(id);

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        List<Favorite> userFavorites = null;
        if (userId != null)
        {
            userFavorites = await _favoriteRepository.GetAllFavoritesByUserIdAsync(userId);
        }

        var indexViewModel = trip.MapToDetailsViewModel();
        indexViewModel.IsCreatedByCurrentUser = trip.UserId == userId;
        indexViewModel.FavoriteCount = await _favoriteRepository.CountTripFavoritesAsync(trip.Id);
        
        if (userFavorites != null)
        {
            var favorite = userFavorites.SingleOrDefault(f => f.TripId == trip.Id);
            indexViewModel.IsAlreadyCurrentUserFavorite = favorite != null;
        }
        
        return View(indexViewModel);
    }
}

