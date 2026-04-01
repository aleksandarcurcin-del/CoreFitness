using CoreFitness2.Application.Dtos.Bookings;

namespace CoreFitness2.Application.Interfaces;

public interface IBookingService
{
    Task<bool> CreateBookingAsync(CreateBookingDto dto);
    Task<IReadOnlyList<BookingDto>> GetUserBookingsAsync(string userId);
    Task<bool> CancelBookingAsync(int bookingId, string userId);
}
