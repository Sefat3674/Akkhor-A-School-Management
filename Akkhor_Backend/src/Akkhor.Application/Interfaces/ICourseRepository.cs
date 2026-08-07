using Akkhor.Domain.Entities;

namespace Akkhor.Application.Interfaces.Repositories;

public interface ICourseRepository
{
    Task<IEnumerable<Course>> GetAllAsync();


    Task<Course?> GetByIdAsync(Guid id);


    Task<Course?> GetByCodeAsync(string code);


    Task AddAsync(Course entity);


    Task UpdateAsync(Course entity);


    Task DeleteAsync(Guid id);


    Task<bool> ExistsAsync(Guid id);
}