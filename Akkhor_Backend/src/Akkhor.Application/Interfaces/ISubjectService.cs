using Akkhor.Application.DTOs.Subjects;

namespace Akkhor.Application.Interfaces.Services;

public interface ISubjectService
{
    Task<IEnumerable<SubjectDto>> GetAllAsync();

    Task<SubjectDto?> GetByIdAsync(Guid id);

    Task<SubjectDto> CreateAsync(CreateSubjectDto dto);

    Task<bool> UpdateAsync(
        Guid id,
        UpdateSubjectDto dto);

    Task<bool> DeleteAsync(Guid id);
}