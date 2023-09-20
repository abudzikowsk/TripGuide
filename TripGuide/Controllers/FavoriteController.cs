using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripGuide.Data.Repositories;
using TripGuide.Enums;
using TripGuide.Models;

namespace TripGuide.Controllers;

[Authorize]
public class FavoriteController : Controller
{
    private readonly FavoriteRepository _favoriteRepository;

    public FavoriteController(FavoriteRepository favoriteRepository)
    {
        _favoriteRepository = favoriteRepository;
    }

    [HttpGet]
    [Route("{action}")]
    public async Task<ActionResult<List<FavoriteViewModel>>> GetAllFavoritesByUserId()
    {
        var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;

        var favorites = await _favoriteRepository.GetAllFavoritesByUserIdAsync(user);

        var result = new List<FavoriteViewModel>();
        
        foreach (var favorite in favorites)
        {
            var favoriteViewModel = favorite.MapToViewModel();
            favoriteViewModel.FavoriteCount = await _favoriteRepository.CountTripFavoritesAsync(favorite.TripId);
            result.Add(favoriteViewModel);
        }
        return View(result);
    }

    [HttpPost]
    [Route("{action}/{tripId:int}/{source}")]
    public async Task<ActionResult> DeleteFavorite(int tripId, SourceEnum source)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        await _favoriteRepository.DeleteFavoriteAsync(tripId, userId);

        switch (source)
        {
            case SourceEnum.TripList:
                return RedirectToAction("Index", "Home");
            case SourceEnum.TripDetails:
                return RedirectToAction("HomeTripDetails", "Home", new {id = tripId});
            case SourceEnum.FavoriteList:
                return RedirectToAction("GetAllFavoritesByUserId");
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [Route("{action}/{tripId:int}/{source}")]
    public async Task<ActionResult> CreateFavorite(int tripId, string source)
    {
        var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;

        var isAlreadyFavoriteForUserAsync = await _favoriteRepository.IsAlreadyFavoriteForUserAsync(tripId, user);

        if (!isAlreadyFavoriteForUserAsync)
        {
            await _favoriteRepository.CreateFavoriteAsync(tripId, user);
        }

        if (source == SourceEnum.TripList.ToString())
        {
            return RedirectToAction("Index", "Home");
        }

        return RedirectToAction("HomeTripDetails", "Home", new {id = tripId});
    }
}