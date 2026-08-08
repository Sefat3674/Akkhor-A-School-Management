using Akkhor.Application.DTOs.TeacherAssignments;
using Akkhor.Application.Interfaces.Services;
using Akkhor.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Akkhor.API.Controllers.Admin;

[ApiController]
[Route("api/teacher-assignments")]
public class TeacherAssignmentController : ControllerBase
{
    private readonly ITeacherAssignmentService _service;
    private readonly ApplicationDbContext _context;

    public TeacherAssignmentController(
        ITeacherAssignmentService service,
        ApplicationDbContext context)
    {
        _service = service;
        _context = context;
    }


    // =====================================================
    // GET: api/teacher-assignments
    // =====================================================

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var assignments = await _service.GetAllAsync();

            return Ok(assignments);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message = "Failed to load teacher assignments.",
                    error = ex.Message
                });
        }
    }


    // =====================================================
    // GET: api/teacher-assignments/teachers
    // =====================================================

    [HttpGet("teachers")]
    public async Task<IActionResult> GetTeachers()
    {
        try
        {
            var teachers = await (
                from user in _context.Users

                join userRole in _context.UserRoles
                    on user.Id equals userRole.UserId

                join role in _context.Roles
                    on userRole.RoleId equals role.Id

                where role.Name == "Teacher"
                      && user.IsActive

                orderby user.FullName

                select new TeacherDropdownDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email
                }
            ).ToListAsync();

            return Ok(teachers);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message = "Failed to load teachers.",
                    error = ex.Message
                });
        }
    }


    // =====================================================
    // GET: api/teacher-assignments/{id}
    // =====================================================

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var assignment =
                await _service.GetByIdAsync(id);

            if (assignment == null)
            {
                return NotFound(new
                {
                    message = "Teacher assignment not found."
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
                    message = "Failed to load teacher assignment.",
                    error = ex.Message
                });
        }
    }


    // =====================================================
    // POST: api/teacher-assignments
    // =====================================================

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateTeacherAssignmentDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var assignment =
                await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    id = assignment.Id
                },
                assignment);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message = "Failed to create teacher assignment.",
                    error = ex.Message
                });
        }
    }


    // =====================================================
    // PUT: api/teacher-assignments/{id}
    // =====================================================

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateTeacherAssignmentDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var assignment =
                await _service.UpdateAsync(id, dto);

            if (assignment == null)
            {
                return NotFound(new
                {
                    message = "Teacher assignment not found."
                });
            }

            return Ok(assignment);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message = "Failed to update teacher assignment.",
                    error = ex.Message
                });
        }
    }


    // =====================================================
    // DELETE: api/teacher-assignments/{id}
    // =====================================================

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var deleted =
                await _service.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Teacher assignment not found."
                });
            }

            return Ok(new
            {
                message = "Teacher assignment deleted successfully."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message = "Failed to delete teacher assignment.",
                    error = ex.Message
                });
        }
    }
}