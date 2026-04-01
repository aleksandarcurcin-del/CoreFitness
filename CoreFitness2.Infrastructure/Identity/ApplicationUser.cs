using CoreFitness2.Domain.Entities.Bookings;
using CoreFitness2.Domain.Entities.MembershipPlans;
using Microsoft.AspNetCore.Identity;

namespace CoreFitness2.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    
    public string? FirstName { get; set; }

    
    public string? LastName { get; set; }

    public MembershipEntity? Membership { get; set; }

    public ICollection<BookingEntity> Bookings { get; set; } = [];
}
