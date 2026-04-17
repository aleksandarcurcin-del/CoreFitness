namespace CoreFitness2.Application.Models;

public record ExternalUserInfo(string LoginProvider, string ProviderKey, string Email)
{
}
