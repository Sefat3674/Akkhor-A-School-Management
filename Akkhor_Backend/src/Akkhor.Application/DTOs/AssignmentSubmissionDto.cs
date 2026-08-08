using System;

namespace Akkhor.Application.DTOs.Assignments;

public class AssignmentSubmissionDto
{
    public Guid Id { get; set; }

    public Guid AssignmentId { get; set; }

    public string? AssignmentTitle { get; set; }


    // =====================================================
    // STUDENT
    // =====================================================

    public string StudentId { get; set; } = string.Empty;

    public string? StudentName { get; set; }


    // =====================================================
    // SUBMISSION
    // =====================================================

    public DateTime SubmittedAt { get; set; }

    public string? SubmissionText { get; set; }

    public string? AttachmentUrl { get; set; }

    public string? AttachmentFileName { get; set; }

    public string? AttachmentContentType { get; set; }

    public long? AttachmentFileSize { get; set; }


    // =====================================================
    // MARKS & FEEDBACK
    // =====================================================

    public decimal? MarksObtained { get; set; }

    public string? Feedback { get; set; }


    // =====================================================
    // STATUS
    // =====================================================

    public string Status { get; set; } = string.Empty;


    // =====================================================
    // AUDIT
    // =====================================================

    public DateTime? GradedAt { get; set; }

    public string? GradedBy { get; set; }
}