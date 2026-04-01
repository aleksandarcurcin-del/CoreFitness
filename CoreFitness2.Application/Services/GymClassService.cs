using CoreFitness2.Application.Dtos.Classes;
using CoreFitness2.Application.Interfaces;
using CoreFitness2.Domain.Entities.Classes;

namespace CoreFitness2.Application.Services;

public class GymClassService(IGymClassRepository gymClassRepository) : IGymClassService
{
    private readonly IGymClassRepository _gymClassRepository = gymClassRepository;

    public async Task<IEnumerable<GymClassDto>> GetAllAsync()
    {
        var entities = await _gymClassRepository.GetAllAsync(
            predicate: null,
            orderBy: query => query.OrderBy(x => x.StartTime),
            tracking: false
            
        );

        return entities.Select(MapToDto);
    }

    public async Task<GymClassDto?> GetByIdAsync(int id)
    {
        var entity = await _gymClassRepository.GetOneAsync(
            predicate: x => x.Id == id,
            tracking: false
        );

        return entity is null ? null : MapToDto(entity);
    }

    public async Task<GymClassDto> CreateAsync(CreateGymClassDto dto)
    {
        var entity = new GymClassEntity
        {
            Name = dto.Name,
            Description = dto.Description,
            Category = dto.Category,
            Instructor = dto.Instructor,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            MaxParticipants = dto.MaxParticipants
        };

        await _gymClassRepository.AddAsync(entity);
        await _gymClassRepository.SaveChangesAsync();

        return MapToDto(entity);
    }

    public async Task<bool> UpdateAsync(UpdateGymClassDto dto)
    {
        var entity = await _gymClassRepository.GetOneAsync(
            predicate: x => x.Id == dto.Id,
            tracking: true
        );

        if (entity is null)
            return false;

        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.Category = dto.Category;
        entity.Instructor = dto.Instructor;
        entity.StartTime = dto.StartTime;
        entity.EndTime = dto.EndTime;
        entity.MaxParticipants = dto.MaxParticipants;

        await _gymClassRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _gymClassRepository.GetOneAsync(
            predicate: x => x.Id == id,
            tracking: true
        );

        if (entity is null)
            return false;

        _gymClassRepository.Delete(entity);
        await _gymClassRepository.SaveChangesAsync();

        return true;
    }

    private static GymClassDto MapToDto(GymClassEntity entity)
    {
        return new GymClassDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Category = entity.Category,
            Instructor = entity.Instructor,
            StartTime = entity.StartTime,
            EndTime = entity.EndTime,
            MaxParticipants = entity.MaxParticipants
        };
    }
}
