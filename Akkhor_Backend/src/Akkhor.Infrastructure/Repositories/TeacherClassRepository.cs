using Akkhor.Application.DTOs.TeacherClasses;
using Akkhor.Application.Interfaces.Repositories;
using Akkhor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Akkhor.Infrastructure.Repositories;

public class TeacherClassRepository : ITeacherClassRepository
{
    private readonly ApplicationDbContext _context;

    public TeacherClassRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }

    // =====================================================
    // GET MY CLASSES
    // =====================================================

    public async Task<List<TeacherClassDto>> GetMyClassesAsync(
        string teacherId,
        CancellationToken cancellationToken = default)
    {
        return await _context.TeacherAssignments
            .AsNoTracking()

            // Only assignments belonging to logged-in teacher
            .Where(x =>
                x.TeacherId == teacherId &&
                x.IsActive)

            // Include related data
            .Include(x => x.AcademicYear)
            .Include(x => x.Class)
            .Include(x => x.Section)
            .Include(x => x.Course)
            .Include(x => x.Subject)

            .Select(x => new TeacherClassDto
            {
                // Assignment
                AssignmentId = x.Id,

                TeacherId = x.TeacherId,

                // Academic Year
                AcademicYearId = x.AcademicYearId,

                AcademicYearName =
                    x.AcademicYear.Name,

                // Class
                ClassId = x.ClassId,

                ClassName =
                    x.Class.Name,

                // Section
                SectionId = x.SectionId,

                SectionName =
                    x.Section != null
                        ? x.Section.SectionName
                        : null,

                // Room Number
                RoomNumber =
                    x.Section != null
                        ? x.Section.RoomNumber
                        : null,

                // Course
                CourseId = x.CourseId,

                CourseName =
                    x.Course.CourseName,

                // Subject
                SubjectId = x.SubjectId,

                SubjectName =
                    x.Subject.Name,

                // Assignment settings
                IsPrimary = x.IsPrimary,

                IsActive = x.IsActive
            })

            // Useful ordering for My Classes
            .OrderBy(x => x.AcademicYearName)
            .ThenBy(x => x.ClassName)
            .ThenBy(x => x.SectionName)
            .ThenBy(x => x.CourseName)
            .ThenBy(x => x.SubjectName)

            .ToListAsync(cancellationToken);
    }
}