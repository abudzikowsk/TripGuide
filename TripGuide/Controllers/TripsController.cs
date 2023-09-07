using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripGuide.Data.Entities;
using TripGuide.Data.Repositories;
using TripGuide.Enums;
using TripGuide.Models;

namespace TripGuide.Controllers;

[Authorize]
[Route("{controller}")]
public class TripsController : Controller
{
    private readonly TripRepository _tripRepository;

    public TripsController(TripRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }

    [HttpGet]
    [Route("{action}")]
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
    [Route("{action}")]
    public ActionResult CreateTrip()
    {
        return View();
    }

    [HttpPost]
    [Route("{action}")]
    public async Task<ActionResult> CreateTrip(CreateTripViewModel createTripViewModel)
    {
        if (!TryValidateModel(createTripViewModel))
        {
            return View(createTripViewModel);
        }

        var currentlyLoggedInUser = User.FindFirst(ClaimTypes.NameIdentifier).Value;

        await _tripRepository.CreateTripAsync(currentlyLoggedInUser, createTripViewModel.Name,
            createTripViewModel.Location, createTripViewModel.StartDate, createTripViewModel.EndDate);

        return RedirectToAction("GetAllTripsForCurrentlyLoggedInUser");
    }

    [HttpGet]
    [Route("{action}/{id:int}")]
    public async Task<ActionResult> TripDetails(int id)
    {
        var trip = await _tripRepository.GetTripByIdAsync(id);

        return View(trip.MapToDetailsViewModel());
    }

    [HttpGet]
    [Route("{action}/{tripId:int}")]
    public ActionResult CreatePlaceToVisit()
    {
        return View();
    }

    [HttpPost]
    [Route("{action}/{tripId:int}")]
    public async Task<ActionResult> CreatePlaceToVisit(CreatePlaceToVisitViewModel createPlaceToVisitViewModel,
        int tripId)
    {
        if (!TryValidateModel(createPlaceToVisitViewModel))
        {
            return View(createPlaceToVisitViewModel);
        }

        var trip = await _tripRepository.GetTripByIdAsync(tripId);

        if (trip is null)
        {
            return RedirectToAction("GetAllTripsForCurrentlyLoggedInUser");
        }

        await _tripRepository.CreatePlaceToVisitAsync(
            tripId,
            createPlaceToVisitViewModel.Name,
            createPlaceToVisitViewModel.VisitOrder,
            createPlaceToVisitViewModel.Note,
            createPlaceToVisitViewModel.Longitude,
            createPlaceToVisitViewModel.Latitude);

        return RedirectToAction("TripDetails", new { id = tripId });
    }

    [HttpPost]
    [Route("{action}/{tripId:int}/{placeToVisitId:int}")]
    public async Task<ActionResult> DeletePlaceToVisit(int tripId, int placeToVisitId)
    {
        await _tripRepository.DeletePlaceToVisitAsync(placeToVisitId);

        return RedirectToAction("TripDetails", new { id = tripId });
    }

    [HttpPost]
    [Route("{action}/{tripId:int}")]
    public async Task<ActionResult> DeleteTrip(int tripId)
    {
        await _tripRepository.DeleteTripAsync(tripId);

        return RedirectToAction("GetAllTripsForCurrentlyLoggedInUser");
    }

    [HttpPost]
    [Route("{action}/{tripId:int}/{source}")]
    public async Task<ActionResult> SwitchTripStatusPublicity(int tripId,string source)
    {
        await _tripRepository.SwitchTripPublicityStatus(tripId);

        if (source == SourceEnum.TripList.ToString())
        {
            return RedirectToAction("GetAllTripsForCurrentlyLoggedInUser");
        }
        else
        {
            return RedirectToAction("TripDetails", new { id = tripId});
        }
    }
}