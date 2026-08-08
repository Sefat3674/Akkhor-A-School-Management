using Akkhor.Application.DTOs.Assignments;

namespace Akkhor.Application.Interfaces.Services;

public interface IAssignmentService
{
    // =====================================================
    // GET ALL
    // =====================================================

    Task<IEnumerable<AssignmentDto>> GetAllAsync();


    // =====================================================
    // GET BY ID
    // =====================================================

    Task<AssignmentDto?> GetByIdAsync(Guid id);


    // =====================================================
    // GET MY ASSIGNMENTS
    // =====================================================

    Task<IEnumerable<AssignmentDto>> GetMyAssignmentsAsync(
        string teacherId);


    // =====================================================
    // GET MY ASSIGNMENT BY ID
    // =====================================================

    Task<AssignmentDto?> GetMyAssignmentByIdAsync(
        Guid id,
        string teacherId);


    // =====================================================
    // GET BY CLASS
    // =====================================================

    Task<IEnumerable<AssignmentDto>> GetByClassAsync(
        Guid classId);


    // =====================================================
    // GET BY COURSE
    // =====================================================

    Task<IEnumerable<AssignmentDto>> GetByCourseAsync(
        Guid courseId);


    // =====================================================
    // GET BY SUBJECT
    // =====================================================

    Task<IEnumerable<AssignmentDto>> GetBySubjectAsync(
        Guid subjectId);


    // =====================================================
    // GET BY TEACHER
    // =====================================================

    Task<IEnumerable<AssignmentDto>> GetByTeacherAsync(
        string teacherId);


    // =====================================================
    // CREATE
    // =====================================================

    Task<AssignmentDto> CreateAsync(
        CreateAssignmentDto dto,
        string teacherId);


    // =====================================================
    // UPDATE
    // =====================================================

    Task<AssignmentDto?> UpdateAsync(
        Guid id,
        UpdateAssignmentDto dto,
        string teacherId);


    // =====================================================
    // DELETE
    // =====================================================

    Task<bool> DeleteAsync(
        Guid id,
        string teacherId);


    // =====================================================
    // PUBLISH
    // =====================================================

    Task<AssignmentDto?> PublishAsync(
        Guid id,
        string teacherId);


    // =====================================================
    // UNPUBLISH / DRAFT
    // =====================================================

    Task<AssignmentDto?> UnpublishAsync(
        Guid id,
        string teacherId);

    // =====================================================
    // STUDENT ASSIGNMENTS
    // =====================================================

    Task<IEnumerable<AssignmentDto>>
        GetAssignmentsForStudentAsync(
            string studentId);

    Task<AssignmentDto?>
        GetAssignmentForStudentAsync(
            Guid id,
            string studentId);

}

