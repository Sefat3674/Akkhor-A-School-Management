using Akkhor.Application.DTOs.Classes;

namespace Akkhor.Application.Interfaces.Services;

public interface IClassService
{
    Task<IEnumerable<ClassDto>> GetAllAsync();

    Task<ClassDto?> GetByIdAsync(Guid id);

    Task<ClassDto> CreateAsync(CreateClassDto dto);

    Task<bool> UpdateAsync(Guid id, UpdateClassDto dto);

    Task<bool> DeleteAsync(Guid id);
}