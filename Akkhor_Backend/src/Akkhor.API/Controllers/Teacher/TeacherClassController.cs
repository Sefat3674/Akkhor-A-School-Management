using Akkhor.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Akkhor.API.Controllers.Teacher;

[ApiController]
[Route("api/teacher-classes")]
[Authorize(Roles = "Teacher")]
public class TeacherClassController : ControllerBase
{
    private readonly ITeacherClassService _service;

    public TeacherClassController(
        ITeacherClassService service)
    {
        _service = service;
    }


    // =====================================================
    // GET MY CLASSES
    // =====================================================

    // GET: api/teacher-classes/my-classes
    [HttpGet("my-classes")]
    public async Task<IActionResult> GetMyClasses(
        CancellationToken cancellationToken)
    {
        // -------------------------------------------------
        // Get logged-in teacher ID
        // -------------------------------------------------

        var teacherId =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(teacherId))
        {
            return Unauthorized(new
            {
                message = "Teacher identity could not be determined."
            });
        }


        // -------------------------------------------------
        // Get assigned classes
        // -------------------------------------------------

        var classes =
            await _service.GetMyClassesAsync(
                teacherId,
                cancellationToken);


        // -------------------------------------------------
        // Return response
        // -------------------------------------------------

        return Ok(classes);
    }
}