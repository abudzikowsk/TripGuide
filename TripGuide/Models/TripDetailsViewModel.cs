namespace TripGuide.Models;

public class TripDetailsViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public bool IsPublic { get; set; }
    
    public List<PlaceToVisitViewModel> PlacesToVisit { get; set; }
}