
using Akkhor.Application.DTOs.ApplicationSettings;
using Akkhor.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Akkhor.API.Controllers.Admin;

[ApiController]
[Route("api/application-settings")]
[Authorize]
public class ApplicationSettingsController : ControllerBase
{
    private readonly IApplicationSettingService _service;

    public ApplicationSettingsController(
        IApplicationSettingService service)
    {
        _service = service;
    }

    // =====================================================
    // GET ALL
    // GET: api/application-settings
    // =====================================================

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var settings = await _service.GetAllAsync();

        return Ok(settings);
    }

    // =====================================================
    // GET BY ID
    // GET: api/application-settings/{id}
    // =====================================================

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var setting = await _service.GetByIdAsync(id);

        if (setting == null)
        {
            return NotFound(new
            {
                message = "Application setting not found."
            });
        }

        return Ok(setting);
    }

    // =====================================================
    // GET BY KEY
    // GET: api/application-settings/key/{key}
    // =====================================================

    [HttpGet("key/{key}")]
    public async Task<IActionResult> GetByKey(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return BadRequest(new
            {
                message = "Setting key is required."
            });
        }

        var setting = await _service.GetByKeyAsync(key);

        if (setting == null)
        {
            return NotFound(new
            {
                message = $"Application setting '{key}' not found."
            });
        }

        return Ok(setting);
    }

    // =====================================================
    // GET BY CATEGORY
    // GET: api/application-settings/category/{category}
    // =====================================================

    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetByCategory(string category)
    {
        if (string.IsNullOrWhiteSpace(category))
        {
            return BadRequest(new
            {
                message = "Category is required."
            });
        }

        var settings = await _service.GetByCategoryAsync(category);

        return Ok(settings);
    }

    // =====================================================
    // CREATE
    // POST: api/application-settings
    // =====================================================

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateApplicationSettingDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var setting = await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = setting.Id },
                setting);
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
    }

    // =====================================================
    // UPDATE
    // PUT: api/application-settings/{id}
    // =====================================================

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateApplicationSettingDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var setting = await _service.UpdateAsync(id, dto);

            if (setting == null)
            {
                return NotFound(new
                {
                    message = "Application setting not found."
                });
            }

            // Get logged-in user's ID
            var userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            // NOTE:
            // UpdatedBy should ideally be assigned inside the service
            // using the authenticated user ID.
            //
            // The current service interface does not accept userId,
            // so this value will be integrated in the next refinement.

            return Ok(setting);
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
    }

    // =====================================================
    // DELETE
    // DELETE: api/application-settings/{id}
    // =====================================================

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message = "Application setting not found."
            });
        }

        return Ok(new
        {
            message = "Application setting deleted successfully."
        });
    }
}

