namespace CoreFitness2.Application.Dtos.Members;

public class UpdateMemberDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }

    public string? ProfileImageUrl { get; set; }
}