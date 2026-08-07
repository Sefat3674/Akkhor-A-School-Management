using Akkhor.Domain.Entities;

namespace Akkhor.Application.Interfaces.Repositories;

public interface ISectionRepository
{
    Task<IEnumerable<ClassSection>> GetAllAsync();

    Task<ClassSection?> GetByIdAsync(Guid id);

    Task<ClassSection?> GetByNameAsync(
        Guid classId,
        string sectionName);

    Task AddAsync(ClassSection entity);

    Task UpdateAsync(ClassSection entity);

    Task DeleteAsync(Guid id);

    Task<bool> ExistsAsync(Guid id);
}