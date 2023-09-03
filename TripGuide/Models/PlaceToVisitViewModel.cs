namespace TripGuide.Models;

public class PlaceToVisitViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int VisitOrder { get; set; }
    public string Note { get; set; }
    public int Longitude { get; set; }
    public int Latitude { get; set; }
}