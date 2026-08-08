using Akkhor.Domain.Entities;

namespace Akkhor.Application.Interfaces.Repositories;

public interface IAssignmentSubmissionRepository
{
    // =====================================================
    // GET ALL SUBMISSIONS
    // =====================================================

    Task<IEnumerable<AssignmentSubmission>> GetAllAsync();


    // =====================================================
    // GET SUBMISSION BY ID
    // =====================================================

    Task<AssignmentSubmission?> GetByIdAsync(
        Guid id);


    // =====================================================
    // GET SUBMISSIONS BY ASSIGNMENT
    // =====================================================

    Task<IEnumerable<AssignmentSubmission>> GetByAssignmentIdAsync(
        Guid assignmentId);


    // =====================================================
    // GET SUBMISSION BY ASSIGNMENT + STUDENT
    // =====================================================

    Task<AssignmentSubmission?> GetByAssignmentAndStudentAsync(
        Guid assignmentId,
        string studentId);


    // =====================================================
    // GET SUBMISSIONS BY STUDENT
    // =====================================================

    Task<IEnumerable<AssignmentSubmission>> GetByStudentIdAsync(
        string studentId);


    // =====================================================
    // CREATE
    // =====================================================

    Task<AssignmentSubmission> CreateAsync(
        AssignmentSubmission submission);


    // =====================================================
    // UPDATE
    // =====================================================

    Task<AssignmentSubmission> UpdateAsync(
        AssignmentSubmission submission);


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
    // CHECK STUDENT SUBMISSION
    // =====================================================

    Task<bool> ExistsForStudentAsync(
        Guid assignmentId,
        string studentId);


    // =====================================================
    // COUNT SUBMISSIONS
    // =====================================================

    Task<int> GetSubmissionCountAsync(
        Guid assignmentId);


    // =====================================================
    // COUNT PENDING SUBMISSIONS
    // =====================================================

    Task<int> GetPendingSubmissionCountAsync(
        Guid assignmentId);
}