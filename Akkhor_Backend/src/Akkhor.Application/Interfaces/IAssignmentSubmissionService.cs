using Akkhor.Application.DTOs.Assignments;

namespace Akkhor.Application.Interfaces.Services;

public interface IAssignmentSubmissionService
{
    // =====================================================
    // GET ALL SUBMISSIONS
    // =====================================================

    Task<IEnumerable<AssignmentSubmissionDto>> GetAllAsync();


    // =====================================================
    // GET SUBMISSION BY ID
    // =====================================================

    Task<AssignmentSubmissionDto?> GetByIdAsync(
        Guid id);


    // =====================================================
    // GET SUBMISSIONS BY ASSIGNMENT
    // =====================================================

    Task<IEnumerable<AssignmentSubmissionDto>>
        GetByAssignmentAsync(
            Guid assignmentId);


    // =====================================================
    // GET SUBMISSION BY ASSIGNMENT + STUDENT
    // =====================================================

    Task<AssignmentSubmissionDto?>
        GetByAssignmentAndStudentAsync(
            Guid assignmentId,
            string studentId);


    // =====================================================
    // GET MY SUBMISSIONS
    // =====================================================

    Task<IEnumerable<AssignmentSubmissionDto>>
        GetMySubmissionsAsync(
            string studentId);


    // =====================================================
    // CREATE / SUBMIT ASSIGNMENT
    // =====================================================

    Task<AssignmentSubmissionDto> CreateAsync(
        CreateAssignmentSubmissionDto dto,
        string studentId);


    // =====================================================
    // UPDATE SUBMISSION
    // =====================================================

    Task<AssignmentSubmissionDto?>
        UpdateAsync(
            Guid id,
            UpdateAssignmentSubmissionDto dto,
            string studentId);


    // =====================================================
    // DELETE SUBMISSION
    // =====================================================

    Task<bool> DeleteAsync(
        Guid id,
        string studentId);


    // =====================================================
    // GRADE / EVALUATE SUBMISSION
    // =====================================================

    Task<AssignmentSubmissionDto?>
        EvaluateAsync(
            Guid id,
            EvaluateAssignmentSubmissionDto dto,
            string teacherId);


    // =====================================================
    // GET SUBMISSION COUNT
    // =====================================================

    Task<int> GetSubmissionCountAsync(
        Guid assignmentId);


    // =====================================================
    // GET PENDING SUBMISSION COUNT
    // =====================================================

    Task<int> GetPendingSubmissionCountAsync(
        Guid assignmentId);


  
}