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

    public async Task<List<Trip>> GetAllPublicTrips(List<string> citiesToFilter = null, List<string> countriesToFilter = null)
    {
        var query = _applicationDbContext.Trips
            .Include(t => t.PlacesToVisit)
            .Where(t => t.IsPublic);

        if (citiesToFilter != null && citiesToFilter.Count > 0)
        {
            query = query.Where(q => citiesToFilter.Contains(q.City.ToLower()));
        }

        if (countriesToFilter != null && countriesToFilter.Count > 0)
        {
            query = query.Where(q => countriesToFilter.Contains(q.Country.ToLower()));
        }

        return await query.ToListAsync();
    }
    public async Task<List<string>> GetAllTripCitiesAsync()
    {
        return await _applicationDbContext.Trips
            .Where(a => a.IsPublic)
            .Select(a => a.City)
            .ToListAsync();
    }

    public async Task<List<string>> GetAllTripCountriesAsync()
    {
        return await _applicationDbContext.Trips
            .Where(a => a.IsPublic)
            .Select(a => a.Country)
            .ToListAsync();
    }

    public async Task<Trip> GetTripByIdAsync(int id)
    {
        return await _applicationDbContext.Trips
            .Include(t => t.PlacesToVisit)
            .SingleOrDefaultAsync(t => t.Id == id);
    }

    public async Task CreateTripAsync(string userId, string name, string city, string country,DateTime startDate, DateTime endDate)
    {
        var newTrip = new Trip
        {
            UserId = userId,
            Name = name,
            City = city,
            Country = country,
            StartDate = startDate,
            EndDate = endDate
        };

        _applicationDbContext.Add(newTrip);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task DeleteTripAsync(int id)
    {
        var tripToDeletion = await _applicationDbContext.Trips
            .Include(t => t.PlacesToVisit)
            .SingleOrDefaultAsync(t => t.Id == id);

        if (tripToDeletion == null)
        {
            return;
        }
        
        _applicationDbContext.PlacesToVisit.RemoveRange(tripToDeletion.PlacesToVisit);
        
        _applicationDbContext.Trips.Remove(tripToDeletion);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task CreatePlaceToVisitAsync(
        int tripId, 
        string name, 
        int visitOrder, 
        string note, 
        string longitude, 
        string latitude)
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

    public async Task SwitchTripPublicityStatus(int tripId)
    {
        var trip = await _applicationDbContext.Trips.SingleOrDefaultAsync(p => p.Id == tripId);

        if (trip == null)
        {
            return;
        }

        trip.IsPublic = !trip.IsPublic;
        await _applicationDbContext.SaveChangesAsync();
    }
}