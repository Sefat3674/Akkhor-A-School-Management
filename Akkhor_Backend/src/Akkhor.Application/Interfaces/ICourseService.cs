using Akkhor.Application.DTOs.Courses;

namespace Akkhor.Application.Interfaces.Services;

public interface ICourseService
{
    Task<IEnumerable<CourseDto>> GetAllAsync();


    Task<CourseDto?> GetByIdAsync(Guid id);


    Task<CourseDto> CreateAsync(CreateCourseDto dto);


    Task<bool> UpdateAsync(
        Guid id,
        UpdateCourseDto dto);


    Task<bool> DeleteAsync(Guid id);
}