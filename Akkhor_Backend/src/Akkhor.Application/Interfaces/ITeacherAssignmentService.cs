using Akkhor.Application.DTOs.TeacherAssignments;

namespace Akkhor.Application.Interfaces.Services;

public interface ITeacherAssignmentService
{
    // Get all assignments
    Task<IEnumerable<TeacherAssignmentDto>> GetAllAsync();

    // Get assignment by Id
    Task<TeacherAssignmentDto?> GetByIdAsync(Guid id);

    // Create assignment
    Task<TeacherAssignmentDto> CreateAsync(
        CreateTeacherAssignmentDto dto);

    // Update assignment
    Task<TeacherAssignmentDto?> UpdateAsync(
        Guid id,
        UpdateTeacherAssignmentDto dto);

    // Delete assignment
    Task<bool> DeleteAsync(Guid id);
}