using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TripGuide.Data.Repositories;
using TripGuide.Models;

namespace TripGuide.Controllers;

public class HomeController : Controller
{
    private readonly TripRepository _tripRepository;

    public HomeController(TripRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var allTrips = await _tripRepository.GetAllPublicTrips();
        
        var result = new List<TripViewModel>();

        foreach (var trip in allTrips)
        {
            result.Add(trip.MapToViewModel());
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
}

