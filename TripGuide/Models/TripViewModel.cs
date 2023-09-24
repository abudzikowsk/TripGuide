namespace TripGuide.Models;

public class TripViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsPublic { get; set; }
    public decimal TotalCost { get; set; }
}