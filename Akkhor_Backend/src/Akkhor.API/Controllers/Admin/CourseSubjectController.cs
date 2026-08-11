using Akkhor.Application.DTOs.CourseSubjects;
using Akkhor.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Akkhor.API.Controllers.Admin;

[ApiController]
[Route("api/course-subjects")]
[Authorize(Roles = "Admin,SuperAdmin")]
public class CourseSubjectController : ControllerBase
{
    private readonly ICourseSubjectService _service;

    public CourseSubjectController(ICourseSubjectService service)
    {
        _service = service;
    }

    // GET: api/course-subjects
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _service.GetAllAsync();
        return Ok(data);
    }

    // GET: api/course-subjects/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var data = await _service.GetByIdAsync(id);

        if (data == null)
            return NotFound();

        return Ok(data);
    }

    // POST: api/course-subjects
    [HttpPost]
    public async Task<IActionResult> Create(CreateCourseSubjectDto dto)
    {
        var result = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }

    // PUT: api/course-subjects/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateCourseSubjectDto dto)
    {
        var success = await _service.UpdateAsync(id, dto);

        if (!success)
            return NotFound();

        return NoContent();
    }

    // DELETE: api/course-subjects/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _service.DeleteAsync(id);

        if (!success)
            return NotFound();

        return NoContent();
    }
}