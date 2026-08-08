using Akkhor.Application.Interfaces.Repositories;
using Akkhor.Domain.Entities;
using Akkhor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Akkhor.Infrastructure.Repositories;

public class AssignmentRepository : IAssignmentRepository
{
    private readonly ApplicationDbContext _context;

    public AssignmentRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }


    // =====================================================
    // GET ALL
    // =====================================================

    public async Task<IEnumerable<Assignment>> GetAllAsync()
    {
        return await _context.Assignments

            .AsNoTracking()

            .Include(x => x.Teacher)
            .Include(x => x.AcademicYear)
            .Include(x => x.Class)
            .Include(x => x.Section)
            .Include(x => x.Course)
            .Include(x => x.Subject)
            .Include(x => x.Submissions)

            .OrderByDescending(x => x.CreatedAt)

            .ToListAsync();
    }


    // =====================================================
    // GET BY ID
    // =====================================================

    public async Task<Assignment?> GetByIdAsync(
        Guid id)
    {
        return await _context.Assignments

            .Include(x => x.Teacher)
            .Include(x => x.AcademicYear)
            .Include(x => x.Class)
            .Include(x => x.Section)
            .Include(x => x.Course)
            .Include(x => x.Subject)
            .Include(x => x.Submissions)

            .FirstOrDefaultAsync(x =>
                x.Id == id);
    }


    // =====================================================
    // GET BY TEACHER
    // =====================================================

    public async Task<IEnumerable<Assignment>> GetByTeacherIdAsync(
        string teacherId)
    {
        return await _context.Assignments

            .AsNoTracking()

            .Include(x => x.Teacher)
            .Include(x => x.AcademicYear)
            .Include(x => x.Class)
            .Include(x => x.Section)
            .Include(x => x.Course)
            .Include(x => x.Subject)
            .Include(x => x.Submissions)

            .Where(x =>
                x.TeacherId == teacherId)

            .OrderByDescending(x => x.CreatedAt)

            .ToListAsync();
    }


    // =====================================================
    // GET BY ID FOR TEACHER
    // =====================================================

    public async Task<Assignment?> GetByIdForTeacherAsync(
        Guid id,
        string teacherId)
    {
        return await _context.Assignments

            .Include(x => x.Teacher)
            .Include(x => x.AcademicYear)
            .Include(x => x.Class)
            .Include(x => x.Section)
            .Include(x => x.Course)
            .Include(x => x.Subject)
            .Include(x => x.Submissions)

            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.TeacherId == teacherId);
    }


    // =====================================================
    // GET BY CLASS
    // =====================================================

    public async Task<IEnumerable<Assignment>> GetByClassIdAsync(
        Guid classId)
    {
        return await _context.Assignments

            .AsNoTracking()

            .Include(x => x.Teacher)
            .Include(x => x.AcademicYear)
            .Include(x => x.Class)
            .Include(x => x.Section)
            .Include(x => x.Course)
            .Include(x => x.Subject)
            .Include(x => x.Submissions)

            .Where(x =>
                x.ClassId == classId)

            .OrderByDescending(x => x.CreatedAt)

            .ToListAsync();
    }


    // =====================================================
    // GET BY COURSE
    // =====================================================

    public async Task<IEnumerable<Assignment>> GetByCourseIdAsync(
        Guid courseId)
    {
        return await _context.Assignments

            .AsNoTracking()

            .Include(x => x.Teacher)
            .Include(x => x.AcademicYear)
            .Include(x => x.Class)
            .Include(x => x.Section)
            .Include(x => x.Course)
            .Include(x => x.Subject)
            .Include(x => x.Submissions)

            .Where(x =>
                x.CourseId == courseId)

            .OrderByDescending(x => x.CreatedAt)

            .ToListAsync();
    }


    // =====================================================
    // GET BY SUBJECT
    // =====================================================

    public async Task<IEnumerable<Assignment>> GetBySubjectIdAsync(
        Guid subjectId)
    {
        return await _context.Assignments

            .AsNoTracking()

            .Include(x => x.Teacher)
            .Include(x => x.AcademicYear)
            .Include(x => x.Class)
            .Include(x => x.Section)
            .Include(x => x.Course)
            .Include(x => x.Subject)
            .Include(x => x.Submissions)

            .Where(x =>
                x.SubjectId == subjectId)

            .OrderByDescending(x => x.CreatedAt)

            .ToListAsync();
    }


    // =====================================================
    // CREATE
    // =====================================================

    public async Task<Assignment> CreateAsync(
        Assignment assignment)
    {
        await _context.Assignments.AddAsync(
            assignment);

        await _context.SaveChangesAsync();

        return assignment;
    }


    // =====================================================
    // UPDATE
    // =====================================================

    public async Task<Assignment> UpdateAsync(
        Assignment assignment)
    {
        _context.Assignments.Update(
            assignment);

        await _context.SaveChangesAsync();

        return assignment;
    }


    // =====================================================
    // DELETE
    // =====================================================

    public async Task<bool> DeleteAsync(
        Guid id)
    {
        var assignment =
            await _context.Assignments
                .FirstOrDefaultAsync(x =>
                    x.Id == id);

        if (assignment == null)
        {
            return false;
        }

        _context.Assignments.Remove(
            assignment);

        await _context.SaveChangesAsync();

        return true;
    }


    // =====================================================
    // EXISTS
    // =====================================================

    public async Task<bool> ExistsAsync(
        Guid id)
    {
        return await _context.Assignments
            .AnyAsync(x =>
                x.Id == id);
    }


    // =====================================================
    // DUPLICATE CHECK
    // =====================================================

    public async Task<bool> ExistsForTeacherAsync(
        string teacherId,
        Guid academicYearId,
        Guid classId,
        Guid? sectionId,
        Guid courseId,
        Guid subjectId,
        string title,
        Guid? excludeId = null)
    {
        var query =
            _context.Assignments
                .AsNoTracking()
                .Where(x =>
                    x.TeacherId == teacherId &&
                    x.AcademicYearId == academicYearId &&
                    x.ClassId == classId &&
                    x.SectionId == sectionId &&
                    x.CourseId == courseId &&
                    x.SubjectId == subjectId &&
                    x.Title.ToLower() ==
                        title.ToLower());


        if (excludeId.HasValue)
        {
            query = query.Where(x =>
                x.Id != excludeId.Value);
        }


        return await query.AnyAsync();
    }

    // =====================================================
    // GET ASSIGNMENTS FOR STUDENT
    // =====================================================

    public async Task<IEnumerable<Assignment>>
        GetAssignmentsForStudentAsync(
            string studentId)
    {
        return await _context.Assignments

            .AsNoTracking()

            // -------------------------------------------------
            // Navigation Properties
            // -------------------------------------------------

            .Include(a => a.Teacher)
            .Include(a => a.AcademicYear)
            .Include(a => a.Class)
            .Include(a => a.Section)
            .Include(a => a.Course)
            .Include(a => a.Subject)
            .Include(a => a.Submissions)

            // -------------------------------------------------
            // Published + Active
            // -------------------------------------------------

            .Where(a =>
                a.IsActive &&
                a.IsPublished

                // -------------------------------------------------
                // Student must be enrolled in the assignment class
                // -------------------------------------------------

                && _context.StudentEnrollments.Any(e =>
                    e.StudentId == studentId &&
                    e.ClassId == a.ClassId &&

                    // -------------------------------------------------
                    // Course must match
                    // -------------------------------------------------

                    e.CourseId == a.CourseId &&

                    // -------------------------------------------------
                    // Section matching
                    // -------------------------------------------------

                    (
                        a.SectionId == null ||
                        e.SectionId == a.SectionId
                    ) &&

                    // -------------------------------------------------
                    // Enrollment must be active
                    // -------------------------------------------------

                    e.Status == "Active"
                )
            )

            .OrderByDescending(a => a.Deadline)

            .ToListAsync();
    }


    // =====================================================
    // GET ASSIGNMENT FOR STUDENT
    // =====================================================

    public async Task<Assignment?>
        GetAssignmentForStudentAsync(
            Guid id,
            string studentId)
    {
        return await _context.Assignments

            .Include(a => a.Teacher)
            .Include(a => a.AcademicYear)
            .Include(a => a.Class)
            .Include(a => a.Section)
            .Include(a => a.Course)
            .Include(a => a.Subject)
            .Include(a => a.Submissions)

            .Where(a =>
                a.Id == id &&
                a.IsActive &&
                a.IsPublished

                // -------------------------------------------------
                // Student enrollment check
                // -------------------------------------------------

                && _context.StudentEnrollments.Any(e =>
                    e.StudentId == studentId &&
                    e.ClassId == a.ClassId &&
                    e.CourseId == a.CourseId &&

                    (
                        a.SectionId == null ||
                        e.SectionId == a.SectionId
                    ) &&

                    e.Status == "Active"
                )
            )

            .FirstOrDefaultAsync();
    }

}