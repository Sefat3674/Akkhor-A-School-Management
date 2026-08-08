using Akkhor.Application.Interfaces.Repositories;
using Akkhor.Domain.Entities;
using Akkhor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Akkhor.Infrastructure.Repositories;

public class AssignmentSubmissionRepository
    : IAssignmentSubmissionRepository
{
    private readonly ApplicationDbContext _context;

    public AssignmentSubmissionRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }


    // =====================================================
    // GET ALL SUBMISSIONS
    // =====================================================

    public async Task<IEnumerable<AssignmentSubmission>> GetAllAsync()
    {
        return await _context.AssignmentSubmissions

            .AsNoTracking()

            .Include(x => x.Assignment)
                .ThenInclude(x => x.Teacher)

            .Include(x => x.Student)

            .OrderByDescending(x => x.SubmittedAt)

            .ToListAsync();
    }


    // =====================================================
    // GET SUBMISSION BY ID
    // =====================================================

    public async Task<AssignmentSubmission?> GetByIdAsync(
        Guid id)
    {
        return await _context.AssignmentSubmissions

            .Include(x => x.Assignment)
                .ThenInclude(x => x.Teacher)

            .Include(x => x.Student)

            .FirstOrDefaultAsync(x => x.Id == id);
    }


    // =====================================================
    // GET SUBMISSIONS BY ASSIGNMENT
    // =====================================================

    public async Task<IEnumerable<AssignmentSubmission>>
        GetByAssignmentIdAsync(Guid assignmentId)
    {
        return await _context.AssignmentSubmissions

            .AsNoTracking()

            .Include(x => x.Assignment)
                .ThenInclude(x => x.Teacher)

            .Include(x => x.Student)

            .Where(x =>
                x.AssignmentId == assignmentId)

            .OrderByDescending(x => x.SubmittedAt)

            .ToListAsync();
    }


    // =====================================================
    // GET SUBMISSION BY ASSIGNMENT + STUDENT
    // =====================================================

    public async Task<AssignmentSubmission?>
        GetByAssignmentAndStudentAsync(
            Guid assignmentId,
            string studentId)
    {
        return await _context.AssignmentSubmissions

            .Include(x => x.Assignment)
                .ThenInclude(x => x.Teacher)

            .Include(x => x.Student)

            .FirstOrDefaultAsync(x =>
                x.AssignmentId == assignmentId &&
                x.StudentId == studentId);
    }


    // =====================================================
    // GET SUBMISSIONS BY STUDENT
    // =====================================================

    public async Task<IEnumerable<AssignmentSubmission>>
        GetByStudentIdAsync(string studentId)
    {
        return await _context.AssignmentSubmissions

            .AsNoTracking()

            .Include(x => x.Assignment)
                .ThenInclude(x => x.Teacher)

            .Include(x => x.Student)

            .Where(x =>
                x.StudentId == studentId)

            .OrderByDescending(x => x.SubmittedAt)

            .ToListAsync();
    }


    // =====================================================
    // CREATE
    // =====================================================

    public async Task<AssignmentSubmission> CreateAsync(
        AssignmentSubmission submission)
    {
        await _context.AssignmentSubmissions
            .AddAsync(submission);

        await _context.SaveChangesAsync();

        return submission;
    }


    // =====================================================
    // UPDATE
    // =====================================================

    public async Task<AssignmentSubmission> UpdateAsync(
        AssignmentSubmission submission)
    {
        _context.AssignmentSubmissions
            .Update(submission);

        await _context.SaveChangesAsync();

        return submission;
    }


    // =====================================================
    // DELETE
    // =====================================================

    public async Task<bool> DeleteAsync(Guid id)
    {
        var submission =
            await _context.AssignmentSubmissions
                .FirstOrDefaultAsync(x =>
                    x.Id == id);

        if (submission == null)
        {
            return false;
        }

        _context.AssignmentSubmissions
            .Remove(submission);

        await _context.SaveChangesAsync();

        return true;
    }


    // =====================================================
    // EXISTS
    // =====================================================

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.AssignmentSubmissions
            .AnyAsync(x => x.Id == id);
    }


    // =====================================================
    // CHECK STUDENT SUBMISSION
    // =====================================================

    public async Task<bool> ExistsForStudentAsync(
        Guid assignmentId,
        string studentId)
    {
        return await _context.AssignmentSubmissions
            .AnyAsync(x =>
                x.AssignmentId == assignmentId &&
                x.StudentId == studentId);
    }


    // =====================================================
    // COUNT SUBMISSIONS
    // =====================================================

    public async Task<int> GetSubmissionCountAsync(
        Guid assignmentId)
    {
        return await _context.AssignmentSubmissions
            .CountAsync(x =>
                x.AssignmentId == assignmentId);
    }


    // =====================================================
    // COUNT PENDING SUBMISSIONS
    // =====================================================

    public async Task<int> GetPendingSubmissionCountAsync(
        Guid assignmentId)
    {
        return await _context.AssignmentSubmissions

            .CountAsync(x =>
                x.AssignmentId == assignmentId &&
                (
                    x.Status == "Pending" ||
                    x.Status == "Submitted"
                ));
    }
}