using Akkhor.Application.DTOs.Classes;
using Akkhor.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Akkhor.API.Controllers.Academic;

[ApiController]
[Route("api/classes")]
public class ClassController : ControllerBase
{
    private readonly IClassService _service;


    public ClassController(IClassService service)
    {
        _service = service;
    }



    // GET: api/classes
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();

        return Ok(result);
    }



    // GET: api/classes/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);


        if (result == null)
        {
            return NotFound(new
            {
                message = "Class not found"
            });
        }


        return Ok(result);
    }



    // POST: api/classes
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateClassDto dto)
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




    // PUT: api/classes/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateClassDto dto)
    {

        var updated = await _service.UpdateAsync(id, dto);


        if (!updated)
        {
            return NotFound(new
            {
                message = "Class not found"
            });
        }


        return Ok(new
        {
            message = "Class updated successfully"
        });
    }




    // DELETE: api/classes/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {

        var deleted = await _service.DeleteAsync(id);


        if (!deleted)
        {
            return NotFound(new
            {
                message = "Class not found"
            });
        }


        return Ok(new
        {
            message = "Class deleted successfully"
        });
    }
}