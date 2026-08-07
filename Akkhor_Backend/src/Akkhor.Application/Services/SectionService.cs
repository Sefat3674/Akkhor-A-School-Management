using Akkhor.Application.DTOs.Sections;
using Akkhor.Application.Interfaces.Repositories;
using Akkhor.Application.Interfaces.Services;
using Akkhor.Domain.Entities;

namespace Akkhor.Application.Services;

public class SectionService : ISectionService
{
    private readonly ISectionRepository _repository;

    public SectionService(ISectionRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<SectionDto>> GetAllAsync()
    {
        var sections = await _repository.GetAllAsync();

        return sections.Select(x => new SectionDto
        {
            Id = x.Id,

            ClassId = x.ClassId,

            ClassName = x.Class?.Name ?? string.Empty,

            SectionName = x.SectionName,

            RoomNumber = x.RoomNumber,

            Capacity = x.Capacity,

            IsActive = x.IsActive,

            StudentCount = x.StudentEnrollments.Count,

            CreatedAt = x.CreatedAt,

            UpdatedAt = x.UpdatedAt
        });
    }

    public async Task<SectionDto?> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
            return null;

        return new SectionDto
        {
            Id = entity.Id,

            ClassId = entity.ClassId,

            ClassName = entity.Class?.Name ?? string.Empty,

            SectionName = entity.SectionName,

            RoomNumber = entity.RoomNumber,

            Capacity = entity.Capacity,

            IsActive = entity.IsActive,

            StudentCount = entity.StudentEnrollments.Count,

            CreatedAt = entity.CreatedAt,

            UpdatedAt = entity.UpdatedAt
        };
    }

    public async Task<SectionDto> CreateAsync(CreateSectionDto dto)
    {
        var exists = await _repository.GetByNameAsync(
            dto.ClassId,
            dto.SectionName);

        if (exists != null)
            throw new Exception("Section already exists for this class.");

        var entity = new ClassSection
        {
            Id = Guid.NewGuid(),

            ClassId = dto.ClassId,

            SectionName = dto.SectionName,

            RoomNumber = dto.RoomNumber,

            Capacity = dto.Capacity,

            IsActive = true,

            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(entity);

        return await GetByIdAsync(entity.Id)
               ?? throw new Exception("Unable to create section.");
    }

    public async Task<bool> UpdateAsync(
        Guid id,
        UpdateSectionDto dto)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
            return false;

        entity.CreatedAt = DateTime.SpecifyKind(
            entity.CreatedAt,
            DateTimeKind.Utc);

        entity.UpdatedAt = DateTime.UtcNow;

        entity.SectionName = dto.SectionName;
        entity.RoomNumber = dto.RoomNumber;
        entity.Capacity = dto.Capacity;
        entity.IsActive = dto.IsActive;

        await _repository.UpdateAsync(entity);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var exists = await _repository.ExistsAsync(id);

        if (!exists)
            return false;

        await _repository.DeleteAsync(id);

        return true;
    }
}