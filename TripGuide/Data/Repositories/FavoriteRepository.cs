using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using TripGuide.Data.Entities;

namespace TripGuide.Data.Repositories;

public class FavoriteRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public FavoriteRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<List<Favorite>> GetAllFavoritesByUserIdAsync(string userId)
    {
        return await _applicationDbContext.Favorites
            .Include(f => f.Trip)
            .Where(f => f.UserId == userId)
            .ToListAsync();
    }

    public async Task DeleteFavoriteAsync(int tripId, string userId)
    {
        var favoriteToDelete = await _applicationDbContext.Favorites
            .SingleOrDefaultAsync(f => f.TripId == tripId && f.UserId == userId);

        if (favoriteToDelete == null)
        {
            return;
        }

        _applicationDbContext.Favorites.Remove(favoriteToDelete);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task CreateFavoriteAsync(int tripId, string userId)
    {
        var trip = await _applicationDbContext.Trips.SingleOrDefaultAsync(t => t.Id == tripId);
        if (trip == null)
        {
            return;
        }

        var user = await _applicationDbContext.Users.SingleOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return;
        }
        
        var favorite = new Favorite
        {
            TripId = tripId,
            UserId = userId
        };

        _applicationDbContext.Add(favorite);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<bool> IsAlreadyFavoriteForUserAsync(int tripId, string userId)
    {
        var favorite =
            await _applicationDbContext.Favorites.SingleOrDefaultAsync(f => f.TripId == tripId && f.UserId == userId);
        if (favorite == null)
        {
            return false;
        }
        return true;
    }

    public async Task<int> CountTripFavoritesAsync(int tripId)
    {
        var favoriteCount = await _applicationDbContext.Favorites.Where(f => f.TripId == tripId).CountAsync();
        return favoriteCount;
    }
}