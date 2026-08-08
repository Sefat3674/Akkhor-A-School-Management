using System;

namespace Akkhor.Application.DTOs.Assignments;

public class CreateAssignmentSubmissionDto
{
    // =====================================================
    // ASSIGNMENT
    // =====================================================

    public Guid AssignmentId { get; set; }


    // =====================================================
    // SUBMISSION
    // =====================================================

    public string? SubmissionText { get; set; }


    // =====================================================
    // ATTACHMENT
    // =====================================================

    public string? AttachmentUrl { get; set; }

    public string? AttachmentFileName { get; set; }

    public string? AttachmentContentType { get; set; }

    public long? AttachmentFileSize { get; set; }

}