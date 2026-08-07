using Akkhor.Application.DTOs.CourseSubjects;

namespace Akkhor.Application.Interfaces.Services;

public interface ICourseSubjectService
{
    Task<List<CourseSubjectDto>> GetAllAsync();

    Task<CourseSubjectDto?> GetByIdAsync(Guid id);

    Task<CourseSubjectDto> CreateAsync(CreateCourseSubjectDto dto);

    Task<bool> UpdateAsync(Guid id, UpdateCourseSubjectDto dto);

    Task<bool> DeleteAsync(Guid id);
}