namespace CoreFitness2.Application.Dtos.Bookings;

public class BookingDto
{
    public int Id { get; set; }
    public int GymClassId { get; set; }
    public string GymClassName { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string Instructor { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime BookedAt { get; set; }
}
