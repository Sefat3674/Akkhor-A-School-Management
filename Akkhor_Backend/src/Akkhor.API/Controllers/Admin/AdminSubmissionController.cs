using Akkhor.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Akkhor.API.Controllers.Admin;

[ApiController]
[Route("api/admin/submissions")]
[Authorize(Roles = "Admin,SuperAdmin")]
public class AdminSubmissionController : ControllerBase
{
    private readonly IAssignmentSubmissionService
        _submissionService;

    public AdminSubmissionController(
        IAssignmentSubmissionService submissionService)
    {
        _submissionService =
            submissionService;
    }


    // =====================================================
    // GET ALL SUBMISSIONS
    // =====================================================
    // GET: api/admin/submissions
    // =====================================================

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var submissions =
                await _submissionService.GetAllAsync();

            return Ok(submissions);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to load submissions.",

                    error =
                        ex.Message
                });
        }
    }


    // =====================================================
    // GET SUBMISSION BY ID
    // =====================================================
    // GET: api/admin/submissions/{id}
    // =====================================================

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id)
    {
        try
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new
                {
                    message =
                        "Submission ID is required."
                });
            }

            var submission =
                await _submissionService.GetByIdAsync(
                    id);

            if (submission == null)
            {
                return NotFound(new
                {
                    message =
                        "Submission not found."
                });
            }

            return Ok(submission);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to load submission.",

                    error =
                        ex.Message
                });
        }
    }


    // =====================================================
    // GET SUBMISSIONS BY ASSIGNMENT
    // =====================================================
    // GET: api/admin/submissions/assignment/{assignmentId}
    // =====================================================

    [HttpGet("assignment/{assignmentId:guid}")]
    public async Task<IActionResult> GetByAssignment(
        Guid assignmentId)
    {
        try
        {
            if (assignmentId == Guid.Empty)
            {
                return BadRequest(new
                {
                    message =
                        "Assignment ID is required."
                });
            }

            var submissions =
                await _submissionService
                    .GetByAssignmentAsync(
                        assignmentId);

            return Ok(submissions);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to load assignment submissions.",

                    error =
                        ex.Message
                });
        }
    }


    // =====================================================
    // GET SUBMISSION BY ASSIGNMENT + STUDENT
    // =====================================================
    // GET:
    // api/admin/submissions/assignment/{assignmentId}/student/{studentId}
    // =====================================================

    [HttpGet(
        "assignment/{assignmentId:guid}/student/{studentId}")]
    public async Task<IActionResult>
        GetByAssignmentAndStudent(
            Guid assignmentId,
            string studentId)
    {
        try
        {
            if (assignmentId == Guid.Empty)
            {
                return BadRequest(new
                {
                    message =
                        "Assignment ID is required."
                });
            }

            if (string.IsNullOrWhiteSpace(studentId))
            {
                return BadRequest(new
                {
                    message =
                        "Student ID is required."
                });
            }

            var submission =
                await _submissionService
                    .GetByAssignmentAndStudentAsync(
                        assignmentId,
                        studentId);

            if (submission == null)
            {
                return NotFound(new
                {
                    message =
                        "Submission not found."
                });
            }

            return Ok(submission);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to load student submission.",

                    error =
                        ex.Message
                });
        }
    }


    // =====================================================
    // GET SUBMISSIONS BY STUDENT
    // =====================================================
    // GET: api/admin/submissions/student/{studentId}
    // =====================================================

    [HttpGet("student/{studentId}")]
    public async Task<IActionResult> GetByStudent(
        string studentId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(studentId))
            {
                return BadRequest(new
                {
                    message =
                        "Student ID is required."
                });
            }

            var submissions =
                await _submissionService
                    .GetMySubmissionsAsync(
                        studentId);

            return Ok(submissions);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to load student submissions.",

                    error =
                        ex.Message
                });
        }
    }


    // =====================================================
    // GET SUBMISSION COUNT
    // =====================================================
    // GET:
    // api/admin/submissions/assignment/{assignmentId}/count
    // =====================================================

    [HttpGet(
        "assignment/{assignmentId:guid}/count")]
    public async Task<IActionResult> GetSubmissionCount(
        Guid assignmentId)
    {
        try
        {
            if (assignmentId == Guid.Empty)
            {
                return BadRequest(new
                {
                    message =
                        "Assignment ID is required."
                });
            }

            var count =
                await _submissionService
                    .GetSubmissionCountAsync(
                        assignmentId);

            return Ok(new
            {
                assignmentId,
                submissionCount = count
            });
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to get submission count.",

                    error =
                        ex.Message
                });
        }
    }


    // =====================================================
    // GET PENDING SUBMISSION COUNT
    // =====================================================
    // GET:
    // api/admin/submissions/assignment/{assignmentId}/pending-count
    // =====================================================

    [HttpGet(
        "assignment/{assignmentId:guid}/pending-count")]
    public async Task<IActionResult>
        GetPendingSubmissionCount(
            Guid assignmentId)
    {
        try
        {
            if (assignmentId == Guid.Empty)
            {
                return BadRequest(new
                {
                    message =
                        "Assignment ID is required."
                });
            }

            var count =
                await _submissionService
                    .GetPendingSubmissionCountAsync(
                        assignmentId);

            return Ok(new
            {
                assignmentId,
                pendingSubmissionCount = count
            });
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to get pending submission count.",

                    error =
                        ex.Message
                });
        }
    }
}