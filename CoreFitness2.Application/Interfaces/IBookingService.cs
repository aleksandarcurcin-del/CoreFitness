using CoreFitness2.Application.Dtos.Bookings;
using CoreFitness2.Application.Results;

namespace CoreFitness2.Application.Interfaces;

public interface IBookingService
{
    Task<ServiceResult> CreateBookingAsync(CreateBookingDto dto);
    Task<IReadOnlyList<BookingDto>> GetMemberBookingsAsync(int memberId);
    Task<ServiceResult> CancelBookingAsync(int bookingId, int memberId);
}
