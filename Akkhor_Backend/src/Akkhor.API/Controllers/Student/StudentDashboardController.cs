using Akkhor.Application.DTOs.StudentDashboard;
using Akkhor.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Akkhor.API.Controllers.Student;

[ApiController]
[Route("api/student/dashboard")]
[Authorize(Roles = "Student")]
public class StudentDashboardController : ControllerBase
{
    private readonly IStudentDashboardService _dashboardService;

    public StudentDashboardController(
        IStudentDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    // =====================================================
    // GET COMPLETE STUDENT DASHBOARD
    // GET: api/student/dashboard
    // =====================================================

    [HttpGet]
    public async Task<IActionResult> GetDashboard()
    {
        var studentId = GetCurrentUserId();

        if (string.IsNullOrWhiteSpace(studentId))
        {
            return Unauthorized(new
            {
                message = "Student ID not found."
            });
        }

        var dashboard =
            await _dashboardService
                .GetDashboardAsync(studentId);

        if (dashboard == null)
        {
            return NotFound(new
            {
                message = "Student dashboard not found."
            });
        }

        return Ok(dashboard);
    }

    // =====================================================
    // GET STATISTICS
    // GET: api/student/dashboard/statistics
    // =====================================================

    [HttpGet("statistics")]
    public async Task<IActionResult> GetStatistics()
    {
        var studentId = GetCurrentUserId();

        if (string.IsNullOrWhiteSpace(studentId))
        {
            return Unauthorized(new
            {
                message = "Student ID not found."
            });
        }

        var statistics =
            await _dashboardService
                .GetStatisticsAsync(studentId);

        return Ok(statistics);
    }

    // =====================================================
    // GET RECENT ASSIGNMENTS
    // GET: api/student/dashboard/recent-assignments?limit=5
    // =====================================================

    [HttpGet("recent-assignments")]
    public async Task<IActionResult> GetRecentAssignments(
        [FromQuery] int limit = 5)
    {
        var studentId = GetCurrentUserId();

        if (string.IsNullOrWhiteSpace(studentId))
        {
            return Unauthorized(new
            {
                message = "Student ID not found."
            });
        }

        limit = NormalizeLimit(limit);

        var assignments =
            await _dashboardService
                .GetRecentAssignmentsAsync(
                    studentId,
                    limit);

        return Ok(assignments);
    }

    // =====================================================
    // GET UPCOMING ASSIGNMENTS
    // GET: api/student/dashboard/upcoming-assignments?limit=5
    // =====================================================

    [HttpGet("upcoming-assignments")]
    public async Task<IActionResult> GetUpcomingAssignments(
        [FromQuery] int limit = 5)
    {
        var studentId = GetCurrentUserId();

        if (string.IsNullOrWhiteSpace(studentId))
        {
            return Unauthorized(new
            {
                message = "Student ID not found."
            });
        }

        limit = NormalizeLimit(limit);

        var assignments =
            await _dashboardService
                .GetUpcomingAssignmentsAsync(
                    studentId,
                    limit);

        return Ok(assignments);
    }

    // =====================================================
    // GET RECENT SUBMISSIONS
    // GET: api/student/dashboard/recent-submissions?limit=5
    // =====================================================

    [HttpGet("recent-submissions")]
    public async Task<IActionResult> GetRecentSubmissions(
        [FromQuery] int limit = 5)
    {
        var studentId = GetCurrentUserId();

        if (string.IsNullOrWhiteSpace(studentId))
        {
            return Unauthorized(new
            {
                message = "Student ID not found."
            });
        }

        limit = NormalizeLimit(limit);

        var submissions =
            await _dashboardService
                .GetRecentSubmissionsAsync(
                    studentId,
                    limit);

        return Ok(submissions);
    }

    // =====================================================
    // GET CURRENT USER ID
    // =====================================================

    private string? GetCurrentUserId()
    {
        return User.FindFirstValue(
            ClaimTypes.NameIdentifier);
    }

    // =====================================================
    // NORMALIZE LIMIT
    // =====================================================

    private static int NormalizeLimit(int limit)
    {
        if (limit <= 0)
        {
            return 5;
        }

        return Math.Min(limit, 50);
    }
}