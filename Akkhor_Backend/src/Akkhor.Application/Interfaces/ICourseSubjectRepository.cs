using Akkhor.Domain.Entities;

namespace Akkhor.Application.Interfaces.Repositories;

public interface ICourseSubjectRepository
{
    Task<List<CourseSubject>> GetAllAsync();

    Task<CourseSubject?> GetByIdAsync(Guid id);

    Task AddAsync(CourseSubject entity);

    Task UpdateAsync(CourseSubject entity);

    Task DeleteAsync(Guid id);

    Task<bool> ExistsAsync(Guid courseId, Guid subjectId);
}