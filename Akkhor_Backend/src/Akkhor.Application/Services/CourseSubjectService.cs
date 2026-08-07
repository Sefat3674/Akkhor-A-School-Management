using Akkhor.Application.DTOs.CourseSubjects;
using Akkhor.Application.Interfaces.Repositories;
using Akkhor.Application.Interfaces.Services;
using Akkhor.Domain.Entities;

namespace Akkhor.Application.Services;

public class CourseSubjectService : ICourseSubjectService
{
    private readonly ICourseSubjectRepository _repository;

    public CourseSubjectService(ICourseSubjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CourseSubjectDto>> GetAllAsync()
    {
        var data = await _repository.GetAllAsync();

        return data.Select(x => new CourseSubjectDto
        {
            Id = x.Id,
            CourseId = x.CourseId,
            CourseName = x.Course.CourseName,
            SubjectId = x.SubjectId,
            SubjectName = x.Subject.Name,
            IsMandatory = x.IsMandatory,
            DisplayOrder = x.DisplayOrder
        }).ToList();
    }

    public async Task<CourseSubjectDto?> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
            return null;

        return new CourseSubjectDto
        {
            Id = entity.Id,
            CourseId = entity.CourseId,
            CourseName = entity.Course.CourseName,
            SubjectId = entity.SubjectId,
            SubjectName = entity.Subject.Name,
            IsMandatory = entity.IsMandatory,
            DisplayOrder = entity.DisplayOrder
        };
    }

    public async Task<CourseSubjectDto> CreateAsync(CreateCourseSubjectDto dto)
    {
        var exists = await _repository.ExistsAsync(dto.CourseId, dto.SubjectId);

        if (exists)
            throw new Exception("This subject is already assigned to this course.");

        var entity = new CourseSubject
        {
            Id = Guid.NewGuid(),
            CourseId = dto.CourseId,
            SubjectId = dto.SubjectId,
            IsMandatory = dto.IsMandatory,
            DisplayOrder = dto.DisplayOrder,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(entity);

        var created = await _repository.GetByIdAsync(entity.Id);

        return new CourseSubjectDto
        {
            Id = created!.Id,
            CourseId = created.CourseId,
            CourseName = created.Course.CourseName,
            SubjectId = created.SubjectId,
            SubjectName = created.Subject.Name,
            IsMandatory = created.IsMandatory,
            DisplayOrder = created.DisplayOrder
        };
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateCourseSubjectDto dto)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
            return false;


        entity.CourseId = dto.CourseId;

        entity.SubjectId = dto.SubjectId;

        entity.IsMandatory = dto.IsMandatory;

        entity.DisplayOrder = dto.DisplayOrder;

        


        // Fix PostgreSQL DateTime issue
        entity.CreatedAt = DateTime.SpecifyKind(
            entity.CreatedAt,
            DateTimeKind.Utc
        );

        entity.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(entity);


        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
            return false;

        await _repository.DeleteAsync(id);

        return true;
    }
}