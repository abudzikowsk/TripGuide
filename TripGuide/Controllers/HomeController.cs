using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> Index()
    {
        var allTrips = await _tripRepository.GetAllPublicTrips();
        
        var result = new List<TripViewModel>();

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        List<Favorite> userFavorites = null;
        if (userId != null)
        {
            userFavorites = await _favoriteRepository.GetAllFavoritesByUserIdAsync(userId);
        }
        
        foreach (var trip in allTrips)
        {
            var tripViewModel = trip.MapToViewModel();
            tripViewModel.IsCreatedByCurrentUser = trip.UserId == userId;
            if (userFavorites != null)
            {
                var favorite = userFavorites.SingleOrDefault(f => f.TripId == trip.Id);
                tripViewModel.IsAlreadyCurrentUserFavorite = favorite != null;
            }
            result.Add(tripViewModel);
        }
        
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

        var tripViewModel = trip.MapToDetailsViewModel();
        tripViewModel.IsCreatedByCurrentUser = trip.UserId == userId;
        
        if (userFavorites != null)
        {
            var favorite = userFavorites.SingleOrDefault(f => f.TripId == trip.Id);
            tripViewModel.IsAlreadyCurrentUserFavorite = favorite != null;
        }
        
        return View(tripViewModel);
    }
}

