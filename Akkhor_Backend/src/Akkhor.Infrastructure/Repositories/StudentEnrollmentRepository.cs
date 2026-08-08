using Akkhor.Application.Interfaces.Repositories;
using Akkhor.Domain.Entities;
using Akkhor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Akkhor.Infrastructure.Repositories;

public class StudentEnrollmentRepository : IStudentEnrollmentRepository
{
    private readonly ApplicationDbContext _context;


    public StudentEnrollmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }



    // GET ALL
    public async Task<List<StudentEnrollment>> GetAllAsync()
    {
        return await _context.StudentEnrollments
            .Include(x => x.Student)
            .Include(x => x.Class)
            .Include(x => x.Course)
            .Include(x => x.Section)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }



    // GET BY ID
    public async Task<StudentEnrollment?> GetByIdAsync(Guid id)
    {
        return await _context.StudentEnrollments
            .Include(x => x.Student)
            .Include(x => x.Class)
            .Include(x => x.Course)
            .Include(x => x.Section)
            .FirstOrDefaultAsync(x => x.Id == id);
    }



    // CREATE
    public async Task AddAsync(StudentEnrollment entity)
    {
        entity.Id = Guid.NewGuid();

        entity.CreatedAt = DateTime.UtcNow;

        await _context.StudentEnrollments.AddAsync(entity);

        await _context.SaveChangesAsync();
    }



    // UPDATE
    public async Task UpdateAsync(StudentEnrollment entity)
    {
        _context.StudentEnrollments.Update(entity);

        await _context.SaveChangesAsync();
    }



    // DELETE
    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.StudentEnrollments
            .FirstOrDefaultAsync(x => x.Id == id);


        if (entity == null)
            return;


        _context.StudentEnrollments.Remove(entity);

        await _context.SaveChangesAsync();
    }



    // CHECK DUPLICATE ENROLLMENT
    public async Task<bool> ExistsAsync(
        string studentId,
        Guid classId,
        Guid courseId)
    {
        return await _context.StudentEnrollments
            .AnyAsync(x =>
                x.StudentId == studentId &&
                x.ClassId == classId &&
                x.CourseId == courseId);
    }


    // =====================================================
    // GET STUDENTS
    //
    // Roles
    //    ↓
    // find STUDENT role
    //    ↓
    // UserRoles
    //    ↓
    // get UserId
    //    ↓
    // Users
    //    ↓
    // UserName
    // =====================================================

    public async Task<List<Users>> GetStudentsAsync()
    {
        var studentRoleIds =
            await _context.Roles

                .Where(r =>
                    r.Name == "STUDENT" ||
                    r.NormalizedName == "STUDENT")

                .Select(r => r.Id)

                .ToListAsync();


        return await _context.Users

            .Where(u =>
                _context.UserRoles.Any(ur =>
                    ur.UserId == u.Id &&
                    studentRoleIds.Contains(ur.RoleId)))

            .OrderBy(u => u.UserName)

            .ToListAsync();
    }


    // =====================================================
    // GET CLASSES
    // =====================================================

    public async Task<List<Class>> GetClassesAsync()
    {
        return await _context.Classes

            .Where(x => x.IsActive)

            .OrderBy(x => x.Name)

            .ToListAsync();
    }


    // =====================================================
    // GET COURSES BY CLASS
    // =====================================================

    public async Task<List<Course>>
        GetCoursesByClassIdAsync(Guid classId)
    {
        return await _context.Courses

            .Where(x =>
                x.ClassId == classId &&
                x.IsActive)

            .OrderBy(x => x.CourseName)

            .ToListAsync();
    }


    // =====================================================
    // GET SECTIONS BY CLASS
    // =====================================================

    public async Task<List<ClassSection>>
        GetSectionsByClassIdAsync(Guid classId)
    {
        return await _context.ClassSections

            .Where(x =>
                x.ClassId == classId &&
                x.IsActive)

            .OrderBy(x => x.SectionName)

            .ToListAsync();
    }



}