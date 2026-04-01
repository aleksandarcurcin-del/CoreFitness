namespace CoreFitness2.Application.Dtos.Bookings;

public class CreateBookingDto
{
    public string UserId { get; set; } = null!;
    public int GymClassId { get; set; }
}
