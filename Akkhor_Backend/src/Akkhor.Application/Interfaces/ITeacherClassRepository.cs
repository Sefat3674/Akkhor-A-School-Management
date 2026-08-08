using Akkhor.Application.DTOs.TeacherClasses;

namespace Akkhor.Application.Interfaces.Repositories;

public interface ITeacherClassRepository
{
    Task<List<TeacherClassDto>> GetMyClassesAsync(
        string teacherId,
        CancellationToken cancellationToken = default);
}