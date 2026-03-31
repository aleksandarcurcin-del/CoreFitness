using CoreFitness2.Application.Dtos.Memberships;
using CoreFitness2.Application.Interfaces;
using CoreFitness2.Domain.Entities.MembershipPlans;

namespace CoreFitness2.Application.Services;

public class MembershipService : IMembershipService
{
    private readonly IMembershipPlanRepository _membershipPlanRepository;
    private readonly IMembershipRepository _membershipRepository;

    public MembershipService(
        IMembershipPlanRepository membershipPlanRepository,
        IMembershipRepository membershipRepository)
    {
        _membershipPlanRepository = membershipPlanRepository;
        _membershipRepository = membershipRepository;
    }

    public async Task<List<MembershipPlanDto>> GetAllPlansAsync()
    {
        var plans = await _membershipPlanRepository.GetAllAsync(
            orderBy: q => q.OrderBy(x => x.SortOrder),
            includes: x => x.Features
        );

        return plans
            .Select(plan => new MembershipPlanDto
            {
                Id = plan.Id,
                MembershipPlanType = plan.MembershipPlanType.ToString(),
                Description = plan.Description,
                Price = plan.Price,
                MonthlyClassLimit = plan.MonthlyClassLimit,
                FreeTrialWeeks = plan.FreeTrialWeeks,
                Features = plan.Features
                    .OrderBy(feature => feature.SortOrder)
                    .Select(feature => new MembershipPlanFeatureDto
                    {
                        Description = feature.Description,
                        SortOrder = feature.SortOrder
                    })
                    .ToList()
            })
            .ToList();
    }

    public async Task<UserMembershipDto?> GetUserMembershipAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return null;

        var membership = await _membershipRepository.GetOneAsync(
            x => x.UserId == userId,
            includes: x => x.MembershipPlan
        );

        if (membership is null)
            return null;

        return new UserMembershipDto
        {
            Id = membership.Guid,
            UserId = membership.UserId,
            MembershipPlanId = membership.MembershipPlanId,
            MembershipPlanType = membership.MembershipPlan.MembershipPlanType.ToString(),
            Status = membership.Status,
            StartDate = membership.StartDate
        };
    }

    public async Task<bool> CreateMembershipAsync(CreateMembershipDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.UserId))
            return false;

        var selectedPlan = await _membershipPlanRepository.GetOneAsync(
            x => x.Id == dto.MembershipPlanId,
            includes: x => x.Features
        );

        if (selectedPlan is null)
            return false;

        var existingMembership = await _membershipRepository.GetOneAsync(
            x => x.UserId == dto.UserId,
            tracking: true
        );

        if (existingMembership is null)
        {
            var membership = new MembershipEntity
            {
                Guid = Guid.NewGuid(),
                UserId = dto.UserId,
                MembershipPlanId = dto.MembershipPlanId,
                StartDate = DateTime.UtcNow,
                Status = "Active"
            };

            await _membershipRepository.AddAsync(membership);
            await _membershipRepository.SaveChangesAsync();
            return true;
        }

        if (existingMembership.Status == "Cancelled")
        {
            existingMembership.MembershipPlanId = dto.MembershipPlanId;
            existingMembership.StartDate = DateTime.UtcNow;
            existingMembership.Status = "Active";

            await _membershipRepository.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<bool> ChangeMembershipPlanAsync(string userId, Guid newPlanId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return false;

        var membership = await _membershipRepository.GetOneAsync(
            x => x.UserId == userId,
            tracking: true,
            includes: x => x.MembershipPlan
        );

        if (membership is null)
            return false;

        if (membership.Status == "Cancelled")
            return false;

        var selectedPlan = await _membershipPlanRepository.GetOneAsync(
            x => x.Id == newPlanId
        );

        if (selectedPlan is null)
            return false;

        if (membership.MembershipPlanId == newPlanId)
            return false;

        membership.MembershipPlanId = newPlanId;
        membership.Status = "Active";

        await _membershipRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CancelMembershipAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return false;

        var membership = await _membershipRepository.GetOneAsync(
            x => x.UserId == userId,
            tracking: true
        );

        if (membership is null)
            return false;

        membership.Status = "Cancelled";

        await _membershipRepository.SaveChangesAsync();

        return true;
    }
}