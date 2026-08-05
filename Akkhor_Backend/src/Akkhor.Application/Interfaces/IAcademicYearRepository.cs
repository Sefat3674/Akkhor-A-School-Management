using Akkhor.Domain.Entities;

namespace Akkhor.Application.Interfaces.Repositories;

public interface IAcademicYearRepository
{
    Task<IEnumerable<AcademicYear>> GetAllAsync();

    Task<AcademicYear?> GetByIdAsync(Guid id);

    Task<bool> ExistsAsync(string name);

    Task AddAsync(AcademicYear academicYear);

    Task UpdateAsync(AcademicYear academicYear);

    Task DeleteAsync(AcademicYear academicYear);

    Task SaveChangesAsync();
}