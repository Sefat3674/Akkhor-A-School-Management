
using Akkhor.Application.DTOs.StudentEnrollments;
using Akkhor.Application.Interfaces.Repositories;
using Akkhor.Application.Interfaces.Services;
using Akkhor.Domain.Entities;

namespace Akkhor.Application.Services;

public class StudentEnrollmentService
    : IStudentEnrollmentService
{
    private readonly IStudentEnrollmentRepository _repository;

    public StudentEnrollmentService(
        IStudentEnrollmentRepository repository)
    {
        _repository = repository;
    }

    // =====================================================
    // GET ALL ENROLLMENTS
    // =====================================================

    public async Task<List<StudentEnrollmentDto>> GetAllAsync()
    {
        var data = await _repository.GetAllAsync();

        return data
            .Select(MapToDto)
            .ToList();
    }

    // =====================================================
    // GET BY ID
    // =====================================================

    public async Task<StudentEnrollmentDto?> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
        {
            return null;
        }

        return MapToDto(entity);
    }

    // =====================================================
    // CREATE
    // =====================================================

    public async Task<StudentEnrollmentDto> CreateAsync(
        CreateStudentEnrollmentDto dto)
    {
        var exists = await _repository.ExistsAsync(
            dto.StudentId,
            dto.ClassId,
            dto.CourseId);

        if (exists)
        {
            throw new Exception(
                "Student already enrolled in this course.");
        }

        var entity = new StudentEnrollment
        {
            Id = Guid.NewGuid(),

            StudentId = dto.StudentId,

            ClassId = dto.ClassId,

            CourseId = dto.CourseId,

            SectionId = dto.SectionId,

            RollNumber = dto.RollNumber,

            EnrollmentDate = dto.EnrollmentDate,

            Status = dto.Status,

            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(entity);

        var result = await _repository.GetByIdAsync(entity.Id);

        if (result == null)
        {
            throw new Exception(
                "Failed to create student enrollment.");
        }

        return MapToDto(result);
    }

    // =====================================================
    // UPDATE
    // =====================================================

    public async Task<bool> UpdateAsync(
        Guid id,
        UpdateStudentEnrollmentDto dto)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
        {
            return false;
        }

        entity.ClassId = dto.ClassId;

        entity.CourseId = dto.CourseId;

        entity.SectionId = dto.SectionId;

        entity.RollNumber = dto.RollNumber;

        entity.Status = dto.Status;

        entity.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(entity);

        return true;
    }

    // =====================================================
    // DELETE
    // =====================================================

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
        {
            return false;
        }

        await _repository.DeleteAsync(id);

        return true;
    }

    // =====================================================
    // GET STUDENTS
    // =====================================================

    public async Task<List<StudentLookupDto>> GetStudentsAsync()
    {
        var students = await _repository.GetStudentsAsync();

        return students
            .Select(x => new StudentLookupDto
            {
                Id = x.Id,
                UserName = x.UserName ?? string.Empty
            })
            .ToList();
    }

    // =====================================================
    // GET CLASSES
    // =====================================================

    public async Task<List<ClassLookupDto>> GetClassesAsync()
    {
        var classes = await _repository.GetClassesAsync();

        return classes
            .Select(x => new ClassLookupDto
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToList();
    }

    // =====================================================
    // GET COURSES BY CLASS
    // =====================================================

    public async Task<List<CourseLookupDto>>
        GetCoursesByClassIdAsync(Guid classId)
    {
        var courses =
            await _repository.GetCoursesByClassIdAsync(classId);

        return courses
            .Select(x => new CourseLookupDto
            {
                Id = x.Id,

                ClassId = x.ClassId,

                CourseName = x.CourseName
            })
            .ToList();
    }

    // =====================================================
    // GET SECTIONS BY CLASS
    // =====================================================

    public async Task<List<SectionLookupDto>>
        GetSectionsByClassIdAsync(Guid classId)
    {
        var sections =
            await _repository.GetSectionsByClassIdAsync(classId);

        return sections
            .Select(x => new SectionLookupDto
            {
                Id = x.Id,

                ClassId = x.ClassId,

                SectionName = x.SectionName
            })
            .ToList();
    }

    // =====================================================
    // ENTITY → DTO
    // =====================================================

    private static StudentEnrollmentDto MapToDto(
        StudentEnrollment x)
    {
        return new StudentEnrollmentDto
        {
            // -------------------------------------------------
            // Enrollment
            // -------------------------------------------------

            Id = x.Id,

            StudentId = x.StudentId,

            StudentName =
                x.Student?.UserName
                ?? string.Empty,

            // -------------------------------------------------
            // Class
            // -------------------------------------------------

            ClassId = x.ClassId,

            ClassName =
                x.Class?.Name
                ?? string.Empty,

            // -------------------------------------------------
            // Course
            // -------------------------------------------------

            CourseId = x.CourseId,

            CourseName =
                x.Course?.CourseName
                ?? string.Empty,

            // -------------------------------------------------
            // Section
            // -------------------------------------------------

            SectionId = x.SectionId,

            SectionName =
                x.Section?.SectionName,

            // -------------------------------------------------
            // Enrollment Information
            // -------------------------------------------------

            RollNumber = x.RollNumber,

            EnrollmentDate = x.EnrollmentDate,

            Status = x.Status,

            // -------------------------------------------------
            // Audit
            // -------------------------------------------------

            CreatedAt = x.CreatedAt,

            UpdatedAt = x.UpdatedAt
        };
    }
}

