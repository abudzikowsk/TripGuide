using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripGuide.Data.Repositories;
using TripGuide.Models;

namespace TripGuide.Controllers;

[Authorize]
public class TripsController : Controller
{
    private readonly TripRepository _tripRepository;

    public TripsController(TripRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<TripViewModel>>> GetAllTripsForCurrentlyLoggedInUser()
    {
        var currentlyLoggedInUser = User.FindFirst(ClaimTypes.NameIdentifier).Value;

        var allTrips = await _tripRepository.GetAllByUserIdAsync(currentlyLoggedInUser);

        var result = new List<TripViewModel>();

        foreach (var trip in allTrips)
        {
            result.Add(trip.MapToViewModel());
        }

        return View(result);
    }

    [HttpGet]
    public ActionResult CreateTrip()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<ActionResult> CreateTrip(CreateTripViewModel createTripViewModel)
    {
        if (!TryValidateModel(createTripViewModel))
        {
            return View(createTripViewModel);
        }
        
        var currentlyLoggedInUser = User.FindFirst(ClaimTypes.NameIdentifier).Value;

        await _tripRepository.CreateTripAsync(currentlyLoggedInUser,createTripViewModel.Name, createTripViewModel.Location, createTripViewModel.StartDate, createTripViewModel.EndDate);

        return RedirectToAction("GetAllTripsForCurrentlyLoggedInUser");
    }
}