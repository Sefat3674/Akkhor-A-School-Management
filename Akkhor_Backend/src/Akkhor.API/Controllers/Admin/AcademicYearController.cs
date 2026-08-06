using Akkhor.Application.DTOs.AcademicYear;
using Akkhor.Application.Interfaces.Repositories;
using Akkhor.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Akkhor.API.Controllers;

[ApiController]
[Route("api/academic-years")]
public class AcademicYearController : ControllerBase
{
    private readonly IAcademicYearRepository _repository;


    public AcademicYearController(
        IAcademicYearRepository repository)
    {
        _repository = repository;
    }



    // GET: api/academic-years
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _repository.GetAllAsync();


        var result = data.Select(x => new AcademicYearDto
        {
            Id = x.Id,

            Name = x.Name,

            StartDate = x.StartDate,

            EndDate = x.EndDate,

            IsActive = x.IsActive,

            CreatedAt = x.CreatedAt
        });


        return Ok(result);
    }




    // GET: api/academic-years/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var academicYear =
            await _repository.GetByIdAsync(id);


        if (academicYear == null)
        {
            return NotFound(
                "Academic year not found");
        }



        return Ok(new AcademicYearDto
        {
            Id = academicYear.Id,

            Name = academicYear.Name,

            StartDate = academicYear.StartDate,

            EndDate = academicYear.EndDate,

            IsActive = academicYear.IsActive,

            CreatedAt = academicYear.CreatedAt
        });
    }




    // POST: api/academic-years
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateAcademicYearDto dto)
    {

        if (await _repository.ExistsAsync(dto.Name))
        {
            return BadRequest(
                "Academic year already exists");
        }



        if (dto.EndDate <= dto.StartDate)
        {
            return BadRequest(
                "End date must be greater than start date");
        }



        var entity = new AcademicYear
        {
            Id = Guid.NewGuid(),

            Name = dto.Name,

            StartDate = dto.StartDate,

            EndDate = dto.EndDate,

            IsActive = dto.IsActive,

            CreatedAt = DateTime.UtcNow
        };



        await _repository.AddAsync(entity);

        await _repository.SaveChangesAsync();



        return Ok(new
        {
            message = "Academic year created successfully",

            id = entity.Id
        });
    }





    // PUT: api/academic-years/{id}
    // PUT: api/academic-years/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateAcademicYearDto dto)
    {
        var entity = await _repository.GetByIdAsync(id);


        if (entity == null)
        {
            return NotFound("Academic year not found");
        }


        if (dto.EndDate <= dto.StartDate)
        {
            return BadRequest(
                "End date must be greater than start date");
        }


        entity.Name = dto.Name;

        entity.StartDate = dto.StartDate;

        entity.EndDate = dto.EndDate;

        entity.IsActive = dto.IsActive;


        entity.UpdatedAt = DateTime.UtcNow;


        await _repository.UpdateAsync(entity);

        await _repository.SaveChangesAsync();


        return Ok(new
        {
            message = "Academic year updated successfully"
        });
    }





    // DELETE: api/academic-years/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        Guid id)
    {

        var entity =
            await _repository.GetByIdAsync(id);



        if (entity == null)
        {
            return NotFound(
                "Academic year not found");
        }



        await _repository.DeleteAsync(entity);

        await _repository.SaveChangesAsync();



        return Ok(new
        {
            message = "Academic year deleted successfully"
        });
    }
}