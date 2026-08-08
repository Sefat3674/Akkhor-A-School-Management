using Akkhor.Application.Interfaces.Repositories;
using Akkhor.Domain.Entities;
using Akkhor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Akkhor.Infrastructure.Repositories;

public class TeacherAssignmentRepository : ITeacherAssignmentRepository
{
    private readonly ApplicationDbContext _context;

    public TeacherAssignmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }


    // =====================================================
    // GET ALL
    // =====================================================

    public async Task<IEnumerable<TeacherAssignment>> GetAllAsync()
    {
        return await _context.TeacherAssignments
            .AsNoTracking()

            .Include(x => x.Teacher)
            .Include(x => x.AcademicYear)
            .Include(x => x.Class)
            .Include(x => x.Section)
            .Include(x => x.Course)
            .Include(x => x.Subject)

            .OrderByDescending(x => x.CreatedAt)

            .ToListAsync();
    }


    // =====================================================
    // GET BY ID
    // =====================================================

    public async Task<TeacherAssignment?> GetByIdAsync(Guid id)
    {
        return await _context.TeacherAssignments
            .AsNoTracking()

            .Include(x => x.Teacher)
            .Include(x => x.AcademicYear)
            .Include(x => x.Class)
            .Include(x => x.Section)
            .Include(x => x.Course)
            .Include(x => x.Subject)

            .FirstOrDefaultAsync(x => x.Id == id);
    }


    // =====================================================
    // CHECK DUPLICATE
    // =====================================================

    public async Task<bool> ExistsAsync(
        string teacherId,
        Guid academicYearId,
        Guid classId,
        Guid? sectionId,
        Guid courseId,
        Guid subjectId,
        Guid? excludeId = null)
    {
        var query = _context.TeacherAssignments
            .AsNoTracking()
            .Where(x =>
                x.TeacherId == teacherId &&
                x.AcademicYearId == academicYearId &&
                x.ClassId == classId &&
                x.SectionId == sectionId &&
                x.CourseId == courseId &&
                x.SubjectId == subjectId
            );

        if (excludeId.HasValue)
        {
            query = query.Where(x => x.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }


    // =====================================================
    // CREATE
    // =====================================================

    public async Task<TeacherAssignment> CreateAsync(
        TeacherAssignment teacherAssignment)
    {
        await _context.TeacherAssignments
            .AddAsync(teacherAssignment);

        await _context.SaveChangesAsync();

        return teacherAssignment;
    }


    // =====================================================
    // UPDATE
    // =====================================================

    public async Task<TeacherAssignment> UpdateAsync(
        TeacherAssignment teacherAssignment)
    {
        _context.TeacherAssignments.Update(teacherAssignment);

        await _context.SaveChangesAsync();

        return teacherAssignment;
    }


    // =====================================================
    // DELETE
    // =====================================================

    public async Task<bool> DeleteAsync(Guid id)
    {
        var assignment = await _context.TeacherAssignments
            .FirstOrDefaultAsync(x => x.Id == id);

        if (assignment == null)
        {
            return false;
        }

        _context.TeacherAssignments.Remove(assignment);

        await _context.SaveChangesAsync();

        return true;
    }
}