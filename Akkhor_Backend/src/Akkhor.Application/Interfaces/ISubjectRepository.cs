using Akkhor.Domain.Entities;

namespace Akkhor.Application.Interfaces.Repositories;

public interface ISubjectRepository
{
    Task<IEnumerable<Subject>> GetAllAsync();

    Task<Subject?> GetByIdAsync(Guid id);

    Task<Subject?> GetByCodeAsync(string code);

    Task AddAsync(Subject entity);

    Task UpdateAsync(Subject entity);

    Task DeleteAsync(Guid id);

    Task<bool> ExistsAsync(Guid id);
}