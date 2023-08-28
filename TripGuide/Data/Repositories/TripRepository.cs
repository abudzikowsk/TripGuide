using Microsoft.EntityFrameworkCore;
using TripGuide.Data.Entities;

namespace TripGuide.Data.Repositories;

public class TripRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public TripRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<List<Trip>> GetAllByUserIdAsync(string userId)
    {
        return await _applicationDbContext.Trips
            .Include(x => x.PlacesToVisit)
            .Where(y => y.UserId == userId)
            .ToListAsync();
    }

    public async Task<Trip> GetTripByIdAsync(int id)
    {
        return await _applicationDbContext.Trips
            .Include(t => t.PlacesToVisit)
            .SingleOrDefaultAsync(t => t.Id == id);
    }
}