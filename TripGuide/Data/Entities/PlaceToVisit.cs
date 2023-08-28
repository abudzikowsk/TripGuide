namespace TripGuide.Data.Entities;

public class PlaceToVisit
{
    public int Id { get; set; }
    public int TripId { get; set; }
    public string Name { get; set; }
    public int VisitOrder { get; set; }
    public string Note { get; set; }
    public int Longitude { get; set; }
    public int Latitude { get; set; }
}