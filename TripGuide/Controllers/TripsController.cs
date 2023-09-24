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
            createTripViewModel.City, createTripViewModel.Country, createTripViewModel.StartDate, createTripViewModel.EndDate);

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
    public async Task<ActionResult> CreatePlaceToVisit(int tripId)
    {
        var trip = await _tripRepository.GetTripByIdAsync(tripId);

        if (trip == null)
        {
            return RedirectToAction("GetAllTripsForCurrentlyLoggedInUser");
        }

        var createPlaceToVisitViewModel = new CreatePlaceToVisitViewModel()
        {
            City = trip.City,
            Country = trip.Country
        };
        
        return View(createPlaceToVisitViewModel);
    }

    [HttpPost]
    [Route("{action}/{tripId:int}")]
    public async Task<ActionResult> CreatePlaceToVisit(CreatePlaceToVisitViewModel createPlaceToVisitViewModel, int tripId)
    {
        var trip = await _tripRepository.GetTripByIdAsync(tripId);

        if (trip is null)
        {
            return RedirectToAction("GetAllTripsForCurrentlyLoggedInUser");
        }
        
        if (!TryValidateModel(createPlaceToVisitViewModel))
        {
            createPlaceToVisitViewModel.City = trip.City;
            createPlaceToVisitViewModel.Country = trip.Country;
            return View(createPlaceToVisitViewModel);
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

    [HttpGet]
    [Route("{action}/{id:int}")]
    public async Task<ActionResult> EditTrip(int id)
    {
        var trip = await _tripRepository.GetTripByIdAsync(id);

        var editTripViewModel = new EditTripViewModel
        {
            Name = trip.Name,
            City = trip.City,
            Country = trip.Country,
            StartDate = trip.StartDate,
            EndDate = trip.EndDate
        };

        return View(editTripViewModel);
    }

    [HttpPost]
    [Route("{action}/{id:int}")]
    public async Task<ActionResult> EditTrip(int id, EditTripViewModel editTripViewModel)
    {
        var trip = await _tripRepository.GetTripByIdAsync(id);

        if (trip == null)
        {
            return RedirectToAction("GetAllTripsForCurrentlyLoggedInUser");
        }

        if (!TryValidateModel(editTripViewModel))
        {
            return View(editTripViewModel);
        }

        await _tripRepository.EditTrip(
            id, editTripViewModel.Name, editTripViewModel.City, editTripViewModel.Country, editTripViewModel.StartDate,
            editTripViewModel.EndDate);

        return RedirectToAction("TripDetails", new {id = id});
    }

    [HttpGet]
    [Route("{action}/{placeToVisitId:int}")]
    public async Task<ActionResult> EditPlaceToVisit(int placeToVisitId)
    {
        var placeToVisit = await _tripRepository.GetPlaceToVisitByIdAsync(placeToVisitId);

        var editPlaceToVisitViewModel = new EditPlaceToVisitViewModel
        {
            Name = placeToVisit.Name,
            City = placeToVisit.Trip.City,
            Country = placeToVisit.Trip.Country,
            VisitOrder = placeToVisit.VisitOrder,
            Longitude = placeToVisit.Longitude,
            Latitude = placeToVisit.Latitude,
            Note = placeToVisit.Note
        };

        return View(editPlaceToVisitViewModel);
    }

    [HttpPost]
    [Route("{action}/{placeToVisitId:int}")]
    public async Task<ActionResult> EditPlaceToVisit(int placeToVisitId,
        EditPlaceToVisitViewModel editPlaceToVisitViewModel)
    {
        var placeToVisit = await _tripRepository.GetPlaceToVisitByIdAsync(placeToVisitId);

        if (placeToVisit == null)
        {
            return RedirectToAction("GetAllTripsForCurrentlyLoggedInUser");
        }

        if (!TryValidateModel(editPlaceToVisitViewModel))
        {
            return View(editPlaceToVisitViewModel);
        }

        await _tripRepository.EditPlaceToVisit(
            placeToVisitId, editPlaceToVisitViewModel.Name, editPlaceToVisitViewModel.VisitOrder,
            editPlaceToVisitViewModel.Note, editPlaceToVisitViewModel.Longitude, editPlaceToVisitViewModel.Latitude);

        return RedirectToAction("TripDetails", new { id = placeToVisit.TripId });
    }
}