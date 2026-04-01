using CoreFitness2.Application.Dtos.Classes;

namespace CoreFitness2.Presentation.ViewModels.Classes;

public class GymClassIndexViewModel
{
    public IEnumerable<GymClassDto> Classes { get; set; } = Enumerable.Empty<GymClassDto>();
}
