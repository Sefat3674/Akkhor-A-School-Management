using Akkhor.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Akkhor.API.Controllers.Admin;

[ApiController]
[Route("api/admin/assignments")]
[Authorize(Roles = "Admin,SuperAdmin")]
public class AdminAssignmentController : ControllerBase
{
    private readonly IAssignmentService _assignmentService;

    public AdminAssignmentController(
        IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    // =====================================================
    // GET ALL ASSIGNMENTS
    // =====================================================
    // GET: api/admin/assignments
    // =====================================================

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var assignments =
                await _assignmentService.GetAllAsync();

            return Ok(assignments);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to load assignments.",

                    error =
                        ex.Message
                });
        }
    }


    // =====================================================
    // GET ASSIGNMENT BY ID
    // =====================================================
    // GET: api/admin/assignments/{id}
    // =====================================================

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new
                {
                    message =
                        "Assignment ID is required."
                });
            }

            var assignment =
                await _assignmentService.GetByIdAsync(id);

            if (assignment == null)
            {
                return NotFound(new
                {
                    message =
                        "Assignment not found."
                });
            }

            return Ok(assignment);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to load assignment.",

                    error =
                        ex.Message
                });
        }
    }


    // =====================================================
    // GET ASSIGNMENTS BY CLASS
    // =====================================================
    // GET: api/admin/assignments/class/{classId}
    // =====================================================

    [HttpGet("class/{classId:guid}")]
    public async Task<IActionResult> GetByClass(
        Guid classId)
    {
        try
        {
            if (classId == Guid.Empty)
            {
                return BadRequest(new
                {
                    message =
                        "Class ID is required."
                });
            }

            var assignments =
                await _assignmentService.GetByClassAsync(
                    classId);

            return Ok(assignments);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to load class assignments.",

                    error =
                        ex.Message
                });
        }
    }


    // =====================================================
    // GET ASSIGNMENTS BY COURSE
    // =====================================================
    // GET: api/admin/assignments/course/{courseId}
    // =====================================================

    [HttpGet("course/{courseId:guid}")]
    public async Task<IActionResult> GetByCourse(
        Guid courseId)
    {
        try
        {
            if (courseId == Guid.Empty)
            {
                return BadRequest(new
                {
                    message =
                        "Course ID is required."
                });
            }

            var assignments =
                await _assignmentService.GetByCourseAsync(
                    courseId);

            return Ok(assignments);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to load course assignments.",

                    error =
                        ex.Message
                });
        }
    }


    // =====================================================
    // GET ASSIGNMENTS BY SUBJECT
    // =====================================================
    // GET: api/admin/assignments/subject/{subjectId}
    // =====================================================

    [HttpGet("subject/{subjectId:guid}")]
    public async Task<IActionResult> GetBySubject(
        Guid subjectId)
    {
        try
        {
            if (subjectId == Guid.Empty)
            {
                return BadRequest(new
                {
                    message =
                        "Subject ID is required."
                });
            }

            var assignments =
                await _assignmentService.GetBySubjectAsync(
                    subjectId);

            return Ok(assignments);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to load subject assignments.",

                    error =
                        ex.Message
                });
        }
    }


    // =====================================================
    // GET ASSIGNMENTS BY TEACHER
    // =====================================================
    // GET: api/admin/assignments/teacher/{teacherId}
    // =====================================================

    [HttpGet("teacher/{teacherId}")]
    public async Task<IActionResult> GetByTeacher(
        string teacherId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(teacherId))
            {
                return BadRequest(new
                {
                    message =
                        "Teacher ID is required."
                });
            }

            var assignments =
                await _assignmentService.GetByTeacherAsync(
                    teacherId);

            return Ok(assignments);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to load teacher assignments.",

                    error =
                        ex.Message
                });
        }
    }
}