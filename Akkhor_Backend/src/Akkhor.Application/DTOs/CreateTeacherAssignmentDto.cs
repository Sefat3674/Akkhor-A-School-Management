using System;

namespace Akkhor.Application.DTOs.TeacherAssignments;

public class CreateTeacherAssignmentDto
{
    // Teacher
    public string TeacherId { get; set; } = string.Empty;

    // Academic Year
    public Guid AcademicYearId { get; set; }

    // Class
    public Guid ClassId { get; set; }

    // Section
    public Guid? SectionId { get; set; }

    // Course
    public Guid CourseId { get; set; }

    // Subject
    public Guid SubjectId { get; set; }

    // Assignment Settings
    public bool IsPrimary { get; set; } = true;

    public bool IsActive { get; set; } = true;

    // Audit
    public string? CreatedBy { get; set; }
}