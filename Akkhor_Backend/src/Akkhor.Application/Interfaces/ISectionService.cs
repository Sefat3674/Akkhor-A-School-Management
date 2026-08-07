using Akkhor.Application.DTOs.Sections;

namespace Akkhor.Application.Interfaces.Services;

public interface ISectionService
{
    Task<IEnumerable<SectionDto>> GetAllAsync();

    Task<SectionDto?> GetByIdAsync(Guid id);

    Task<SectionDto> CreateAsync(CreateSectionDto dto);

    Task<bool> UpdateAsync(
        Guid id,
        UpdateSectionDto dto);

    Task<bool> DeleteAsync(Guid id);
}