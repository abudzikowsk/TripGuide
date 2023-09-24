using System.ComponentModel.DataAnnotations;
using TripGuide.Attributes;

namespace TripGuide.Models;

public class EditTripViewModel
{
    [Required]
    [MaxLength(255)]
    public string Name { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string City { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Country { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    [EndDateAfterStartDate]
    public DateTime EndDate { get; set; }
}