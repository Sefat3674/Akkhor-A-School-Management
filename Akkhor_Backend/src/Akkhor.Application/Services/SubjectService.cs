using Akkhor.Application.DTOs.Subjects;
using Akkhor.Application.Interfaces.Repositories;
using Akkhor.Application.Interfaces.Services;
using Akkhor.Domain.Entities;

namespace Akkhor.Application.Services;

public class SubjectService : ISubjectService
{
    private readonly ISubjectRepository _repository;

    public SubjectService(ISubjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<SubjectDto>> GetAllAsync()
    {
        var subjects = await _repository.GetAllAsync();

        return subjects.Select(x => new SubjectDto
        {
            Id = x.Id,
            Name = x.Name,
            Code = x.Code,
            Description = x.Description,
            CreditHours = x.CreditHours,
            IsActive = x.IsActive,
            CreatedAt = x.CreatedAt
        });
    }

    public async Task<SubjectDto?> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
            return null;

        return new SubjectDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Code = entity.Code,
            Description = entity.Description,
            CreditHours = entity.CreditHours,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt
        };
    }

    public async Task<SubjectDto> CreateAsync(CreateSubjectDto dto)
    {
        var exists = await _repository.GetByCodeAsync(dto.Code);

        if (exists != null)
            throw new Exception("Subject code already exists.");

        var entity = new Subject
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Code = dto.Code,
            Description = dto.Description,
            CreditHours = dto.CreditHours,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(entity);

        return await GetByIdAsync(entity.Id)
            ?? throw new Exception("Unable to create subject.");
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateSubjectDto dto)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
            return false;

        entity.Name = dto.Name;
        entity.Code = dto.Code;
        entity.Description = dto.Description;
        entity.CreditHours = dto.CreditHours;
        entity.IsActive = dto.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;

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