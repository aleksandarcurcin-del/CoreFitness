using CoreFitness2.Application.Dtos.Bookings;
using CoreFitness2.Application.Interfaces;
using CoreFitness2.Domain.Entities.Bookings;

namespace CoreFitness2.Application.Services;

public class BookingService(IBookingRepository bookingRepository,IGymClassRepository gymClassRepository) : IBookingService
{
    private readonly IBookingRepository _bookingRepository = bookingRepository;
    private readonly IGymClassRepository _gymClassRepository = gymClassRepository;

    public async Task<bool> CreateBookingAsync(CreateBookingDto dto)
    {
        var gymClass = await _gymClassRepository.GetOneAsync(
            predicate: x => x.Id == dto.GymClassId,
            tracking: false,
            default,
            x => x.Bookings
        );

        if (gymClass is null)
            return false;

        var alreadyBooked = await _bookingRepository.ExistsAsync(
            x => x.UserId == dto.UserId && x.GymClassId == dto.GymClassId
        );

        if (alreadyBooked)
            return false;

        if (gymClass.Bookings.Count >= gymClass.MaxParticipants)
            return false;

        var booking = new BookingEntity
        {
            UserId = dto.UserId,
            GymClassId = dto.GymClassId,
            BookedAt = DateTime.UtcNow
        };

        await _bookingRepository.AddAsync(booking);
        await _bookingRepository.SaveChangesAsync();

        return true;
    }

    public async Task<IReadOnlyList<BookingDto>> GetUserBookingsAsync(string userId)
    {
        var bookings = await _bookingRepository.GetAllAsync(
            predicate: x => x.UserId == userId,
            orderBy: query => query.OrderBy(x => x.GymClass.StartTime),
            tracking: false,
            default,
            x => x.GymClass
        );

        return bookings.Select(x => new BookingDto
        {
            Id = x.Id,
            GymClassId = x.GymClassId,
            GymClassName = x.GymClass.Name,
            Category = x.GymClass.Category,
            Instructor = x.GymClass.Instructor,
            StartTime = x.GymClass.StartTime,
            EndTime = x.GymClass.EndTime,
            BookedAt = x.BookedAt
        }).ToList();
    }

    public async Task<bool> CancelBookingAsync(int bookingId, string userId)
    {
        var booking = await _bookingRepository.GetOneAsync(
            predicate: x => x.Id == bookingId && x.UserId == userId,
            tracking: true
        );

        if (booking is null)
            return false;

        _bookingRepository.Delete(booking);
        await _bookingRepository.SaveChangesAsync();

        return true;
    }
}
