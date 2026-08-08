using System;

namespace Akkhor.Domain.Entities;

public class TeacherAssignment
{
    public Guid Id { get; set; }

    // =====================================================
    // Foreign Keys
    // =====================================================

    public string TeacherId { get; set; } = string.Empty;

    public Guid AcademicYearId { get; set; }

    public Guid ClassId { get; set; }

    public Guid? SectionId { get; set; }

    public Guid CourseId { get; set; }

    public Guid SubjectId { get; set; }


    // =====================================================
    // Assignment Settings
    // =====================================================

    public bool IsPrimary { get; set; } = true;

    public bool IsActive { get; set; } = true;


    // =====================================================
    // Audit Fields
    // =====================================================

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }


    // =====================================================
    // Navigation Properties
    // =====================================================

    public virtual Users Teacher { get; set; } = null!;

    public virtual AcademicYear AcademicYear { get; set; } = null!;

    public virtual Class Class { get; set; } = null!;

    public virtual ClassSection? Section { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;
}