using Akkhor.Application.DTOs.StudentEnrollments;
using Akkhor.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Akkhor.API.Controllers.Admin;


[ApiController]
[Route("api/student-enrollments")]
[Authorize(Roles = "Admin,SuperAdmin")]
public class StudentEnrollmentController : ControllerBase
{
    private readonly IStudentEnrollmentService _service;


    public StudentEnrollmentController(
        IStudentEnrollmentService service)
    {
        _service = service;
    }



    // GET: api/student-enrollments
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _service.GetAllAsync();

        return Ok(data);
    }



    // GET: api/student-enrollments/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var data = await _service.GetByIdAsync(id);


        if (data == null)
            return NotFound();


        return Ok(data);
    }




    // POST: api/student-enrollments
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateStudentEnrollmentDto dto)
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





    // PUT: api/student-enrollments/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateStudentEnrollmentDto dto)
    {

        var result = await _service.UpdateAsync(
            id,
            dto);


        if (!result)
            return NotFound();


        return NoContent();
    }






    // DELETE: api/student-enrollments/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {

        var result = await _service.DeleteAsync(id);


        if (!result)
            return NotFound();


        return NoContent();
    }

    
            // =====================================================
            // GET STUDENTS
            // GET: api/student-enrollments/students
            // =====================================================

            [HttpGet("students")]
            public async Task<IActionResult> GetStudents()
                {
                    try
                    {
                        var students =
                            await _service.GetStudentsAsync();

                        return Ok(students);
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(
                            StatusCodes.Status500InternalServerError,
                            new
                            {
                                message = "Failed to load students.",
                                error = ex.Message
                            });
                    }
                }


                // =====================================================
                // GET CLASSES
                // GET: api/student-enrollments/classes
                // =====================================================

                [HttpGet("classes")]
                public async Task<IActionResult> GetClasses()
                {
                    try
                    {
                        var classes =
                            await _service.GetClassesAsync();

                        return Ok(classes);
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(
                            StatusCodes.Status500InternalServerError,
                            new
                            {
                                message = "Failed to load classes.",
                                error = ex.Message
                            });
                    }
                }


                // =====================================================
                // GET COURSES BY CLASS
                // GET: api/student-enrollments/classes/{classId}/courses
                // =====================================================

                [HttpGet("classes/{classId:guid}/courses")]
                public async Task<IActionResult> GetCoursesByClass(Guid classId)
                {
                    try
                    {
                        var courses =
                            await _service
                                .GetCoursesByClassIdAsync(classId);

                        return Ok(courses);
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(
                            StatusCodes.Status500InternalServerError,
                            new
                            {
                                message = "Failed to load courses.",
                                error = ex.Message
                            });
                    }
                }


                // =====================================================
                // GET SECTIONS BY CLASS
                // GET: api/student-enrollments/classes/{classId}/sections
                // =====================================================

                [HttpGet("classes/{classId:guid}/sections")]
                public async Task<IActionResult> GetSectionsByClass(Guid classId)
                {
                    try
                    {
                        var sections =
                            await _service
                                .GetSectionsByClassIdAsync(classId);

                        return Ok(sections);
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(
                            StatusCodes.Status500InternalServerError,
                            new
                            {
                                message = "Failed to load sections.",
                                error = ex.Message
                            });
                    }
                }


}