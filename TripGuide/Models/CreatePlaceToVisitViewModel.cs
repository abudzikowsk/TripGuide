using System.ComponentModel.DataAnnotations;

namespace TripGuide.Models;

public class CreatePlaceToVisitViewModel
{
    [Required]
    [MaxLength(255)]
    public string Name { get; set; }
    
    [Required]
    [Range(1,999999)]
    public int VisitOrder { get; set; }
    
    [MaxLength(1000)]
    public string Note { get; set; }
    
    public int Longitude { get; set; }
    public int Latitude { get; set; }
}