using CoreFitness2.Domain.Entities.Bookings;
using CoreFitness2.Domain.Entities.MembershipPlans;

namespace CoreFitness2.Domain.Entities.Members;

public class MemberEntity
{
    public int Id { get; set; }
    public string ApplicationUserId { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? ProfileImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public MembershipEntity? Membership { get; set; }
    public ICollection<BookingEntity> Bookings { get; set; } = [];

}
