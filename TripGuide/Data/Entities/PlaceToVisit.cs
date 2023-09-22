using TripGuide.Models;

namespace TripGuide.Data.Entities;

public class PlaceToVisit
{
    public int Id { get; set; }
    public int TripId { get; set; }
    public string Name { get; set; }
    public int VisitOrder { get; set; }
    public string Note { get; set; }
    public string Longitude { get; set; }
    public string Latitude { get; set; }

    public PlaceToVisitViewModel MapToViewModel()
    {
        return new PlaceToVisitViewModel
        {
            Id = Id,
            Name = Name,
            Latitude = Latitude,
            Longitude = Longitude,
            Note = Note,
            VisitOrder = VisitOrder
        };
    }
}