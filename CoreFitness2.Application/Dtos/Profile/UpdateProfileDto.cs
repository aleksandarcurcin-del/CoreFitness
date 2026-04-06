namespace CoreFitness2.Application.Dtos.Profile;

public class UpdateProfileDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
}
