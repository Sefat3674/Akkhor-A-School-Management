using Akkhor.Application.DTOs.TeacherClasses;

namespace Akkhor.Application.Interfaces.Services;

public interface ITeacherClassService
{
    Task<List<TeacherClassDto>> GetMyClassesAsync(
        string teacherId,
        CancellationToken cancellationToken = default);
}