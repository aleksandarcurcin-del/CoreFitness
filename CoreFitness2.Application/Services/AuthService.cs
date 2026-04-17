using CoreFitness2.Application.Dtos.Auth;
using CoreFitness2.Application.Interfaces;
using CoreFitness2.Application.Results;
using CoreFitness2.Domain.Entities.Members;
using Microsoft.Extensions.Logging;

namespace CoreFitness2.Application.Services;

public class AuthService : IAuthService
{
    private readonly IAuthGateway _authGateway;
    private readonly IMemberRepository _memberRepository;
    private readonly ILogger<AuthService> _logger;


    public AuthService(IAuthGateway authGateway, IMemberRepository memberRepository, ILogger<AuthService> logger)
    {
        _authGateway = authGateway;
        _memberRepository = memberRepository;
        _logger = logger;
    }

    public async Task<IReadOnlyList<string>> GetExternalProvidersAsync()
    {
        return await _authGateway.GetExternalProvidersAsync();
    }

    public async Task<AuthenticationResult> HandleExternalLoginCallbackAsync(string? returnUrl = null, string? remoteError = null)
    {
        if (!string.IsNullOrWhiteSpace(remoteError))
        {
            _logger.LogWarning("External login failed: {Error}", remoteError);
            return AuthenticationResult.Failed(returnUrl);
        }

        var externalUserInfo = await _authGateway.GetExternalUserInfoAsync();

        if (externalUserInfo is null)
        {
            _logger.LogWarning("External login info could not be retrieved.");
            return AuthenticationResult.Failed(returnUrl);
        }

        return await _authGateway.ExternalLoginSignInAsync(externalUserInfo, returnUrl);
    }

    public async Task<AuthenticationResult> VerifyExternalLoginAsync(string code, string? returnUrl = null)
    {
        if (!string.Equals(code, "123456", StringComparison.Ordinal))
        {
            _logger.LogWarning("Invalid external verification code.");
            return AuthenticationResult.InvalidCode(returnUrl);
        }

        var externalUserInfo = await _authGateway.GetExternalUserInfoAsync();
        if (externalUserInfo is null)
        {
            _logger.LogWarning("Could not load external login information during verification.");
            return AuthenticationResult.Failed(returnUrl);
        }

        var existingMember = await _memberRepository.GetOneAsync(
            x => x.Email == externalUserInfo.Email,
            tracking: false
        );

        if (existingMember is not null)
        {
            _logger.LogInformation(
                "Existing member found for external login verification: {Email}",
                externalUserInfo.Email);

            var linkResult = await _authGateway.LinkExternalLoginAsync(
                externalUserInfo.Email,
                externalUserInfo);

            if (!linkResult.Succeeded)
            {
                _logger.LogError(
                    "Failed to link external login for existing member {Email}. Error: {Error}",
                    externalUserInfo.Email,
                    linkResult.ErrorMessage);

                return AuthenticationResult.Failed(returnUrl);
            }

            _logger.LogInformation(
                "External login linked successfully for existing member {Email}.",
                externalUserInfo.Email);

            return AuthenticationResult.SignedIn(returnUrl);
        }

        _logger.LogInformation(
            "No existing member found. Creating external user and member for {Email}.",
            externalUserInfo.Email);

        var (createResult, applicationUserId) = await _authGateway.CreateExternalIdentityUserAsync(externalUserInfo);

        if (!createResult.Succeeded)
        {
            _logger.LogError(
                "Failed to create external identity user for {Email}. Error: {Error}",
                externalUserInfo.Email,
                createResult.ErrorMessage);

            return AuthenticationResult.Failed(returnUrl);
        }

        if (string.IsNullOrWhiteSpace(applicationUserId))
        {
            _logger.LogError(
                "External identity user creation returned empty ApplicationUserId for {Email}.",
                externalUserInfo.Email);

            return AuthenticationResult.Failed(returnUrl);
        }

        var member = new MemberEntity
        {
            ApplicationUserId = applicationUserId,
            Email = externalUserInfo.Email,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _memberRepository.AddAsync(member);
        await _memberRepository.SaveChangesAsync();

        _logger.LogInformation(
            "MemberEntity created successfully for external user {Email} with ApplicationUserId {ApplicationUserId}.",
            externalUserInfo.Email,
            applicationUserId);

        return AuthenticationResult.SignedIn(returnUrl);
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