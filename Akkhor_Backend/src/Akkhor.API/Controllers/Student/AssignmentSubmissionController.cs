using Akkhor.Application.DTOs.Assignments;
using Akkhor.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Akkhor.API.Controllers;

[ApiController]
[Route("api/assignment-submissions")]
[Authorize]
public class AssignmentSubmissionController : ControllerBase
{
    private readonly IAssignmentSubmissionService _service;
    private readonly IWebHostEnvironment _environment;

    public AssignmentSubmissionController(
        IAssignmentSubmissionService service,
        IWebHostEnvironment environment)
    {
        _service = service;
        _environment = environment;
    }


    // =====================================================
    // GET ALL SUBMISSIONS
    // =====================================================
    // Admin / Teacher
    //
    // GET:
    // api/assignment-submissions
    // =====================================================

    [HttpGet]
    [Authorize(Roles = "Admin,SuperAdmin,Teacher")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var submissions =
                await _service.GetAllAsync();

            return Ok(submissions);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message = "Failed to load submissions.",
                    error = ex.Message
                });
        }
    }


    // =====================================================
    // GET SUBMISSION BY ID
    // =====================================================
    //
    // GET:
    // api/assignment-submissions/{id}
    // =====================================================

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var submission =
                await _service.GetByIdAsync(id);

            if (submission == null)
            {
                return NotFound(new
                {
                    message = "Submission not found."
                });
            }

            return Ok(submission);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message = "Failed to load submission.",
                    error = ex.Message
                });
        }
    }


    // =====================================================
    // GET SUBMISSIONS BY ASSIGNMENT
    // =====================================================
    // Teacher
    //
    // GET:
    // api/assignment-submissions/assignment/{assignmentId}
    // =====================================================

    [HttpGet("assignment/{assignmentId:guid}")]
    [Authorize(Roles = "Teacher,Admin,SuperAdmin")]
    public async Task<IActionResult> GetByAssignment(
        Guid assignmentId)
    {
        try
        {
            var submissions =
                await _service.GetByAssignmentAsync(
                    assignmentId);

            return Ok(submissions);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to load assignment submissions.",
                    error = ex.Message
                });
        }
    }


    // =====================================================
    // GET SUBMISSION BY ASSIGNMENT + STUDENT
    // =====================================================
    //
    // GET:
    // api/assignment-submissions/assignment/{assignmentId}/student/{studentId}
    // =====================================================

    [HttpGet(
        "assignment/{assignmentId:guid}/student/{studentId}")]
    [Authorize(Roles = "Teacher,Admin,SuperAdmin")]
    public async Task<IActionResult>
        GetByAssignmentAndStudent(
            Guid assignmentId,
            string studentId)
    {
        try
        {
            var submission =
                await _service
                    .GetByAssignmentAndStudentAsync(
                        assignmentId,
                        studentId);

            if (submission == null)
            {
                return NotFound(new
                {
                    message = "Submission not found."
                });
            }

            return Ok(submission);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to load submission.",
                    error = ex.Message
                });
        }
    }


    // =====================================================
    // GET MY SUBMISSIONS
    // =====================================================
    // Student
    //
    // GET:
    // api/assignment-submissions/my
    // =====================================================

    [HttpGet("my")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetMySubmissions()
    {
        try
        {
            var studentId =
                GetCurrentUserId();

            if (string.IsNullOrWhiteSpace(studentId))
            {
                return Unauthorized(new
                {
                    message = "Student ID not found."
                });
            }

            var submissions =
                await _service
                    .GetMySubmissionsAsync(
                        studentId);

            return Ok(submissions);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to load your submissions.",
                    error = ex.Message
                });
        }
    }


    // =====================================================
    // GET MY SUBMISSION BY ASSIGNMENT
    // =====================================================
    // Student
    //
    // GET:
    // api/assignment-submissions/my/assignment/{assignmentId}
    // =====================================================

    [HttpGet("my/assignment/{assignmentId:guid}")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult>
        GetMySubmissionByAssignment(
            Guid assignmentId)
    {
        try
        {
            var studentId =
                GetCurrentUserId();

            if (string.IsNullOrWhiteSpace(studentId))
            {
                return Unauthorized(new
                {
                    message = "Student ID not found."
                });
            }

            var submission =
                await _service
                    .GetByAssignmentAndStudentAsync(
                        assignmentId,
                        studentId);

            if (submission == null)
            {
                return NotFound(new
                {
                    message =
                        "You have not submitted this assignment."
                });
            }

            return Ok(submission);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to load your submission.",
                    error = ex.Message
                });
        }
    }


    // =====================================================
    // CREATE / SUBMIT ASSIGNMENT
    // =====================================================
    // Student
    //
    // POST:
    // api/assignment-submissions
    //
    // Content-Type:
    // multipart/form-data
    // =====================================================

    [HttpPost]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Create(
        [FromForm] CreateAssignmentSubmissionDto dto,
        IFormFile? Attachment)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentId =
                GetCurrentUserId();

            if (string.IsNullOrWhiteSpace(studentId))
            {
                return Unauthorized(new
                {
                    message = "Student ID not found."
                });
            }


            // =================================================
            // VALIDATE ASSIGNMENT ID
            // =================================================

            if (dto.AssignmentId == Guid.Empty)
            {
                return BadRequest(new
                {
                    message = "Assignment ID is required."
                });
            }


            // =================================================
            // SAVE ATTACHMENT
            // =================================================

            if (Attachment != null &&
                Attachment.Length > 0)
            {
                var uploadFolder =
                    Path.Combine(
                        _environment.WebRootPath,
                        "uploads",
                        "submissions");


                // Create folder
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(
                        uploadFolder);
                }


                // Generate unique filename
                var extension =
                    Path.GetExtension(
                        Attachment.FileName);

                var fileName =
                    $"{Guid.NewGuid()}{extension}";


                var filePath =
                    Path.Combine(
                        uploadFolder,
                        fileName);


                // Save file
                await using (
                    var stream =
                        new FileStream(
                            filePath,
                            FileMode.Create))
                {
                    await Attachment.CopyToAsync(
                        stream);
                }


                // Store metadata
                dto.AttachmentUrl =
                    $"/uploads/submissions/{fileName}";

                dto.AttachmentFileName =
                    Attachment.FileName;

                dto.AttachmentContentType =
                    Attachment.ContentType;

                dto.AttachmentFileSize =
                    Attachment.Length;
            }


            // =================================================
            // CREATE SUBMISSION
            // =================================================

            var submission =
                await _service.CreateAsync(
                    dto,
                    studentId);


            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    id = submission.Id
                },
                submission);
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
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to submit assignment.",
                    error = ex.Message
                });
        }
    }


    // =====================================================
    // UPDATE MY SUBMISSION
    // =====================================================
    // Student
    //
    // PUT:
    // api/assignment-submissions/{id}
    //
    // Content-Type:
    // multipart/form-data
    // =====================================================

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromForm] UpdateAssignmentSubmissionDto dto,
        IFormFile? Attachment)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentId =
                GetCurrentUserId();

            if (string.IsNullOrWhiteSpace(studentId))
            {
                return Unauthorized(new
                {
                    message = "Student ID not found."
                });
            }


            // =================================================
            // SAVE NEW ATTACHMENT
            // =================================================

            if (Attachment != null &&
                Attachment.Length > 0)
            {
                var uploadFolder =
                    Path.Combine(
                        _environment.WebRootPath,
                        "uploads",
                        "submissions");


                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(
                        uploadFolder);
                }


                var extension =
                    Path.GetExtension(
                        Attachment.FileName);

                var fileName =
                    $"{Guid.NewGuid()}{extension}";

                var filePath =
                    Path.Combine(
                        uploadFolder,
                        fileName);


                await using (
                    var stream =
                        new FileStream(
                            filePath,
                            FileMode.Create))
                {
                    await Attachment.CopyToAsync(
                        stream);
                }


                dto.AttachmentUrl =
                    $"/uploads/submissions/{fileName}";

                dto.AttachmentFileName =
                    Attachment.FileName;

                dto.AttachmentContentType =
                    Attachment.ContentType;

                dto.AttachmentFileSize =
                    Attachment.Length;
            }


            // =================================================
            // UPDATE
            // =================================================

            var submission =
                await _service.UpdateAsync(
                    id,
                    dto,
                    studentId);


            if (submission == null)
            {
                return NotFound(new
                {
                    message =
                        "Submission not found."
                });
            }


            return Ok(new
            {
                message =
                    "Submission updated successfully.",
                submission
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(
                StatusCodes.Status403Forbidden,
                new
                {
                    message = ex.Message
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
            return Conflict(new
            {
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to update submission.",
                    error = ex.Message
                });
        }
    }


    // =====================================================
    // DELETE MY SUBMISSION
    // =====================================================
    // Student
    //
    // DELETE:
    // api/assignment-submissions/{id}
    // =====================================================

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Delete(
        Guid id)
    {
        try
        {
            var studentId =
                GetCurrentUserId();

            if (string.IsNullOrWhiteSpace(studentId))
            {
                return Unauthorized(new
                {
                    message = "Student ID not found."
                });
            }


            var deleted =
                await _service.DeleteAsync(
                    id,
                    studentId);


            if (!deleted)
            {
                return NotFound(new
                {
                    message =
                        "Submission not found."
                });
            }


            return Ok(new
            {
                message =
                    "Submission deleted successfully."
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(
                StatusCodes.Status403Forbidden,
                new
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
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to delete submission.",
                    error = ex.Message
                });
        }
    }


    // =====================================================
    // EVALUATE / GRADE SUBMISSION
    // =====================================================
    // Teacher
    //
    // PUT:
    // api/assignment-submissions/{id}/evaluate
    // =====================================================

    [HttpPut("{id:guid}/evaluate")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Evaluate(
        Guid id,
        [FromBody] EvaluateAssignmentSubmissionDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var teacherId =
                GetCurrentUserId();

            if (string.IsNullOrWhiteSpace(teacherId))
            {
                return Unauthorized(new
                {
                    message = "Teacher ID not found."
                });
            }


            var submission =
                await _service.EvaluateAsync(
                    id,
                    dto,
                    teacherId);


            if (submission == null)
            {
                return NotFound(new
                {
                    message =
                        "Submission not found."
                });
            }


            return Ok(new
            {
                message =
                    "Submission evaluated successfully.",
                submission
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
            return Conflict(new
            {
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to evaluate submission.",
                    error = ex.Message
                });
        }
    }


    // =====================================================
    // GET SUBMISSION COUNT
    // =====================================================
    // Teacher
    //
    // GET:
    // api/assignment-submissions/assignment/{assignmentId}/count
    // =====================================================

    [HttpGet(
        "assignment/{assignmentId:guid}/count")]
    [Authorize(Roles = "Teacher,Admin,SuperAdmin")]
    public async Task<IActionResult> GetSubmissionCount(
        Guid assignmentId)
    {
        try
        {
            var count =
                await _service
                    .GetSubmissionCountAsync(
                        assignmentId);

            return Ok(new
            {
                assignmentId,
                count
            });
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to get submission count.",
                    error = ex.Message
                });
        }
    }


    // =====================================================
    // GET PENDING SUBMISSION COUNT
    // =====================================================
    // Teacher
    //
    // GET:
    // api/assignment-submissions/assignment/{assignmentId}/pending-count
    // =====================================================

    [HttpGet(
        "assignment/{assignmentId:guid}/pending-count")]
    [Authorize(Roles = "Teacher,Admin,SuperAdmin")]
    public async Task<IActionResult>
        GetPendingSubmissionCount(
            Guid assignmentId)
    {
        try
        {
            var count =
                await _service
                    .GetPendingSubmissionCountAsync(
                        assignmentId);

            return Ok(new
            {
                assignmentId,
                count
            });
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to get pending submission count.",
                    error = ex.Message
                });
        }
    }


    // =====================================================
    // DOWNLOAD SUBMISSION ATTACHMENT
    // =====================================================
    //
    // GET:
    // api/assignment-submissions/{id}/attachment
    // =====================================================

    [HttpGet("{id:guid}/attachment")]
    [Authorize]
    public async Task<IActionResult>
        DownloadAttachment(Guid id)
    {
        try
        {
            var submission =
                await _service.GetByIdAsync(id);

            if (submission == null)
            {
                return NotFound(new
                {
                    message =
                        "Submission not found."
                });
            }


            if (string.IsNullOrWhiteSpace(
                submission.AttachmentUrl))
            {
                return NotFound(new
                {
                    message =
                        "No attachment found."
                });
            }


            var relativePath =
                submission.AttachmentUrl
                    .TrimStart('/')
                    .Replace(
                        '/',
                        Path.DirectorySeparatorChar);


            var filePath =
                Path.Combine(
                    _environment.WebRootPath,
                    relativePath);


            if (!System.IO.File.Exists(
                filePath))
            {
                return NotFound(new
                {
                    message =
                        "Submission attachment file not found."
                });
            }


            var contentType =
                submission.AttachmentContentType
                ?? "application/octet-stream";


            return PhysicalFile(
                filePath,
                contentType,
                submission.AttachmentFileName);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message =
                        "Failed to download attachment.",
                    error = ex.Message
                });
        }
    }


    // =====================================================
    // CURRENT USER ID
    // =====================================================

    private string? GetCurrentUserId()
    {
        return User.FindFirstValue(
            ClaimTypes.NameIdentifier);
    }
}