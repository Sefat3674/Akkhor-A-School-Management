using Akkhor.Application.DTOs.TeacherAssignments;
using Akkhor.Application.Interfaces.Repositories;
using Akkhor.Application.Interfaces.Services;
using Akkhor.Domain.Entities;

namespace Akkhor.Application.Services;

public class TeacherAssignmentService : ITeacherAssignmentService
{
    private readonly ITeacherAssignmentRepository _repository;

    public TeacherAssignmentService(
        ITeacherAssignmentRepository repository)
    {
        _repository = repository;
    }


    // =====================================================
    // GET ALL
    // =====================================================

    public async Task<IEnumerable<TeacherAssignmentDto>> GetAllAsync()
    {
        var assignments = await _repository.GetAllAsync();

        return assignments.Select(MapToDto);
    }


    // =====================================================
    // GET BY ID
    // =====================================================

    public async Task<TeacherAssignmentDto?> GetByIdAsync(Guid id)
    {
        var assignment = await _repository.GetByIdAsync(id);

        if (assignment == null)
        {
            return null;
        }

        return MapToDto(assignment);
    }


    // =====================================================
    // CREATE
    // =====================================================

    public async Task<TeacherAssignmentDto> CreateAsync(
        CreateTeacherAssignmentDto dto)
    {
        // -------------------------------------------------
        // Validate Teacher
        // -------------------------------------------------

        if (string.IsNullOrWhiteSpace(dto.TeacherId))
        {
            throw new ArgumentException(
                "Teacher is required.");
        }


        // -------------------------------------------------
        // Validate Academic Year
        // -------------------------------------------------

        if (dto.AcademicYearId == Guid.Empty)
        {
            throw new ArgumentException(
                "Academic year is required.");
        }


        // -------------------------------------------------
        // Validate Class
        // -------------------------------------------------

        if (dto.ClassId == Guid.Empty)
        {
            throw new ArgumentException(
                "Class is required.");
        }


        // -------------------------------------------------
        // Validate Course
        // -------------------------------------------------

        if (dto.CourseId == Guid.Empty)
        {
            throw new ArgumentException(
                "Course is required.");
        }


        // -------------------------------------------------
        // Validate Subject
        // -------------------------------------------------

        if (dto.SubjectId == Guid.Empty)
        {
            throw new ArgumentException(
                "Subject is required.");
        }


        // -------------------------------------------------
        // Check Duplicate
        // -------------------------------------------------

        var exists = await _repository.ExistsAsync(
            dto.TeacherId,
            dto.AcademicYearId,
            dto.ClassId,
            dto.SectionId,
            dto.CourseId,
            dto.SubjectId
        );

        if (exists)
        {
            throw new InvalidOperationException(
                "This teacher is already assigned to the selected class, section, course and subject.");
        }


        // -------------------------------------------------
        // Create Entity
        // -------------------------------------------------

        var assignment = new TeacherAssignment
        {
            Id = Guid.NewGuid(),

            TeacherId = dto.TeacherId,

            AcademicYearId = dto.AcademicYearId,

            ClassId = dto.ClassId,

            SectionId = dto.SectionId,

            CourseId = dto.CourseId,

            SubjectId = dto.SubjectId,

            IsPrimary = dto.IsPrimary,

            IsActive = dto.IsActive,

            CreatedAt = DateTime.UtcNow,

            CreatedBy = dto.CreatedBy
        };


        // -------------------------------------------------
        // Save
        // -------------------------------------------------

        var created =
            await _repository.CreateAsync(assignment);

        return MapToDto(created);
    }


    // =====================================================
    // UPDATE
    // =====================================================

    public async Task<TeacherAssignmentDto?> UpdateAsync(
        Guid id,
        UpdateTeacherAssignmentDto dto)
    {
        // -------------------------------------------------
        // Find existing assignment
        // -------------------------------------------------

        var assignment =
            await _repository.GetByIdAsync(id);

        if (assignment == null)
        {
            return null;
        }


        // -------------------------------------------------
        // Validate Teacher
        // -------------------------------------------------

        if (string.IsNullOrWhiteSpace(dto.TeacherId))
        {
            throw new ArgumentException(
                "Teacher is required.");
        }


        // -------------------------------------------------
        // Validate Academic Year
        // -------------------------------------------------

        if (dto.AcademicYearId == Guid.Empty)
        {
            throw new ArgumentException(
                "Academic year is required.");
        }


        // -------------------------------------------------
        // Validate Class
        // -------------------------------------------------

        if (dto.ClassId == Guid.Empty)
        {
            throw new ArgumentException(
                "Class is required.");
        }


        // -------------------------------------------------
        // Validate Course
        // -------------------------------------------------

        if (dto.CourseId == Guid.Empty)
        {
            throw new ArgumentException(
                "Course is required.");
        }


        // -------------------------------------------------
        // Validate Subject
        // -------------------------------------------------

        if (dto.SubjectId == Guid.Empty)
        {
            throw new ArgumentException(
                "Subject is required.");
        }


        // -------------------------------------------------
        // Check Duplicate
        // -------------------------------------------------

        var exists = await _repository.ExistsAsync(
            dto.TeacherId,
            dto.AcademicYearId,
            dto.ClassId,
            dto.SectionId,
            dto.CourseId,
            dto.SubjectId,
            id
        );

        if (exists)
        {
            throw new InvalidOperationException(
                "This teacher is already assigned to the selected class, section, course and subject.");
        }


        // -------------------------------------------------
        // Update Entity
        // -------------------------------------------------

        assignment.TeacherId =
            dto.TeacherId;

        assignment.AcademicYearId =
            dto.AcademicYearId;

        assignment.ClassId =
            dto.ClassId;

        assignment.SectionId =
            dto.SectionId;

        assignment.CourseId =
            dto.CourseId;

        assignment.SubjectId =
            dto.SubjectId;

        assignment.IsPrimary =
            dto.IsPrimary;

        assignment.IsActive =
            dto.IsActive;

        assignment.UpdatedAt =
            DateTime.UtcNow;

        assignment.UpdatedBy =
            dto.UpdatedBy;


        // -------------------------------------------------
        // Save
        // -------------------------------------------------

        var updated =
            await _repository.UpdateAsync(assignment);

        return MapToDto(updated);
    }


    // =====================================================
    // DELETE
    // =====================================================

    public async Task<bool> DeleteAsync(Guid id)
    {
        var assignment =
            await _repository.GetByIdAsync(id);

        if (assignment == null)
        {
            return false;
        }

        return await _repository.DeleteAsync(id);
    }


    // =====================================================
    // ENTITY → DTO
    // =====================================================

    private static TeacherAssignmentDto MapToDto(
        TeacherAssignment entity)
    {
        return new TeacherAssignmentDto
        {
            Id = entity.Id,


            // -------------------------------------------------
            // Teacher
            // -------------------------------------------------

            TeacherId =
                entity.TeacherId,

            TeacherName =
                entity.Teacher?.FullName,

            TeacherEmail =
                entity.Teacher?.Email,


            // -------------------------------------------------
            // Academic Year
            // -------------------------------------------------

            AcademicYearId =
                entity.AcademicYearId,

            AcademicYearName =
                entity.AcademicYear?.Name,


            // -------------------------------------------------
            // Class
            // -------------------------------------------------

            ClassId =
                entity.ClassId,

            ClassName =
                entity.Class?.Name,


            // -------------------------------------------------
            // Section
            // -------------------------------------------------

            SectionId =
                entity.SectionId,

            SectionName =
                entity.Section?.SectionName,


            // -------------------------------------------------
            // Course
            // -------------------------------------------------

            CourseId =
                entity.CourseId,

            CourseName =
                entity.Course?.CourseName,


            // -------------------------------------------------
            // Subject
            // -------------------------------------------------

            SubjectId =
                entity.SubjectId,

            SubjectName =
                entity.Subject?.Name,


            // -------------------------------------------------
            // Assignment
            // -------------------------------------------------

            IsPrimary =
                entity.IsPrimary,

            IsActive =
                entity.IsActive,


            // -------------------------------------------------
            // Audit
            // -------------------------------------------------

            CreatedAt =
                entity.CreatedAt,

            CreatedBy =
                entity.CreatedBy,

            UpdatedAt =
                entity.UpdatedAt,

            UpdatedBy =
                entity.UpdatedBy
        };
    }
}