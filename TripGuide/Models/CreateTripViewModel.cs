using System.ComponentModel.DataAnnotations;
using TripGuide.Attributes;

namespace TripGuide.Models;

public class CreateTripViewModel
{
    [Required]
    [MaxLength(255)]
    public string Name { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Location { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    [EndDateAfterStartDate]
    public DateTime EndDate { get; set; }
}