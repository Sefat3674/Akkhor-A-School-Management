using Akkhor.Application.DTOs.Assignments;
using Akkhor.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Akkhor.API.Controllers.Teacher;

[ApiController]
[Route("api/assignments")]
[Authorize(Roles = "Teacher")]
public class AssignmentController : ControllerBase
{
    private readonly IAssignmentService _service;
    private readonly IWebHostEnvironment _environment;

    public AssignmentController(IAssignmentService service, IWebHostEnvironment environment)
    {
        _service = service;
        _environment = environment;
    }


    // =====================================================
    // GET ALL ASSIGNMENTS
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
                500,
                new
                {
                    message = "Failed to load assignments.",
                    error = ex.Message
                });
        }
    }


    // =====================================================
    // GET ASSIGNMENT BY ID
    // =====================================================

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var assignment = await _service.GetByIdAsync(id);

            if (assignment == null)
            {
                return NotFound(new
                {
                    message = "Assignment not found."
                });
            }

            return Ok(assignment);
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new
                {
                    message = "Failed to load assignment.",
                    error = ex.Message
                });
        }
    }


    // =====================================================
    // GET MY ASSIGNMENTS
    // =====================================================

    [HttpGet("my")]
    public async Task<IActionResult> GetMyAssignments()
    {
        try
        {
            var teacherId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(teacherId))
            {
                return Unauthorized(new
                {
                    message = "Teacher identity not found."
                });
            }

            var assignments =
                await _service.GetByTeacherAsync(teacherId);

            return Ok(assignments);
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new
                {
                    message = "Failed to load your assignments.",
                    error = ex.Message
                });
        }
    }


    // =====================================================
    // CREATE ASSIGNMENT
    // =====================================================

    // =====================================================
    // CREATE ASSIGNMENT
    // =====================================================

    [HttpPost]
    public async Task<IActionResult> Create(
     [FromForm] CreateAssignmentDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var teacherId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(teacherId))
            {
                return Unauthorized(new
                {
                    message = "Teacher identity not found."
                });
            }

            // =====================================================
            // SAVE ATTACHMENT
            // =====================================================

            if (dto.Attachment != null &&
                dto.Attachment.Length > 0)
            {
                var uploadFolder = Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    "assignments"
                );

                // Create folder if it doesn't exist
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                // Generate unique file name
                var extension =
                    Path.GetExtension(dto.Attachment.FileName);

                var fileName =
                    $"{Guid.NewGuid()}{extension}";

                var filePath =
                    Path.Combine(
                        uploadFolder,
                        fileName);

                // Save physical file
                using (var stream =
                       new FileStream(
                           filePath,
                           FileMode.Create))
                {
                    await dto.Attachment.CopyToAsync(stream);
                }

                // Store URL
                dto.AttachmentUrl =
                    $"/uploads/assignments/{fileName}";

                // Store metadata
                dto.AttachmentFileName =
                    dto.Attachment.FileName;

                dto.AttachmentContentType =
                    dto.Attachment.ContentType;

                dto.AttachmentFileSize =
                    dto.Attachment.Length;
            }

            // =====================================================
            // CREATE ASSIGNMENT
            // =====================================================

            var assignment =
                await _service.CreateAsync(
                    dto,
                    teacherId);

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
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new
                {
                    message = "Failed to create assignment.",
                    error = ex.Message
                });
        }
    }


    // =====================================================
    // UPDATE ASSIGNMENT
    // =====================================================

    // =====================================================
    // UPDATE ASSIGNMENT
    // =====================================================

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromForm] UpdateAssignmentDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var teacherId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(teacherId))
            {
                return Unauthorized(new
                {
                    message = "Teacher identity not found."
                });
            }


            // =====================================================
            // SAVE NEW ATTACHMENT
            // =====================================================

            if (dto.Attachment != null &&
                dto.Attachment.Length > 0)
            {
                var uploadFolder =
                    Path.Combine(
                        _environment.WebRootPath,
                        "uploads",
                        "assignments"
                    );


                // Create folder
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(
                        uploadFolder);
                }


                // Get extension
                var extension =
                    Path.GetExtension(
                        dto.Attachment.FileName);


                // Generate unique filename
                var fileName =
                    $"{Guid.NewGuid()}{extension}";


                var filePath =
                    Path.Combine(
                        uploadFolder,
                        fileName);


                // Save physical file
                await using (
                    var stream =
                        new FileStream(
                            filePath,
                            FileMode.Create))
                {
                    await dto.Attachment
                        .CopyToAsync(stream);
                }


                // =================================================
                // STORE FILE INFORMATION
                // =================================================

                dto.AttachmentUrl =
                    $"/uploads/assignments/{fileName}";

                dto.AttachmentFileName =
                    dto.Attachment.FileName;

                dto.AttachmentContentType =
                    dto.Attachment.ContentType;

                dto.AttachmentFileSize =
                    dto.Attachment.Length;
            }


            // =====================================================
            // UPDATE ASSIGNMENT
            // =====================================================

            var assignment =
                await _service.UpdateAsync(
                    id,
                    dto,
                    teacherId);


            if (assignment == null)
            {
                return NotFound(new
                {
                    message = "Assignment not found."
                });
            }


            return Ok(new
            {
                message =
                    "Assignment updated successfully.",

                assignment
            });
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
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new
                {
                    message =
                        "Failed to update assignment.",

                    error = ex.Message
                });
        }
    }


    // =====================================================
    // DELETE ASSIGNMENT
    // =====================================================

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var teacherId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(teacherId))
            {
                return Unauthorized(new
                {
                    message = "Teacher identity not found."
                });
            }


            var result =
                await _service.DeleteAsync(
                    id,
                    teacherId);


            if (!result)
            {
                return NotFound(new
                {
                    message = "Assignment not found."
                });
            }


            return Ok(new
            {
                message = "Assignment deleted successfully."
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new
                {
                    message = "Failed to delete assignment.",
                    error = ex.Message
                });
        }
    }


    // =====================================================
    // PUBLISH ASSIGNMENT
    // =====================================================

    [HttpPatch("{id:guid}/publish")]
    public async Task<IActionResult> Publish(Guid id)
    {
        try
        {
            var teacherId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(teacherId))
            {
                return Unauthorized(new
                {
                    message = "Teacher identity not found."
                });
            }


            var assignment =
                await _service.PublishAsync(
                    id,
                    teacherId);


            if (assignment == null)
            {
                return NotFound(new
                {
                    message = "Assignment not found."
                });
            }


            return Ok(new
            {
                message = "Assignment published successfully.",
                assignment
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new
                {
                    message = "Failed to publish assignment.",
                    error = ex.Message
                });
        }
    }


    // =====================================================
    // UNPUBLISH ASSIGNMENT
    // =====================================================

    [HttpPatch("{id:guid}/unpublish")]
    public async Task<IActionResult> Unpublish(Guid id)
    {
        try
        {
            var teacherId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(teacherId))
            {
                return Unauthorized(new
                {
                    message = "Teacher identity not found."
                });
            }


            var assignment =
                await _service.UnpublishAsync(
                    id,
                    teacherId);


            if (assignment == null)
            {
                return NotFound(new
                {
                    message = "Assignment not found."
                });
            }


            return Ok(new
            {
                message = "Assignment moved to draft.",
                assignment
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new
                {
                    message = "Failed to unpublish assignment.",
                    error = ex.Message
                });
        }
    }


    // =====================================================
    // GET BY CLASS
    // =====================================================

    [HttpGet("class/{classId:guid}")]
    public async Task<IActionResult> GetByClass(Guid classId)
    {
        try
        {
            var assignments =
                await _service.GetByClassAsync(classId);

            return Ok(assignments);
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new
                {
                    message = "Failed to load class assignments.",
                    error = ex.Message
                });
        }
    }


    // =====================================================
    // GET BY COURSE
    // =====================================================

    [HttpGet("course/{courseId:guid}")]
    public async Task<IActionResult> GetByCourse(Guid courseId)
    {
        try
        {
            var assignments =
                await _service.GetByCourseAsync(courseId);

            return Ok(assignments);
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new
                {
                    message = "Failed to load course assignments.",
                    error = ex.Message
                });
        }
    }


    // =====================================================
    // GET BY SUBJECT
    // =====================================================

    [HttpGet("subject/{subjectId:guid}")]
    public async Task<IActionResult> GetBySubject(Guid subjectId)
    {
        try
        {
            var assignments =
                await _service.GetBySubjectAsync(subjectId);

            return Ok(assignments);
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new
                {
                    message = "Failed to load subject assignments.",
                    error = ex.Message
                });
        }
    }



    // =====================================================
    // DOWNLOAD ASSIGNMENT ATTACHMENT
    // =====================================================

    // =====================================================
    // DOWNLOAD ASSIGNMENT ATTACHMENT
    // =====================================================

    [HttpGet("{id:guid}/attachment")]
    public async Task<IActionResult> DownloadAttachment(Guid id)
    {
        try
        {
            // =================================================
            // GET ASSIGNMENT
            // =================================================

            var assignment =
                await _service.GetByIdAsync(id);

            if (assignment == null)
            {
                return NotFound(new
                {
                    message = "Assignment not found."
                });
            }


            // =================================================
            // CHECK ATTACHMENT
            // =================================================

            if (string.IsNullOrWhiteSpace(
                assignment.AttachmentUrl))
            {
                return NotFound(new
                {
                    message = "No attachment found for this assignment."
                });
            }


            // =================================================
            // GET PHYSICAL FILE NAME FROM URL
            // =================================================

            var relativePath =
                assignment.AttachmentUrl
                    .TrimStart('/')
                    .Replace(
                        '/',
                        Path.DirectorySeparatorChar);


            // =================================================
            // BUILD FILE PATH
            // =================================================

            var filePath =
                Path.Combine(
                    _environment.WebRootPath,
                    relativePath
                );


            // =================================================
            // CHECK FILE EXISTS
            // =================================================

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new
                {
                    message = "Attachment file not found.",
                    filePath = relativePath
                });
            }


            // =================================================
            // CONTENT TYPE
            // =================================================

            var contentType =
                assignment.AttachmentContentType
                ?? "application/octet-stream";


            // =================================================
            // DOWNLOAD
            // =================================================

            return PhysicalFile(
                filePath,
                contentType,
                assignment.AttachmentFileName
            );
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new
                {
                    message =
                        "Failed to download attachment.",

                    error =
                        ex.Message
                });
        }
    }

}