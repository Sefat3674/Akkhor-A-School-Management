using Akkhor.Domain.Entities;

namespace Akkhor.Application.Interfaces.Repositories;

public interface IAssignmentRepository
{
    // =====================================================
    // GET ALL
    // =====================================================

    Task<IEnumerable<Assignment>> GetAllAsync();


    // =====================================================
    // GET BY ID
    // =====================================================

    Task<Assignment?> GetByIdAsync(
        Guid id);


    // =====================================================
    // GET BY TEACHER
    // =====================================================

    Task<IEnumerable<Assignment>> GetByTeacherIdAsync(
        string teacherId);


    // =====================================================
    // GET BY ID FOR TEACHER
    // =====================================================

    Task<Assignment?> GetByIdForTeacherAsync(
        Guid id,
        string teacherId);


    // =====================================================
    // GET BY CLASS
    // =====================================================

    Task<IEnumerable<Assignment>> GetByClassIdAsync(
        Guid classId);


    // =====================================================
    // GET BY COURSE
    // =====================================================

    Task<IEnumerable<Assignment>> GetByCourseIdAsync(
        Guid courseId);


    // =====================================================
    // GET BY SUBJECT
    // =====================================================

    Task<IEnumerable<Assignment>> GetBySubjectIdAsync(
        Guid subjectId);


    // =====================================================
    // CREATE
    // =====================================================

    Task<Assignment> CreateAsync(
        Assignment assignment);


    // =====================================================
    // UPDATE
    // =====================================================

    Task<Assignment> UpdateAsync(
        Assignment assignment);


    // =====================================================
    // DELETE
    // =====================================================

    Task<bool> DeleteAsync(
        Guid id);


    // =====================================================
    // EXISTS
    // =====================================================

    Task<bool> ExistsAsync(
        Guid id);


    // =====================================================
    // DUPLICATE CHECK
    // =====================================================

    Task<bool> ExistsForTeacherAsync(
        string teacherId,
        Guid academicYearId,
        Guid classId,
        Guid? sectionId,
        Guid courseId,
        Guid subjectId,
        string title,
        Guid? excludeId = null);

    // =====================================================
    // GET ASSIGNMENTS FOR STUDENT
    // =====================================================

    // =====================================================
    // GET ASSIGNMENTS FOR STUDENT
    // =====================================================

    Task<IEnumerable<Assignment>>
        GetAssignmentsForStudentAsync(
            string studentId);


    // =====================================================
    // GET ASSIGNMENT FOR STUDENT
    // =====================================================

    Task<Assignment?>
        GetAssignmentForStudentAsync(
            Guid id,
            string studentId);

}