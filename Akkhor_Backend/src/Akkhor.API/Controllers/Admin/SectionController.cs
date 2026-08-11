using Akkhor.Application.DTOs.Sections;
using Akkhor.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Akkhor.API.Controllers.Academic;

[ApiController]
[Route("api/sections")]
[Authorize(Roles = "Admin,SuperAdmin")]
public class SectionController : ControllerBase
{
    private readonly ISectionService _service;

    public SectionController(ISectionService service)
    {
        _service = service;
    }

    // GET: api/sections
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();

        return Ok(result);
    }

    // GET: api/sections/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);

        if (result == null)
        {
            return NotFound(new
            {
                message = "Section not found."
            });
        }

        return Ok(result);
    }

    // POST: api/sections
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateSectionDto dto)
    {
        try
        {
            var result = await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    id = result.Id
                },
                result);
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    // PUT: api/sections/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateSectionDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);

        if (!updated)
        {
            return NotFound(new
            {
                message = "Section not found."
            });
        }

        return Ok(new
        {
            message = "Section updated successfully."
        });
    }

    // DELETE: api/sections/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message = "Section not found."
            });
        }

        return Ok(new
        {
            message = "Section deleted successfully."
        });
    }
}