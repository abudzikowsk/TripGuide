using TripGuide.Models;

namespace TripGuide.Data.Entities;

public class Trip
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public bool IsPublic { get; set; }
    
    public List<PlaceToVisit> PlacesToVisit { get; set; }

    public TripViewModel MapToViewModel()
    {
        return new TripViewModel
        {
            Id = Id,
            Name = Name,
            Location = Location,
            StartDate = StartDate,
            EndDate = EndDate,
            IsPublic = IsPublic
        };
    }

    public TripDetailsViewModel MapToDetailsViewModel()
    {
        var result = new TripDetailsViewModel
        {
            Id = Id,
            Name = Name,
            Location = Location,
            StartDate = StartDate,
            EndDate = EndDate,
            IsPublic = IsPublic,
            PlacesToVisit = new List<PlaceToVisitViewModel>()
        };

        foreach (var placeToVisit in PlacesToVisit)
        { 
            result.PlacesToVisit.Add(placeToVisit.MapToViewModel());
        }

        return result;
    }
}