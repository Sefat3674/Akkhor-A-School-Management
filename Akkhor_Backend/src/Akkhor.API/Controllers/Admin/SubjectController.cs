using Akkhor.Application.DTOs.Subjects;
using Akkhor.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Akkhor.API.Controllers.Admin;

[ApiController]
[Route("api/subjects")]
public class SubjectController : ControllerBase
{
    private readonly ISubjectService _service;

    public SubjectController(ISubjectService service)
    {
        _service = service;
    }

    // GET: api/subjects
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _service.GetAllAsync();
        return Ok(data);
    }

    // GET: api/subjects/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var data = await _service.GetByIdAsync(id);

        if (data == null)
            return NotFound();

        return Ok(data);
    }

    // POST: api/subjects
    [HttpPost]
    public async Task<IActionResult> Create(CreateSubjectDto dto)
    {
        var id = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id },
            null);
    }

    // PUT: api/subjects/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateSubjectDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);

        if (!result)
            return NotFound();

        return NoContent();
    }

    // DELETE: api/subjects/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.DeleteAsync(id);

        if (!result)
            return NotFound();

        return NoContent();
    }
}