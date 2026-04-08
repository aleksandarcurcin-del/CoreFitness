using CoreFitness2.Application.Dtos.Bookings;
using CoreFitness2.Application.Interfaces;
using CoreFitness2.Application.Results;
using CoreFitness2.Domain.Entities.Bookings;

namespace CoreFitness2.Application.Services;

public class BookingService(IBookingRepository bookingRepository,IGymClassRepository gymClassRepository) : IBookingService
{
    private readonly IBookingRepository _bookingRepository = bookingRepository;
    private readonly IGymClassRepository _gymClassRepository = gymClassRepository;

    public async Task<ServiceResult> CreateBookingAsync(CreateBookingDto dto)
    {
        var gymClass = await _gymClassRepository.GetOneAsync(
            predicate: x => x.Id == dto.GymClassId,
            tracking: false,
            includes: x => x.Bookings
        );

        if (gymClass is null)
            return ServiceResult.Failure("The selected class could not be found.");

        var alreadyBooked = await _bookingRepository.ExistsAsync(
            x => x.UserId == dto.MemberId && x.GymClassId == dto.GymClassId
        );

        if (alreadyBooked)
            return ServiceResult.Failure("You have already booked this class.");

        if (gymClass.Bookings.Count >= gymClass.MaxParticipants)
            return ServiceResult.Failure("This class is already full.");

        var booking = new BookingEntity
        {
            UserId = dto.MemberId,
            GymClassId = dto.GymClassId,
            BookedAt = DateTime.UtcNow
        };

        await _bookingRepository.AddAsync(booking);
        await _bookingRepository.SaveChangesAsync();

        return ServiceResult.Success();
    }

    public async Task<IReadOnlyList<BookingDto>> GetUserBookingsAsync(string userId)
    {
        var bookings = await _bookingRepository.GetAllAsync(
            predicate: x => x.UserId == userId,
            orderBy: query => query.OrderBy(x => x.GymClass.StartTime),
            tracking: false,
            includes: x => x.GymClass
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

    public async Task<ServiceResult> CancelBookingAsync(int bookingId, string userId)
    {
        var booking = await _bookingRepository.GetOneAsync(
            predicate: x => x.Id == bookingId && x.UserId == userId,
            tracking: true
        );

        if (booking is null)
            return ServiceResult.Failure("The booking could not be found.");

        _bookingRepository.Delete(booking);
        await _bookingRepository.SaveChangesAsync();

        return ServiceResult.Success();
    }
}