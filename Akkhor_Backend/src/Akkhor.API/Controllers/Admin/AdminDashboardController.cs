using Akkhor.Application.DTOs.AdminDashboard;
using Akkhor.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Akkhor.API.Controllers.Admin;

[ApiController]
[Route("api/admin/dashboard")]
[Authorize(Roles = "Admin,SuperAdmin")]
public class AdminDashboardController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AdminDashboardController(
        ApplicationDbContext context)
    {
        _context = context;
    }


    // =====================================================
    // GET ADMIN DASHBOARD
    // GET: api/admin/dashboard
    // =====================================================

    [HttpGet]
    public async Task<IActionResult> GetDashboard()
    {
        try
        {
            // =================================================
            // USERS
            // =================================================

            var totalUsers =
                await _context.Users.CountAsync(
                    x => x.IsActive);


            // =================================================
            // ROLES
            // =================================================

            var roleCounts =
                await (
                    from user in _context.Users

                    join userRole in _context.UserRoles
                        on user.Id equals userRole.UserId

                    join role in _context.Roles
                        on userRole.RoleId equals role.Id

                    where user.IsActive

                    group role by role.Name
                    into grouped

                    select new
                    {
                        Role = grouped.Key,
                        Count = grouped.Count()
                    }
                ).ToListAsync();


            var totalStudents =
                roleCounts
                    .Where(x =>
                        x.Role == "Student")
                    .Select(x => x.Count)
                    .FirstOrDefault();


            var totalTeachers =
                roleCounts
                    .Where(x =>
                        x.Role == "Teacher")
                    .Select(x => x.Count)
                    .FirstOrDefault();


            var totalAdmins =
                roleCounts
                    .Where(x =>
                        x.Role == "Admin" ||
                        x.Role == "SuperAdmin")
                    .Select(x => x.Count)
                    .Sum();


            // =================================================
            // ACADEMIC DATA
            // =================================================
            var totalAcademicYears =
                 await _context.AcademicYears.CountAsync();

            var totalClasses =
                await _context.Classes.CountAsync(
                    x => x.IsActive);


            var totalSections =
                await _context.ClassSections.CountAsync();


            var totalCourses =
                await _context.Courses.CountAsync(
                    x => x.IsActive);


            var totalSubjects =
                await _context.Subjects.CountAsync();


            var totalCourseSubjects =
                await _context.CourseSubjects.CountAsync();


            var totalEnrollments =
                await _context.StudentEnrollments.CountAsync();


            var totalTeacherAssignments =
                await _context.TeacherAssignments.CountAsync(
                    x => x.IsActive);


            // =================================================
            // ASSIGNMENTS
            // =================================================

            var totalAssignments =
                await _context.Assignments.CountAsync(
                    x => x.IsActive);


            // =================================================
            // SUBMISSIONS
            // =================================================

            var totalSubmissions =
                await _context.AssignmentSubmissions.CountAsync();
            var pendingSubmissions =
                    await _context.AssignmentSubmissions.CountAsync(
                        x => x.Status == "Pending");
            // =================================================
            // RECENT ASSIGNMENTS
            // =================================================

            var recentAssignments =
                 await _context.Assignments
                     .Where(x => x.IsActive)
                     .OrderByDescending(x => x.CreatedAt)
                     .Take(5)
                     .Select(x => new RecentAssignmentDto
                     {
                         Id = x.Id,

                         Title = x.Title,

                         SubjectName =
                             x.Subject != null
                                 ? x.Subject.Name
                                 : null,

                         TeacherName =
                             x.Teacher != null
                                 ? x.Teacher.FullName
                                 : null,

                         CourseName =
                             x.Course != null
                                 ? x.Course.CourseName
                                 : null,

                         // Assignment entity uses Deadline
                         DueDate = x.Deadline,

                         Status =
                             x.IsPublished
                                 ? "Published"
                                 : "Draft"
                     })
                     .ToListAsync();


            // =================================================
            // RECENT SUBMISSIONS
            // =================================================

            var recentSubmissions =
                await _context.AssignmentSubmissions
                    .OrderByDescending(x => x.SubmittedAt)
                    .Take(5)
                    .Select(x => new RecentSubmissionDto
                    {
                        Id = x.Id,

                        AssignmentId = x.AssignmentId,

                        AssignmentTitle =
                            x.Assignment.Title,

                        StudentId =
                            x.StudentId,

                        StudentName =
                            x.Student.FullName,

                        SubmittedAt =
                            x.SubmittedAt,

                        Status =
                            x.Status,

                        Marks =
                            x.Marks
                    })
                    .ToListAsync();


            // =================================================
            // ACTIVE ACADEMIC YEAR
            // =================================================

            var activeAcademicYear =
                await _context.AcademicYears
                    .Where(x => x.IsActive)
                    .Select(x => new AcademicYearSummaryDto
                    {
                        Id = x.Id,

                        Name = x.Name,

                        StartDate = x.StartDate,

                        EndDate = x.EndDate,

                        IsActive = x.IsActive
                    })
                    .FirstOrDefaultAsync();


            // =================================================
            // RESULT
            // =================================================

            var result =
                new AdminDashboardDto
                {
                    TotalUsers =
                        totalUsers,

                    TotalStudents =
                        totalStudents,

                    TotalTeachers =
                        totalTeachers,

                    TotalAdmins =
                        totalAdmins,

                    TotalAcademicYears =
                            totalAcademicYears,

                    TotalClasses =
                        totalClasses,

                    TotalSections =
                        totalSections,

                    TotalCourses =
                        totalCourses,

                    TotalSubjects =
                        totalSubjects,

                    TotalCourseSubjects =
                        totalCourseSubjects,

                    TotalEnrollments =
                        totalEnrollments,

                    TotalTeacherAssignments =
                        totalTeacherAssignments,

                    TotalAssignments =
                        totalAssignments,

                    TotalSubmissions =
                        totalSubmissions,

                    PendingSubmissions =
                          pendingSubmissions,

                    ActiveAcademicYear =
                        activeAcademicYear,

                        RecentAssignments =
                            recentAssignments,

                    RecentSubmissions =
                            recentSubmissions
                };


            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to load admin dashboard.",

                    error =
                        ex.Message
                });
        }
    }
}