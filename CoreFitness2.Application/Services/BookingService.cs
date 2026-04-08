using CoreFitness2.Application.Dtos.Bookings;
using CoreFitness2.Application.Interfaces;
using CoreFitness2.Application.Results;
using CoreFitness2.Domain.Entities.Bookings;

namespace CoreFitness2.Application.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IGymClassRepository _gymClassRepository;

    public BookingService(IBookingRepository bookingRepository, IGymClassRepository gymClassRepository)
    {
        _bookingRepository = bookingRepository;
        _gymClassRepository = gymClassRepository;
    }

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
            x => x.MemberId == dto.MemberId && x.GymClassId == dto.GymClassId
        );

        if (alreadyBooked)
            return ServiceResult.Failure("You have already booked this class.");

        if (gymClass.Bookings.Count >= gymClass.MaxParticipants)
            return ServiceResult.Failure("This class is already full.");

        var booking = new BookingEntity
        {
            MemberId = dto.MemberId,
            GymClassId = dto.GymClassId,
            BookedAt = DateTime.UtcNow
        };

        await _bookingRepository.AddAsync(booking);
        await _bookingRepository.SaveChangesAsync();

        return ServiceResult.Success();
    }

    public async Task<IReadOnlyList<BookingDto>> GetMemberBookingsAsync(int memberId)
    {
        var bookings = await _bookingRepository.GetAllAsync(
            predicate: x => x.MemberId == memberId,
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

    public async Task<ServiceResult> CancelBookingAsync(int bookingId, int memberId)
    {
        var booking = await _bookingRepository.GetOneAsync(
            predicate: x => x.Id == bookingId && x.MemberId == memberId,
            tracking: true
        );

        if (booking is null)
            return ServiceResult.Failure("The booking could not be found.");

        _bookingRepository.Delete(booking);
        await _bookingRepository.SaveChangesAsync();

        return ServiceResult.Success();
    }
}