using Akkhor.Application.DTOs.Assignments;
using Akkhor.Application.Interfaces.Repositories;
using Akkhor.Application.Interfaces.Services;
using Akkhor.Domain.Entities;

namespace Akkhor.Application.Services;

public class AssignmentService : IAssignmentService
{
    private readonly IAssignmentRepository _repository;

    public AssignmentService(
        IAssignmentRepository repository)
    {
        _repository = repository;
    }


    // =====================================================
    // GET ALL
    // =====================================================

    public async Task<IEnumerable<AssignmentDto>> GetAllAsync()
    {
        var assignments =
            await _repository.GetAllAsync();

        return assignments.Select(MapToDto);
    }


    // =====================================================
    // GET BY ID
    // =====================================================

    public async Task<AssignmentDto?> GetByIdAsync(
        Guid id)
    {
        var assignment =
            await _repository.GetByIdAsync(id);

        if (assignment == null)
        {
            return null;
        }

        return MapToDto(assignment);
    }


    // =====================================================
    // GET MY ASSIGNMENTS
    // =====================================================

    public async Task<IEnumerable<AssignmentDto>> GetMyAssignmentsAsync(
        string teacherId)
    {
        if (string.IsNullOrWhiteSpace(teacherId))
        {
            throw new ArgumentException(
                "Teacher ID is required.");
        }

        var assignments =
            await _repository.GetByTeacherIdAsync(
                teacherId);

        return assignments.Select(MapToDto);
    }


    // =====================================================
    // GET MY ASSIGNMENT BY ID
    // =====================================================

    public async Task<AssignmentDto?> GetMyAssignmentByIdAsync(
        Guid id,
        string teacherId)
    {
        if (string.IsNullOrWhiteSpace(teacherId))
        {
            throw new ArgumentException(
                "Teacher ID is required.");
        }

        var assignment =
            await _repository.GetByIdForTeacherAsync(
                id,
                teacherId);

        if (assignment == null)
        {
            return null;
        }

        return MapToDto(assignment);
    }


    // =====================================================
    // GET BY CLASS
    // =====================================================

    public async Task<IEnumerable<AssignmentDto>> GetByClassAsync(
        Guid classId)
    {
        var assignments =
            await _repository.GetByClassIdAsync(
                classId);

        return assignments.Select(MapToDto);
    }


    // =====================================================
    // GET BY COURSE
    // =====================================================

    public async Task<IEnumerable<AssignmentDto>> GetByCourseAsync(
        Guid courseId)
    {
        var assignments =
            await _repository.GetByCourseIdAsync(
                courseId);

        return assignments.Select(MapToDto);
    }


    // =====================================================
    // GET BY SUBJECT
    // =====================================================

    public async Task<IEnumerable<AssignmentDto>> GetBySubjectAsync(
        Guid subjectId)
    {
        var assignments =
            await _repository.GetBySubjectIdAsync(
                subjectId);

        return assignments.Select(MapToDto);
    }


    // =====================================================
    // GET BY TEACHER
    // =====================================================

    public async Task<IEnumerable<AssignmentDto>> GetByTeacherAsync(
        string teacherId)
    {
        if (string.IsNullOrWhiteSpace(teacherId))
        {
            throw new ArgumentException(
                "Teacher ID is required.");
        }

        var assignments =
            await _repository.GetByTeacherIdAsync(
                teacherId);

        return assignments.Select(MapToDto);
    }


    // =====================================================
    // CREATE
    // =====================================================

    public async Task<AssignmentDto> CreateAsync(
        CreateAssignmentDto dto,
        string teacherId)
    {
        if (string.IsNullOrWhiteSpace(teacherId))
        {
            throw new ArgumentException(
                "Teacher ID is required.");
        }

        if (dto == null)
        {
            throw new ArgumentNullException(
                nameof(dto));
        }


        // -------------------------------------------------
        // Validation
        // -------------------------------------------------

        ValidateAssignment(
            dto.Title,
            dto.Deadline,
            dto.MaximumMarks);


        // -------------------------------------------------
        // Duplicate Check
        // -------------------------------------------------

        var exists =
            await _repository.ExistsForTeacherAsync(
                teacherId,
                dto.AcademicYearId,
                dto.ClassId,
                dto.SectionId,
                dto.CourseId,
                dto.SubjectId,
                dto.Title);

        if (exists)
        {
            throw new InvalidOperationException(
                "An assignment with the same title already exists for this class, course and subject.");
        }


        // -------------------------------------------------
        // Create Entity
        // -------------------------------------------------

        var assignment = new Assignment
        {
            Id = Guid.NewGuid(),

            TeacherId = teacherId,

            AcademicYearId =
                dto.AcademicYearId,

            ClassId =
                dto.ClassId,

            SectionId =
                dto.SectionId,

            CourseId =
                dto.CourseId,

            SubjectId =
                dto.SubjectId,


            // Assignment Information

            Title =
                dto.Title.Trim(),

            Description =
                dto.Description?.Trim(),

            Deadline =
                dto.Deadline,

            MaximumMarks =
                dto.MaximumMarks,


            // Attachment

            AttachmentUrl =
    dto.AttachmentUrl,

            AttachmentFileName =
    dto.Attachment?.FileName,

            AttachmentContentType =
    dto.Attachment?.ContentType,

            AttachmentFileSize =
    dto.Attachment?.Length,


            // Publication

            IsPublished =
                dto.IsPublished,

            PublishedAt =
                dto.IsPublished
                    ? DateTime.UtcNow
                    : null,


            // Active

            IsActive = true,


            // Audit

            CreatedAt =
                DateTime.UtcNow,

            CreatedBy =
                teacherId
        };


        var created =
            await _repository.CreateAsync(
                assignment);

        return MapToDto(created);
    }


    // =====================================================
    // UPDATE
    // =====================================================

    public async Task<AssignmentDto?> UpdateAsync(
        Guid id,
        UpdateAssignmentDto dto,
        string teacherId)
    {
        if (string.IsNullOrWhiteSpace(teacherId))
        {
            throw new ArgumentException(
                "Teacher ID is required.");
        }

        if (dto == null)
        {
            throw new ArgumentNullException(
                nameof(dto));
        }


        // -------------------------------------------------
        // Find Assignment
        // -------------------------------------------------

        var assignment =
            await _repository.GetByIdForTeacherAsync(
                id,
                teacherId);

        if (assignment == null)
        {
            return null;
        }


        // -------------------------------------------------
        // Validation
        // -------------------------------------------------

        ValidateAssignment(
            dto.Title,
            dto.Deadline,
            dto.MaximumMarks);


        // -------------------------------------------------
        // Duplicate Check
        // -------------------------------------------------

        var exists =
            await _repository.ExistsForTeacherAsync(
                teacherId,
                dto.AcademicYearId,
                dto.ClassId,
                dto.SectionId,
                dto.CourseId,
                dto.SubjectId,
                dto.Title,
                id);

        if (exists)
        {
            throw new InvalidOperationException(
                "An assignment with the same title already exists for this class, course and subject.");
        }


        // -------------------------------------------------
        // Academic Information
        // -------------------------------------------------

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


        // -------------------------------------------------
        // Assignment Information
        // -------------------------------------------------

        assignment.Title =
            dto.Title.Trim();

        assignment.Description =
            dto.Description?.Trim();

        assignment.Deadline =
            dto.Deadline;

        assignment.MaximumMarks =
            dto.MaximumMarks;


        // -------------------------------------------------
        // Attachment
        // -------------------------------------------------

        // =====================================================
        // ATTACHMENT
        // =====================================================

        if (dto.Attachment != null &&
            !string.IsNullOrWhiteSpace(dto.AttachmentUrl))
        {
            assignment.AttachmentUrl =
                dto.AttachmentUrl;

            assignment.AttachmentFileName =
                dto.AttachmentFileName;

            assignment.AttachmentContentType =
                dto.AttachmentContentType;

            assignment.AttachmentFileSize =
                dto.AttachmentFileSize;
        }


        // -------------------------------------------------
        // Publication
        // -------------------------------------------------

        if (dto.IsPublished &&
            !assignment.IsPublished)
        {
            assignment.PublishedAt =
                DateTime.UtcNow;
        }

        if (!dto.IsPublished)
        {
            assignment.PublishedAt = null;
        }

        assignment.IsPublished =
            dto.IsPublished;


        // -------------------------------------------------
        // Active
        // -------------------------------------------------

        assignment.IsActive =
            dto.IsActive;


        // -------------------------------------------------
        // Audit
        // -------------------------------------------------

        assignment.UpdatedAt =
            DateTime.UtcNow;

        assignment.UpdatedBy =
            teacherId;


        // -------------------------------------------------
        // Save
        // -------------------------------------------------

        var updated =
            await _repository.UpdateAsync(
                assignment);

        return MapToDto(updated);
    }


    // =====================================================
    // DELETE
    // =====================================================

    public async Task<bool> DeleteAsync(
        Guid id,
        string teacherId)
    {
        if (string.IsNullOrWhiteSpace(teacherId))
        {
            throw new ArgumentException(
                "Teacher ID is required.");
        }


        // -------------------------------------------------
        // Security Check
        // -------------------------------------------------

        var assignment =
            await _repository.GetByIdForTeacherAsync(
                id,
                teacherId);

        if (assignment == null)
        {
            return false;
        }


        return await _repository.DeleteAsync(id);
    }


    // =====================================================
    // PUBLISH
    // =====================================================

    public async Task<AssignmentDto?> PublishAsync(
        Guid id,
        string teacherId)
    {
        if (string.IsNullOrWhiteSpace(teacherId))
        {
            throw new ArgumentException(
                "Teacher ID is required.");
        }


        var assignment =
            await _repository.GetByIdForTeacherAsync(
                id,
                teacherId);

        if (assignment == null)
        {
            return null;
        }


        if (!assignment.IsActive)
        {
            throw new InvalidOperationException(
                "Inactive assignment cannot be published.");
        }


        // -------------------------------------------------
        // Publish
        // -------------------------------------------------

        assignment.IsPublished = true;

        assignment.PublishedAt =
            assignment.PublishedAt
            ?? DateTime.UtcNow;


        // -------------------------------------------------
        // Audit
        // -------------------------------------------------

        assignment.UpdatedAt =
            DateTime.UtcNow;

        assignment.UpdatedBy =
            teacherId;


        var updated =
            await _repository.UpdateAsync(
                assignment);

        return MapToDto(updated);
    }


    // =====================================================
    // UNPUBLISH / DRAFT
    // =====================================================

    public async Task<AssignmentDto?> UnpublishAsync(
        Guid id,
        string teacherId)
    {
        if (string.IsNullOrWhiteSpace(teacherId))
        {
            throw new ArgumentException(
                "Teacher ID is required.");
        }


        var assignment =
            await _repository.GetByIdForTeacherAsync(
                id,
                teacherId);

        if (assignment == null)
        {
            return null;
        }


        // -------------------------------------------------
        // Set Draft
        // -------------------------------------------------

        assignment.IsPublished = false;

        assignment.PublishedAt = null;


        // -------------------------------------------------
        // Audit
        // -------------------------------------------------

        assignment.UpdatedAt =
            DateTime.UtcNow;

        assignment.UpdatedBy =
            teacherId;


        var updated =
            await _repository.UpdateAsync(
                assignment);

        return MapToDto(updated);
    }


    // =====================================================
    // VALIDATION
    // =====================================================

    private static void ValidateAssignment(
        string title,
        DateTime deadline,
        decimal maximumMarks)
    {
        // -------------------------------------------------
        // Title
        // -------------------------------------------------

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException(
                "Assignment title is required.");
        }


        if (title.Trim().Length > 250)
        {
            throw new ArgumentException(
                "Assignment title cannot exceed 250 characters.");
        }


        // -------------------------------------------------
        // Maximum Marks
        // -------------------------------------------------

        if (maximumMarks <= 0)
        {
            throw new ArgumentException(
                "Maximum marks must be greater than zero.");
        }


        // -------------------------------------------------
        // Deadline
        // -------------------------------------------------

        if (deadline == default)
        {
            throw new ArgumentException(
                "Assignment deadline is required.");
        }
    }


    // =====================================================
    // ENTITY → DTO
    // =====================================================

    private static AssignmentDto MapToDto(
        Assignment assignment)
    {
        return new AssignmentDto
        {
            // -------------------------------------------------
            // ID
            // -------------------------------------------------

            Id =
                assignment.Id,


            // -------------------------------------------------
            // Teacher
            // -------------------------------------------------

            TeacherId =
                assignment.TeacherId,

            TeacherName =
                assignment.Teacher?.FullName,


            // -------------------------------------------------
            // Academic Year
            // -------------------------------------------------

            AcademicYearId =
                assignment.AcademicYearId,

            AcademicYearName =
                assignment.AcademicYear?.Name,


            // -------------------------------------------------
            // Class
            // -------------------------------------------------

            ClassId =
                assignment.ClassId,

            ClassName =
                assignment.Class?.Name,


            // -------------------------------------------------
            // Section
            // -------------------------------------------------

            SectionId =
                assignment.SectionId,

            SectionName =
                assignment.Section?.SectionName,


            // -------------------------------------------------
            // Course
            // -------------------------------------------------

            CourseId =
                assignment.CourseId,

            // IMPORTANT:
            // Change this if your Course entity uses
            // a different property name.

            CourseName =
                assignment.Course?.CourseName,


            // -------------------------------------------------
            // Subject
            // -------------------------------------------------

            SubjectId =
                assignment.SubjectId,

            SubjectName =
                assignment.Subject?.Name,


            // -------------------------------------------------
            // Assignment Information
            // -------------------------------------------------

            Title =
                assignment.Title,

            Description =
                assignment.Description,

            Deadline =
                assignment.Deadline,

            MaximumMarks =
                assignment.MaximumMarks,


            // -------------------------------------------------
            // Attachment
            // -------------------------------------------------

            AttachmentUrl =
                assignment.AttachmentUrl,

            AttachmentFileName =
                assignment.AttachmentFileName,

            AttachmentContentType =
                assignment.AttachmentContentType,

            AttachmentFileSize =
                assignment.AttachmentFileSize,


            // -------------------------------------------------
            // Publication
            // -------------------------------------------------

            IsPublished =
                assignment.IsPublished,

            PublishedAt =
                assignment.PublishedAt,


            // -------------------------------------------------
            // Active
            // -------------------------------------------------

            IsActive =
                assignment.IsActive,


            // -------------------------------------------------
            // Audit
            // -------------------------------------------------

            CreatedAt =
                assignment.CreatedAt,

            UpdatedAt =
                assignment.UpdatedAt,


            // -------------------------------------------------
            // Submissions
            // -------------------------------------------------

            SubmissionCount =
                assignment.Submissions?.Count ?? 0
        };
    }

    // =====================================================
    // GET ASSIGNMENTS FOR STUDENT
    // =====================================================

    public async Task<IEnumerable<AssignmentDto>>
        GetAssignmentsForStudentAsync(
            string studentId)
    {
        if (string.IsNullOrWhiteSpace(studentId))
        {
            throw new ArgumentException(
                "Student ID is required.");
        }

        var assignments =
            await _repository
                .GetAssignmentsForStudentAsync(
                    studentId);

        return assignments.Select(MapToDto);
    }


    // =====================================================
    // GET ASSIGNMENT FOR STUDENT
    // =====================================================

    public async Task<AssignmentDto?>
        GetAssignmentForStudentAsync(
            Guid id,
            string studentId)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Assignment ID is required.");
        }

        if (string.IsNullOrWhiteSpace(studentId))
        {
            throw new ArgumentException(
                "Student ID is required.");
        }

        var assignment =
            await _repository
                .GetAssignmentForStudentAsync(
                    id,
                    studentId);

        if (assignment == null)
        {
            return null;
        }

        return MapToDto(assignment);
    }
}