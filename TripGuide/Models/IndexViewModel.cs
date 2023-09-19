using Microsoft.AspNetCore.Mvc.Rendering;

namespace TripGuide.Models;

public class IndexViewModel
{
    public List<IndexTripViewModel> Trips { get; set; }
    public List<SelectListItem> AllLocations { get; set; }
}