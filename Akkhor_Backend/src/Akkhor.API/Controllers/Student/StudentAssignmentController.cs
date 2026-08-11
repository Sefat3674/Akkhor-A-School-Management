using Akkhor.Application.DTOs.Assignments;
using Akkhor.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Akkhor.API.Controllers;

[ApiController]
[Route("api/student-assignments")]
[Authorize(Roles = "Student")]
public class AssignmentController : ControllerBase
{
    private readonly IAssignmentService _service;

    public AssignmentController(
        IAssignmentService service)
    {
        _service = service;
    }


    // =====================================================
    // GET ALL ASSIGNMENTS
    // =====================================================

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var assignments =
            await _service.GetAllAsync();

        return Ok(assignments);
    }


    // =====================================================
    // GET ASSIGNMENT BY ID
    // =====================================================

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> GetById(
        Guid id)
    {
        var assignment =
            await _service.GetByIdAsync(id);

        if (assignment == null)
        {
            return NotFound(new
            {
                message = "Assignment not found."
            });
        }

        return Ok(assignment);
    }


    // =====================================================
    // GET MY ASSIGNMENTS - TEACHER
    // =====================================================

    [HttpGet("my")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> GetMyAssignments()
    {
        var teacherId =
            GetCurrentUserId();

        if (string.IsNullOrWhiteSpace(teacherId))
        {
            return Unauthorized(new
            {
                message = "Teacher ID not found."
            });
        }

        var assignments =
            await _service.GetMyAssignmentsAsync(
                teacherId);

        return Ok(assignments);
    }


    // =====================================================
    // GET MY ASSIGNMENT BY ID - TEACHER
    // =====================================================

    [HttpGet("my/{id:guid}")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> GetMyAssignmentById(
        Guid id)
    {
        var teacherId =
            GetCurrentUserId();

        if (string.IsNullOrWhiteSpace(teacherId))
        {
            return Unauthorized(new
            {
                message = "Teacher ID not found."
            });
        }

        var assignment =
            await _service.GetMyAssignmentByIdAsync(
                id,
                teacherId);

        if (assignment == null)
        {
            return NotFound(new
            {
                message = "Assignment not found."
            });
        }

        return Ok(assignment);
    }


    // =====================================================
    // GET BY CLASS
    // =====================================================

    [HttpGet("class/{classId:guid}")]
    [Authorize]
    public async Task<IActionResult> GetByClass(
        Guid classId)
    {
        var assignments =
            await _service.GetByClassAsync(
                classId);

        return Ok(assignments);
    }


    // =====================================================
    // GET BY COURSE
    // =====================================================

    [HttpGet("course/{courseId:guid}")]
    [Authorize]
    public async Task<IActionResult> GetByCourse(
        Guid courseId)
    {
        var assignments =
            await _service.GetByCourseAsync(
                courseId);

        return Ok(assignments);
    }


    // =====================================================
    // GET BY SUBJECT
    // =====================================================

    [HttpGet("subject/{subjectId:guid}")]
    [Authorize]
    public async Task<IActionResult> GetBySubject(
        Guid subjectId)
    {
        var assignments =
            await _service.GetBySubjectAsync(
                subjectId);

        return Ok(assignments);
    }


    // =====================================================
    // GET BY TEACHER
    // =====================================================

    [HttpGet("teacher/{teacherId}")]
    [Authorize]
    public async Task<IActionResult> GetByTeacher(
        string teacherId)
    {
        var assignments =
            await _service.GetByTeacherAsync(
                teacherId);

        return Ok(assignments);
    }


    // =====================================================
    // CREATE ASSIGNMENT
    // =====================================================

    [HttpPost]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Create(
        [FromBody] CreateAssignmentDto dto)
    {
        var teacherId =
            GetCurrentUserId();

        if (string.IsNullOrWhiteSpace(teacherId))
        {
            return Unauthorized(new
            {
                message = "Teacher ID not found."
            });
        }

        var assignment =
            await _service.CreateAsync(
                dto,
                teacherId);

        return CreatedAtAction(
            nameof(GetById),
            new
            {
                id = assignment.Id
            },
            assignment);
    }


    // =====================================================
    // UPDATE ASSIGNMENT
    // =====================================================

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateAssignmentDto dto)
    {
        var teacherId =
            GetCurrentUserId();

        if (string.IsNullOrWhiteSpace(teacherId))
        {
            return Unauthorized(new
            {
                message = "Teacher ID not found."
            });
        }

        var assignment =
            await _service.UpdateAsync(
                id,
                dto,
                teacherId);

        if (assignment == null)
        {
            return NotFound(new
            {
                message =
                    "Assignment not found or you are not the owner."
            });
        }

        return Ok(assignment);
    }


    // =====================================================
    // DELETE ASSIGNMENT
    // =====================================================

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Delete(
        Guid id)
    {
        var teacherId =
            GetCurrentUserId();

        if (string.IsNullOrWhiteSpace(teacherId))
        {
            return Unauthorized(new
            {
                message = "Teacher ID not found."
            });
        }

        var deleted =
            await _service.DeleteAsync(
                id,
                teacherId);

        if (!deleted)
        {
            return NotFound(new
            {
                message =
                    "Assignment not found or you are not the owner."
            });
        }

        return Ok(new
        {
            message = "Assignment deleted successfully."
        });
    }


    // =====================================================
    // PUBLISH
    // =====================================================

    [HttpPut("{id:guid}/publish")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Publish(
        Guid id)
    {
        var teacherId =
            GetCurrentUserId();

        if (string.IsNullOrWhiteSpace(teacherId))
        {
            return Unauthorized(new
            {
                message = "Teacher ID not found."
            });
        }

        var assignment =
            await _service.PublishAsync(
                id,
                teacherId);

        if (assignment == null)
        {
            return NotFound(new
            {
                message =
                    "Assignment not found or you are not the owner."
            });
        }

        return Ok(assignment);
    }


    // =====================================================
    // UNPUBLISH / DRAFT
    // =====================================================

    [HttpPut("{id:guid}/unpublish")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Unpublish(
        Guid id)
    {
        var teacherId =
            GetCurrentUserId();

        if (string.IsNullOrWhiteSpace(teacherId))
        {
            return Unauthorized(new
            {
                message = "Teacher ID not found."
            });
        }

        var assignment =
            await _service.UnpublishAsync(
                id,
                teacherId);

        if (assignment == null)
        {
            return NotFound(new
            {
                message =
                    "Assignment not found or you are not the owner."
            });
        }

        return Ok(assignment);
    }


    // =====================================================
    // GET ASSIGNMENTS FOR CURRENT STUDENT
    // =====================================================

    [HttpGet("student")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult>
        GetAssignmentsForStudent()
    {
        var studentId =
            GetCurrentUserId();

        if (string.IsNullOrWhiteSpace(studentId))
        {
            return Unauthorized(new
            {
                message = "Student ID not found."
            });
        }

        var assignments =
            await _service
                .GetAssignmentsForStudentAsync(
                    studentId);

        return Ok(assignments);
    }


    // =====================================================
    // GET SINGLE ASSIGNMENT FOR CURRENT STUDENT
    // =====================================================

    [HttpGet("student/{id:guid}")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult>
        GetAssignmentForStudent(
            Guid id)
    {
        var studentId =
            GetCurrentUserId();

        if (string.IsNullOrWhiteSpace(studentId))
        {
            return Unauthorized(new
            {
                message = "Student ID not found."
            });
        }

        var assignment =
            await _service
                .GetAssignmentForStudentAsync(
                    id,
                    studentId);

        if (assignment == null)
        {
            return NotFound(new
            {
                message =
                    "Assignment not found or you are not enrolled in this class/course."
            });
        }

        return Ok(assignment);
    }


    // =====================================================
    // GET CURRENT USER ID
    // =====================================================

    private string? GetCurrentUserId()
    {
        return User.FindFirstValue(
            ClaimTypes.NameIdentifier);
    }
}