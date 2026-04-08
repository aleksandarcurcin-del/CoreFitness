using CoreFitness2.Application.Dtos.Members;
using CoreFitness2.Application.Interfaces;
using CoreFitness2.Application.Results;
using CoreFitness2.Domain.Entities.Members;

namespace CoreFitness2.Application.Services;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;
    private readonly IUserAccountGateway _userAccountGateway;

    public MemberService(IMemberRepository memberRepository, IUserAccountGateway userAccountGateway)
    {
        _memberRepository = memberRepository;
        _userAccountGateway = userAccountGateway;
    }

    public async Task<MemberDto?> GetByApplicationUserIdAsync(string applicationUserId)
    {
        if (string.IsNullOrWhiteSpace(applicationUserId))
            return null;

        var member = await _memberRepository.GetOneAsync(
            predicate: x => x.ApplicationUserId == applicationUserId,
            tracking: false
        );

        if (member == null)
            return null;

        return new MemberDto
        {
            Id = member.Id,
            ApplicationUserId = member.ApplicationUserId,
            FirstName = member.FirstName,
            LastName = member.LastName,
            Email = member.Email,
            PhoneNumber = member.PhoneNumber,
            ProfileImageUrl = member.ProfileImageUrl
        };
    }

    public async Task<ServiceResult> UpdateAsync(string applicationUserId, UpdateMemberDto dto)
    {
        if (string.IsNullOrWhiteSpace(applicationUserId))
            return ServiceResult.Failure("User was not found.");

        var member = await _memberRepository.GetOneAsync(
            predicate: x => x.ApplicationUserId == applicationUserId,
            tracking: true
        );

        if (member == null)
            return ServiceResult.Failure("Member profile was not found.");

        member.FirstName = dto.FirstName;
        member.LastName = dto.LastName;
        member.Email = dto.Email;
        member.PhoneNumber = dto.PhoneNumber;

        await _memberRepository.SaveChangesAsync();

        return ServiceResult.Success();
    }

    public async Task<ServiceResult> DeleteAsync(string applicationUserId)
    {
        if (string.IsNullOrWhiteSpace(applicationUserId))
            return ServiceResult.Failure("User was not found.");

        var member = await _memberRepository.GetOneAsync(
            predicate: x => x.ApplicationUserId == applicationUserId,
            tracking: true
        );

        if (member == null)
            return ServiceResult.Failure("Member profile was not found.");

        _memberRepository.Delete(member);
        await _memberRepository.SaveChangesAsync();

        var identityDeleteResult = await _userAccountGateway.DeleteIdentityUserAsync(applicationUserId);

        if (!identityDeleteResult.Succeeded)
            return ServiceResult.Failure(identityDeleteResult.ErrorMessage ?? "Could not delete account.");

        return ServiceResult.Success();
    }
}