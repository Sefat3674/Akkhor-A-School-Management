using System;

namespace Akkhor.Application.DTOs.TeacherAssignments;

public class TeacherAssignmentDto
{
    public Guid Id { get; set; }

    // Teacher
    public string TeacherId { get; set; } = string.Empty;
    public string? TeacherName { get; set; }
    public string? TeacherEmail { get; set; }

    // Academic Year
    public Guid AcademicYearId { get; set; }
    public string? AcademicYearName { get; set; }

    // Class
    public Guid ClassId { get; set; }
    public string? ClassName { get; set; }

    // Section
    public Guid? SectionId { get; set; }
    public string? SectionName { get; set; }

    // Course
    public Guid CourseId { get; set; }
    public string? CourseName { get; set; }

    // Subject
    public Guid SubjectId { get; set; }
    public string? SubjectName { get; set; }

    // Assignment
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }

    // Audit
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
}