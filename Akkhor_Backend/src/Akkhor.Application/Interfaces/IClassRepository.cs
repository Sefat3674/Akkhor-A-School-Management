using Akkhor.Domain.Entities;

namespace Akkhor.Application.Interfaces.Repositories;

public interface IClassRepository
{
    Task<IEnumerable<Class>> GetAllAsync();

    Task<Class?> GetByIdAsync(Guid id);

    Task<Class?> GetByCodeAsync(string code);

    Task AddAsync(Class entity);

    Task UpdateAsync(Class entity);

    Task DeleteAsync(Guid id);

    Task<bool> ExistsAsync(Guid id);
}