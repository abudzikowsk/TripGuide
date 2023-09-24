using System.ComponentModel.DataAnnotations;

namespace TripGuide.Models;

public class EditPlaceToVisitViewModel
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

    [Required]
    [Range(0.00,999999)]
    public decimal Cost { get; set; }
    
    public string? City { get; set; }
    
    public string? Country { get; set; }
}