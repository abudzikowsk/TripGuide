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
            .Include(t => t.PlacesToVisit)
            .Where(t => t.UserId == userId)
            .ToListAsync();
    }

    public async Task<Trip> GetTripByIdAsync(int id)
    {
        return await _applicationDbContext.Trips
            .Include(t => t.PlacesToVisit)
            .SingleOrDefaultAsync(t => t.Id == id);
    }

    public async Task CreateTripAsync(string userId, string name, string location, DateTime startDate, DateTime endDate)
    {
        var newTrip = new Trip
        {
            UserId = userId,
            Name = name,
            Location = location,
            StartDate = startDate,
            EndDate = endDate
        };

        _applicationDbContext.Add(newTrip);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task DeleteTripAsync(int id)
    {
        var tripToDeletion = _applicationDbContext.Trips
            .Include(t => t.PlacesToVisit)
            .SingleOrDefault(t => t.Id == id);

        if (tripToDeletion == null)
        {
            return;
        }
        
        _applicationDbContext.PlacesToVisit.RemoveRange(tripToDeletion.PlacesToVisit);
        
        _applicationDbContext.Trips.Remove(tripToDeletion);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task CreatePlaceToVisit(
        int tripId, 
        string name, 
        int visitOrder, 
        string note, 
        int longitude, 
        int latitude)
    {
        var trip = await _applicationDbContext.Trips.SingleOrDefaultAsync(t => t.Id == tripId);
        
        if (trip == null)
        {
            return;
        }

        var newPlace = new PlaceToVisit
        {
            TripId = tripId,
            Name = name,
            VisitOrder = visitOrder,
            Note = note,
            Longitude = longitude,
            Latitude = latitude
        };
        _applicationDbContext.PlacesToVisit.Add(newPlace);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task DeletePlaceToVisitAsync(int id)
    {
        var placeToDelete = await _applicationDbContext.PlacesToVisit
            .SingleOrDefaultAsync((p => p.Id == id));

        if (placeToDelete == null)
        {
            return;
        }

        _applicationDbContext.PlacesToVisit.Remove(placeToDelete);
        await _applicationDbContext.SaveChangesAsync();
    }
}