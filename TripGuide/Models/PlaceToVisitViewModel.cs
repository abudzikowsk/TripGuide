namespace TripGuide.Models;

public class PlaceToVisitViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int VisitOrder { get; set; }
    public string Note { get; set; }
    public string Longitude { get; set; }
    public string Latitude { get; set; }
    public decimal Cost { get; set; }
}