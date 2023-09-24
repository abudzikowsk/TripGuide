using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

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
    
    [Required]
    public string Longitude { get; set; }
    
    [Required(ErrorMessage = "The point on map is required.")]
    public string Latitude { get; set; }
    
    public string? City { get; set; }
    
    public string? Country { get; set; }
}