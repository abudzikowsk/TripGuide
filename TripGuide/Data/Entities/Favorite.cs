using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using TripGuide.Models;

namespace TripGuide.Data.Entities;

public class Favorite
{
    public int Id { get; set; }
    public int TripId { get; set; }
    public string UserId { get; set; }
        
    public Trip Trip { get; set; }
    public IdentityUser User { get; set; }
    
    public FavoriteViewModel MapToViewModel()
    {
        return new FavoriteViewModel
        {
            Id = Id,
            City = Trip.City,
            Name = Trip.Name,
            TripId = TripId
        };
    }
}