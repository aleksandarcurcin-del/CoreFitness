using CoreFitness2.Domain.Entities.Classes;

namespace CoreFitness2.Domain.Entities.Bookings;

public class BookingEntity
{
    public int Id { get; set; }

    public int MemberId { get; set; }

    public int GymClassId { get; set; }
    public GymClassEntity GymClass { get; set; } = null!;

    public DateTime BookedAt { get; set; } = DateTime.UtcNow;
}
