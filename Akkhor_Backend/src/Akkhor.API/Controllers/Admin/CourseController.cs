using Akkhor.Application.DTOs.Courses;
using Akkhor.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Akkhor.API.Controllers.Admin;

[ApiController]
[Route("api/courses")]
public class CourseController : ControllerBase
{
    private readonly ICourseService _service;


    public CourseController(ICourseService service)
    {
        _service = service;
    }





    // GET: api/courses
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();

        return Ok(result);
    }





    // GET: api/courses/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);


        if (result == null)
            return NotFound();


        return Ok(result);
    }





    // POST: api/courses
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateCourseDto dto)
    {
        try
        {
            var result = await _service.CreateAsync(dto);


            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
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





    // PUT: api/courses/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateCourseDto dto)
    {
        var result = await _service
            .UpdateAsync(id, dto);



        if (!result)
            return NotFound();



        return NoContent();
    }





    // DELETE: api/courses/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service
            .DeleteAsync(id);



        if (!result)
            return NotFound();



        return NoContent();
    }
}