using System;

namespace Akkhor.Application.DTOs.Assignments;

public class AssignmentDto
{
    public Guid Id { get; set; }

    // Teacher
    public string TeacherId { get; set; } = string.Empty;
    public string? TeacherName { get; set; }


    // Academic Information
    public Guid AcademicYearId { get; set; }
    public string? AcademicYearName { get; set; }

    public Guid ClassId { get; set; }
    public string? ClassName { get; set; }

    public Guid? SectionId { get; set; }
    public string? SectionName { get; set; }

    public Guid CourseId { get; set; }
    public string? CourseName { get; set; }

    public Guid SubjectId { get; set; }
    public string? SubjectName { get; set; }


    // Assignment Information
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime Deadline { get; set; }

    public decimal MaximumMarks { get; set; }


    // Attachment
    public string? AttachmentUrl { get; set; }

    public string? AttachmentFileName { get; set; }

    public string? AttachmentContentType { get; set; }

    public long? AttachmentFileSize { get; set; }


    // Publication
    public bool IsPublished { get; set; }

    public DateTime? PublishedAt { get; set; }


    // Status
    public bool IsActive { get; set; }


    // Audit
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }


    // Submission information
    public int SubmissionCount { get; set; }
}