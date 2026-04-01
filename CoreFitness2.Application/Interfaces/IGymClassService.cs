using CoreFitness2.Application.Dtos.Classes;

namespace CoreFitness2.Application.Interfaces;

public interface IGymClassService
{
    Task<IEnumerable<GymClassDto>> GetAllAsync();
    Task<GymClassDto?> GetByIdAsync(int id);
    Task<GymClassDto> CreateAsync(CreateGymClassDto dto);
    Task<bool> UpdateAsync(UpdateGymClassDto dto);
    Task<bool> DeleteAsync(int id);
}
