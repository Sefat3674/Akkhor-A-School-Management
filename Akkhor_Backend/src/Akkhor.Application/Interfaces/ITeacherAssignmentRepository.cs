using Akkhor.Domain.Entities;

namespace Akkhor.Application.Interfaces.Repositories;

public interface ITeacherAssignmentRepository
{
    // Get all teacher assignments
    Task<IEnumerable<TeacherAssignment>> GetAllAsync();

    // Get assignment by Id
    Task<TeacherAssignment?> GetByIdAsync(Guid id);

    // Check duplicate assignment
    Task<bool> ExistsAsync(
        string teacherId,
        Guid academicYearId,
        Guid classId,
        Guid? sectionId,
        Guid courseId,
        Guid subjectId,
        Guid? excludeId = null);

    // Create
    Task<TeacherAssignment> CreateAsync(
        TeacherAssignment teacherAssignment);

    // Update
    Task<TeacherAssignment> UpdateAsync(
        TeacherAssignment teacherAssignment);

    // Delete
    Task<bool> DeleteAsync(Guid id);
}