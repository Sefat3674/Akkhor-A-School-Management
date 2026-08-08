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
        var data =
            await _repository.GetAllAsync();

        return data
            .Select(x => new StudentEnrollmentDto
            {
                Id = x.Id,

                StudentId = x.StudentId,

                StudentName =
                    x.Student.UserName,

                ClassId = x.ClassId,

                ClassName =
                    x.Class.Name,

                CourseId = x.CourseId,

                CourseName =
                    x.Course.CourseName,

                SectionId = x.SectionId,

                SectionName =
                    x.Section != null
                        ? x.Section.SectionName
                        : null,

                RollNumber =
                    x.RollNumber,

                EnrollmentDate =
                    x.EnrollmentDate,

                Status =
                    x.Status,

                CreatedAt =
                    x.CreatedAt,

                UpdatedAt =
                    x.UpdatedAt
            })
            .ToList();
    }


    // =====================================================
    // GET BY ID
    // =====================================================

    public async Task<StudentEnrollmentDto?>
        GetByIdAsync(Guid id)
    {
        var x =
            await _repository.GetByIdAsync(id);

        if (x == null)
            return null;

        return new StudentEnrollmentDto
        {
            Id = x.Id,

            StudentId = x.StudentId,

            StudentName =
                x.Student.UserName,

            ClassId = x.ClassId,

            ClassName =
                x.Class.Name,

            CourseId = x.CourseId,

            CourseName =
                x.Course.CourseName,

            SectionId = x.SectionId,

            SectionName =
                x.Section?.SectionName,

            RollNumber =
                x.RollNumber,

            EnrollmentDate =
                x.EnrollmentDate,

            Status =
                x.Status,

            CreatedAt =
                x.CreatedAt,

            UpdatedAt =
                x.UpdatedAt
        };
    }


    // =====================================================
    // CREATE
    // =====================================================

    public async Task<StudentEnrollmentDto>
        CreateAsync(
            CreateStudentEnrollmentDto dto)
    {
        var exists =
            await _repository.ExistsAsync(
                dto.StudentId,
                dto.ClassId,
                dto.CourseId);

        if (exists)
        {
            throw new Exception(
                "Student already enrolled in this course.");
        }

        var entity =
            new StudentEnrollment
            {
                Id = Guid.NewGuid(),

                StudentId =
                    dto.StudentId,

                ClassId =
                    dto.ClassId,

                CourseId =
                    dto.CourseId,

                SectionId =
                    dto.SectionId,

                RollNumber =
                    dto.RollNumber,

                EnrollmentDate =
                    dto.EnrollmentDate,

                Status =
                    dto.Status,

                CreatedAt =
                    DateTime.UtcNow
            };

        await _repository.AddAsync(entity);

        var result =
            await _repository.GetByIdAsync(entity.Id);

        if (result == null)
        {
            throw new Exception(
                "Failed to create student enrollment.");
        }

        return new StudentEnrollmentDto
        {
            Id = result.Id,

            StudentId =
                result.StudentId,

            StudentName =
                result.Student.UserName,

            ClassId =
                result.ClassId,

            ClassName =
                result.Class.Name,

            CourseId =
                result.CourseId,

            CourseName =
                result.Course.CourseName,

            SectionId =
                result.SectionId,

            SectionName =
                result.Section?.SectionName,

            RollNumber =
                result.RollNumber,

            EnrollmentDate =
                result.EnrollmentDate,

            Status =
                result.Status,

            CreatedAt =
                result.CreatedAt,

            UpdatedAt =
                result.UpdatedAt
        };
    }


    // =====================================================
    // UPDATE
    // =====================================================

    public async Task<bool>
        UpdateAsync(
            Guid id,
            UpdateStudentEnrollmentDto dto)
    {
        var entity =
            await _repository.GetByIdAsync(id);

        if (entity == null)
            return false;

        entity.ClassId =
            dto.ClassId;

        entity.CourseId =
            dto.CourseId;

        entity.SectionId =
            dto.SectionId;

        entity.RollNumber =
            dto.RollNumber;

        entity.Status =
            dto.Status;

        entity.UpdatedAt =
            DateTime.UtcNow;

        await _repository.UpdateAsync(entity);

        return true;
    }


    // =====================================================
    // DELETE
    // =====================================================

    public async Task<bool>
        DeleteAsync(Guid id)
    {
        var entity =
            await _repository.GetByIdAsync(id);

        if (entity == null)
            return false;

        await _repository.DeleteAsync(id);

        return true;
    }


    // =====================================================
    // GET STUDENTS
    // =====================================================

    public async Task<List<StudentLookupDto>>
        GetStudentsAsync()
    {
        var students =
            await _repository.GetStudentsAsync();

        return students
            .Select(x => new StudentLookupDto
            {
                Id = x.Id,

                UserName =
                    x.UserName ?? string.Empty
            })
            .ToList();
    }


    // =====================================================
    // GET CLASSES
    // =====================================================

    public async Task<List<ClassLookupDto>>
        GetClassesAsync()
    {
        var classes =
            await _repository.GetClassesAsync();

        return classes
            .Select(x => new ClassLookupDto
            {
                Id = x.Id,

                Name =
                    x.Name
            })
            .ToList();
    }


    // =====================================================
    // GET COURSES BY CLASS
    // =====================================================

    public async Task<List<CourseLookupDto>>
        GetCoursesByClassIdAsync(
            Guid classId)
    {
        var courses =
            await _repository
                .GetCoursesByClassIdAsync(classId);

        return courses
            .Select(x => new CourseLookupDto
            {
                Id = x.Id,

                ClassId =
                    x.ClassId,

                CourseName =
                    x.CourseName
            })
            .ToList();
    }


    // =====================================================
    // GET SECTIONS BY CLASS
    // =====================================================

    public async Task<List<SectionLookupDto>>
        GetSectionsByClassIdAsync(
            Guid classId)
    {
        var sections =
            await _repository
                .GetSectionsByClassIdAsync(classId);

        return sections
            .Select(x => new SectionLookupDto
            {
                Id = x.Id,

                ClassId =
                    x.ClassId,

                SectionName =
                    x.SectionName
            })
            .ToList();
    }
}