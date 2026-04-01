using CoreFitness2.Domain.Entities.Bookings;

namespace CoreFitness2.Domain.Entities.Classes;

public class GymClassEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public string Category { get; set; } = null!;
    public string Instructor { get; set; } = null!;

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public int MaxParticipants { get; set; }

    public ICollection<BookingEntity> Bookings { get; set; } = [];
}
