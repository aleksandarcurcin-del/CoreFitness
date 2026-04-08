using CoreFitness2.Application.Dtos.Auth;
using CoreFitness2.Application.Interfaces;
using CoreFitness2.Application.Results;
using CoreFitness2.Domain.Entities.Members;

namespace CoreFitness2.Application.Services;

public class AuthService : IAuthService
{
    private readonly IAuthGateway _authGateway;
    private readonly IMemberRepository _memberRepository;

    public AuthService(IAuthGateway authGateway, IMemberRepository memberRepository)
    {
        _authGateway = authGateway;
        _memberRepository = memberRepository;
    }

    public async Task<ServiceResult> RegisterAsync(SignUpDto dto)
    {
        var (result, applicationUserId) = await _authGateway.RegisterIdentityUserAsync(dto);

        if (!result.Succeeded)
            return result;

        if (string.IsNullOrWhiteSpace(applicationUserId))
            return ServiceResult.Failure("Could not create user account.");

        var member = new MemberEntity
        {
            ApplicationUserId = applicationUserId,
            Email = dto.Email,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _memberRepository.AddAsync(member);
        await _memberRepository.SaveChangesAsync();

        return ServiceResult.Success();
    }

    public async Task<ServiceResult> SignInAsync(SignInDto dto)
    {
        return await _authGateway.SignInAsync(dto);
    }

    public async Task SignOutAsync()
    {
        await _authGateway.SignOutAsync();
    }
}