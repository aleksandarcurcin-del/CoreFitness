using System.ComponentModel.DataAnnotations;

namespace CoreFitness2.Presentation.ViewModels.Classes;

public class CreateGymClassViewModel
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;

    [Required]
    public string Category { get; set; } = null!;

    [Required]
    public string Instructor { get; set; } = null!;

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    [Range(1, 500)]
    public int MaxParticipants { get; set; }
}
