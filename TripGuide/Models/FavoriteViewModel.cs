namespace TripGuide.Models;

public class FavoriteViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public int TripId { get; set; }
    public int FavoriteCount { get; set; }
}