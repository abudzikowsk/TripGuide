using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TripGuide.Data.Repositories;
using TripGuide.Models;

namespace TripGuide.Controllers;

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
            result.Add(favorite.MapToViewModel());
        }

        return View(result);
    }

    [HttpPost]
    [Route("{action}/{id:int}")]
    public async Task<ActionResult> DeleteFavorite(int id)
    {
        await _favoriteRepository.DeleteFavoriteAsync(id);
        return RedirectToAction("GetAllFavoritesByUserId");
    }

    [HttpPost]
    [Route("{action}/{id:int}")]
    public async Task<ActionResult> CreateFavorite(int id)
    {
        var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;

        var isAlreadyFavoriteForUserAsync = await _favoriteRepository.IsAlreadyFavoriteForUserAsync(id, user);

        if (!isAlreadyFavoriteForUserAsync)
        {
            await _favoriteRepository.CreateFavoriteAsync(id, user);
        }
        return RedirectToAction("Index", "Home");
    }
}