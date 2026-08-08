using Microsoft.AspNetCore.Http;

namespace Akkhor.Application.DTOs.Assignments;

public class UpdateAssignmentDto
{
    // =====================================================
    // ACADEMIC INFORMATION
    // =====================================================

    public Guid AcademicYearId { get; set; }

    public Guid ClassId { get; set; }

    public Guid? SectionId { get; set; }

    public Guid CourseId { get; set; }

    public Guid SubjectId { get; set; }


    // =====================================================
    // ASSIGNMENT INFORMATION
    // =====================================================

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime Deadline { get; set; }

    public decimal MaximumMarks { get; set; }


    // =====================================================
    // PUBLICATION
    // =====================================================

    public bool IsPublished { get; set; }


    // =====================================================
    // ACTIVE STATUS
    // =====================================================

    public bool IsActive { get; set; }


    // =====================================================
    // ATTACHMENT
    // =====================================================

    public IFormFile? Attachment { get; set; }
    public string? AttachmentUrl { get; set; }

    public string? AttachmentFileName { get; set; }

    public string? AttachmentContentType { get; set; }

    public long? AttachmentFileSize { get; set; }
}