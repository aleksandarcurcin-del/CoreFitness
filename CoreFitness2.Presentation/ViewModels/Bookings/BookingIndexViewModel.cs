using CoreFitness2.Application.Dtos.Bookings;

namespace CoreFitness2.Presentation.ViewModels.Bookings;

public class BookingIndexViewModel
{
    public IEnumerable<BookingDto> Bookings { get; set; } = Enumerable.Empty<BookingDto>();
}