namespace TripGuide.Models;

public class IndexTripViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsPublic { get; set; }
    public bool IsCreatedByCurrentUser { get; set; }
    public bool IsAlreadyCurrentUserFavorite { get; set; }
    public int FavoriteCount { get; set; }
}