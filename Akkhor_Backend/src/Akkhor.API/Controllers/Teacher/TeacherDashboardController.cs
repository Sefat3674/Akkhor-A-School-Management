using Akkhor.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Akkhor.API.Controllers.Teacher;

[ApiController]
[Route("api/teacher-dashboard")]
[Authorize(Roles = "Teacher")]
public class TeacherDashboardController : ControllerBase
{
    private readonly IAssignmentService _assignmentService;
    private readonly ITeacherClassService _teacherClassService;

    public TeacherDashboardController(
        IAssignmentService assignmentService,
        ITeacherClassService teacherClassService)
    {
        _assignmentService = assignmentService;
        _teacherClassService = teacherClassService;
    }


    // =====================================================
    // GET TEACHER DASHBOARD
    // =====================================================

    // GET: api/teacher-dashboard
    [HttpGet]
    public async Task<IActionResult> GetDashboard(
        CancellationToken cancellationToken)
    {
        try
        {
            // -------------------------------------------------
            // Get logged-in teacher ID
            // -------------------------------------------------

            var teacherId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(teacherId))
            {
                return Unauthorized(new
                {
                    message =
                        "Teacher identity could not be determined."
                });
            }


            // -------------------------------------------------
            // Get teacher classes
            // -------------------------------------------------

            var classes =
                await _teacherClassService.GetMyClassesAsync(
                    teacherId,
                    cancellationToken);


            // -------------------------------------------------
            // Get teacher assignments
            // -------------------------------------------------

            var assignments =
                await _assignmentService.GetByTeacherAsync(
                    teacherId);


            // -------------------------------------------------
            // Calculate statistics
            // -------------------------------------------------

            var totalClasses =
                classes?.Count() ?? 0;

            var totalAssignments =
                assignments?.Count() ?? 0;


            // -------------------------------------------------
            // Published assignments
            // -------------------------------------------------

            var publishedAssignments =
                assignments?
                    .Count(x => x.IsPublished)
                ?? 0;


            // -------------------------------------------------
            // Draft assignments
            // -------------------------------------------------

            var draftAssignments =
                assignments?
                    .Count(x => !x.IsPublished)
                ?? 0;


            // -------------------------------------------------
            // Recent assignments
            // -------------------------------------------------

            var recentAssignments =
                assignments?
                    .OrderByDescending(x => x.CreatedAt)
                    .Take(5)
                    .ToList()
                ?? new();


            // -------------------------------------------------
            // Dashboard response
            // -------------------------------------------------

            return Ok(new
            {
                totalClasses,
                totalAssignments,
                publishedAssignments,
                draftAssignments,
                recentAssignments
            });
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to load teacher dashboard.",

                    error =
                        ex.Message
                });
        }
    }


    // =====================================================
    // GET DASHBOARD SUMMARY
    // =====================================================

    // GET: api/teacher-dashboard/summary
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(
        CancellationToken cancellationToken)
    {
        try
        {
            // -------------------------------------------------
            // Get logged-in teacher ID
            // -------------------------------------------------

            var teacherId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(teacherId))
            {
                return Unauthorized(new
                {
                    message =
                        "Teacher identity could not be determined."
                });
            }


            // -------------------------------------------------
            // Get classes
            // -------------------------------------------------

            var classes =
                await _teacherClassService.GetMyClassesAsync(
                    teacherId,
                    cancellationToken);


            // -------------------------------------------------
            // Get assignments
            // -------------------------------------------------

            var assignments =
                await _assignmentService.GetByTeacherAsync(
                    teacherId);


            // -------------------------------------------------
            // Calculate statistics
            // -------------------------------------------------

            var totalClasses =
                classes?.Count() ?? 0;

            var totalAssignments =
                assignments?.Count() ?? 0;

            var publishedAssignments =
                assignments?
                    .Count(x => x.IsPublished)
                ?? 0;

            var draftAssignments =
                assignments?
                    .Count(x => !x.IsPublished)
                ?? 0;


            // -------------------------------------------------
            // Response
            // -------------------------------------------------

            return Ok(new
            {
                totalClasses,
                totalAssignments,
                publishedAssignments,
                draftAssignments
            });
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to load dashboard summary.",

                    error =
                        ex.Message
                });
        }
    }


    // =====================================================
    // GET RECENT ASSIGNMENTS
    // =====================================================

    // GET: api/teacher-dashboard/recent-assignments
    [HttpGet("recent-assignments")]
    public async Task<IActionResult> GetRecentAssignments()
    {
        try
        {
            // -------------------------------------------------
            // Get logged-in teacher ID
            // -------------------------------------------------

            var teacherId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(teacherId))
            {
                return Unauthorized(new
                {
                    message =
                        "Teacher identity could not be determined."
                });
            }


            // -------------------------------------------------
            // Get assignments
            // -------------------------------------------------

            var assignments =
                await _assignmentService.GetByTeacherAsync(
                    teacherId);


            // -------------------------------------------------
            // Get recent assignments
            // -------------------------------------------------

            var recentAssignments =
                assignments?
                    .OrderByDescending(x => x.CreatedAt)
                    .Take(5)
                    .ToList()
                ?? new();


            return Ok(recentAssignments);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to load recent assignments.",

                    error =
                        ex.Message
                });
        }
    }


    // =====================================================
    // GET PUBLISHED ASSIGNMENTS
    // =====================================================

    // GET: api/teacher-dashboard/published-assignments
    [HttpGet("published-assignments")]
    public async Task<IActionResult> GetPublishedAssignments()
    {
        try
        {
            // -------------------------------------------------
            // Get logged-in teacher ID
            // -------------------------------------------------

            var teacherId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(teacherId))
            {
                return Unauthorized(new
                {
                    message =
                        "Teacher identity could not be determined."
                });
            }


            // -------------------------------------------------
            // Get assignments
            // -------------------------------------------------

            var assignments =
                await _assignmentService.GetByTeacherAsync(
                    teacherId);


            // -------------------------------------------------
            // Published assignments
            // -------------------------------------------------

            var publishedAssignments =
                assignments?
                    .Where(x => x.IsPublished)
                    .OrderByDescending(x => x.CreatedAt)
                    .ToList()
                ?? new();


            return Ok(publishedAssignments);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to load published assignments.",

                    error =
                        ex.Message
                });
        }
    }


    // =====================================================
    // GET DRAFT ASSIGNMENTS
    // =====================================================

    // GET: api/teacher-dashboard/draft-assignments
    [HttpGet("draft-assignments")]
    public async Task<IActionResult> GetDraftAssignments()
    {
        try
        {
            // -------------------------------------------------
            // Get logged-in teacher ID
            // -------------------------------------------------

            var teacherId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(teacherId))
            {
                return Unauthorized(new
                {
                    message =
                        "Teacher identity could not be determined."
                });
            }


            // -------------------------------------------------
            // Get assignments
            // -------------------------------------------------

            var assignments =
                await _assignmentService.GetByTeacherAsync(
                    teacherId);


            // -------------------------------------------------
            // Draft assignments
            // -------------------------------------------------

            var draftAssignments =
                assignments?
                    .Where(x => !x.IsPublished)
                    .OrderByDescending(x => x.CreatedAt)
                    .ToList()
                ?? new();


            return Ok(draftAssignments);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to load draft assignments.",

                    error =
                        ex.Message
                });
        }
    }


    // =====================================================
    // GET TEACHER CLASSES
    // =====================================================

    // GET: api/teacher-dashboard/classes
    [HttpGet("classes")]
    public async Task<IActionResult> GetClasses(
        CancellationToken cancellationToken)
    {
        try
        {
            // -------------------------------------------------
            // Get logged-in teacher ID
            // -------------------------------------------------

            var teacherId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(teacherId))
            {
                return Unauthorized(new
                {
                    message =
                        "Teacher identity could not be determined."
                });
            }


            // -------------------------------------------------
            // Get classes
            // -------------------------------------------------

            var classes =
                await _teacherClassService.GetMyClassesAsync(
                    teacherId,
                    cancellationToken);


            return Ok(classes);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to load teacher classes.",

                    error =
                        ex.Message
                });
        }
    }
}