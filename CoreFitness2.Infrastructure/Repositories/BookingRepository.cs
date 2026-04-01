using CoreFitness2.Application.Interfaces;
using CoreFitness2.Domain.Entities.Bookings;
using CoreFitness2.Infrastructure.Data;

namespace CoreFitness2.Infrastructure.Repositories;

public class BookingRepository(ApplicationDbContext context) : BaseRepository<BookingEntity>(context), IBookingRepository
{
}
