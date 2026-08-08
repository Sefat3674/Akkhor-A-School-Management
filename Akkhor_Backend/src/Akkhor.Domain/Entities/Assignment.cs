using System;

namespace Akkhor.Domain.Entities;

public class Assignment
{
    public Guid Id { get; set; }


    // =====================================================
    // FOREIGN KEYS
    // =====================================================

    public string TeacherId { get; set; } = string.Empty;

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
    // ATTACHMENT
    // =====================================================

    public string? AttachmentUrl { get; set; }

    public string? AttachmentFileName { get; set; }

    public string? AttachmentContentType { get; set; }

    public long? AttachmentFileSize { get; set; }


    // =====================================================
    // PUBLICATION
    // =====================================================

    public bool IsPublished { get; set; } = false;

    public DateTime? PublishedAt { get; set; }

    

    //public DateTime Deadline { get; set; }

    // =====================================================
    // STATUS
    // =====================================================

    public bool IsActive { get; set; } = true;


    // =====================================================
    // AUDIT
    // =====================================================

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }


    // =====================================================
    // NAVIGATION PROPERTIES
    // =====================================================

    public virtual Users Teacher { get; set; } = null!;

    public virtual AcademicYear AcademicYear { get; set; } = null!;

    public virtual Class Class { get; set; } = null!;

    public virtual ClassSection? Section { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;


    // =====================================================
    // SUBMISSIONS
    // =====================================================

    public virtual ICollection<AssignmentSubmission> Submissions { get; set; }
        = new List<AssignmentSubmission>();
}