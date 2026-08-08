using System;

namespace Akkhor.Domain.Entities;

public class AssignmentSubmission
{
    public Guid Id { get; set; }


    // =====================================================
    // FOREIGN KEYS
    // =====================================================

    public Guid AssignmentId { get; set; }

    public string StudentId { get; set; } = string.Empty;


    // =====================================================
    // STUDENT SUBMISSION
    // =====================================================

    public string? SubmissionText { get; set; }

    public string? FileUrl { get; set; }

    public string? FileName { get; set; }

    public string? ContentType { get; set; }

    public long? FileSize { get; set; }

    public DateTime? SubmittedAt { get; set; }


    // =====================================================
    // MARKS & FEEDBACK
    // =====================================================

    public decimal? Marks { get; set; }

    public string? Feedback { get; set; }

    public DateTime? EvaluatedAt { get; set; }

    public string? EvaluatedBy { get; set; }
    // =====================================================
    // SUBMISSION STATUS
    // =====================================================

    public string Status { get; set; } = "Pending";
    

    // =====================================================
    // AUDIT
    // =====================================================

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }


    // =====================================================
    // NAVIGATION PROPERTIES
    // =====================================================

    public virtual Assignment Assignment { get; set; } = null!;

    public virtual Users Student { get; set; } = null!;
}