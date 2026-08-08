using Akkhor.Application.DTOs.TeacherClasses;
using Akkhor.Application.Interfaces.Repositories;
using Akkhor.Application.Interfaces.Services;

namespace Akkhor.Application.Services;

public class TeacherClassService : ITeacherClassService
{
    private readonly ITeacherClassRepository _repository;

    public TeacherClassService(
        ITeacherClassRepository repository)
    {
        _repository = repository;
    }

    // =====================================================
    // GET MY CLASSES
    // =====================================================

    public async Task<List<TeacherClassDto>> GetMyClassesAsync(
        string teacherId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(teacherId))
        {
            throw new ArgumentException(
                "Teacher ID is required.",
                nameof(teacherId));
        }

        return await _repository.GetMyClassesAsync(
            teacherId,
            cancellationToken);
    }
}