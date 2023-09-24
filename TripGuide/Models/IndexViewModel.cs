using Microsoft.AspNetCore.Mvc.Rendering;

namespace TripGuide.Models;

public class IndexViewModel
{
    public List<IndexTripViewModel> Trips { get; set; }
    public List<SelectListItem> AllCitites { get; set; }

    public List<SelectListItem> AllCountries { get; set; }
}