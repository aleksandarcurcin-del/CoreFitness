using CoreFitness2.Application.Dtos.Members;
using CoreFitness2.Application.Results;

namespace CoreFitness2.Application.Interfaces;

public interface IMemberService
{
    Task<MemberDto?> GetByApplicationUserIdAsync(string applicationUserId);
    Task<ServiceResult> UpdateAsync(string applicationUserId, UpdateMemberDto dto);
    Task<ServiceResult> DeleteAsync(string applicationUserId);
}