using Akkhor.Application.DTOs.Courses;
using Akkhor.Application.Interfaces.Repositories;
using Akkhor.Application.Interfaces.Services;
using Akkhor.Domain.Entities;

namespace Akkhor.Application.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _repository;


    public CourseService(ICourseRepository repository)
    {
        _repository = repository;
    }



    public async Task<IEnumerable<CourseDto>> GetAllAsync()
    {
        var courses = await _repository.GetAllAsync();


        return courses.Select(x => new CourseDto
        {
            Id = x.Id,

            ClassId = x.ClassId,

            ClassName = x.Class?.Name ?? "",

            CourseName = x.CourseName,

            CourseCode = x.CourseCode,

            Description = x.Description,

            DurationMonths = x.DurationMonths,

            IsActive = x.IsActive,

            SubjectCount = x.CourseSubjects.Count,

            StudentCount = x.StudentEnrollments.Count,

            CreatedAt = x.CreatedAt
        });
    }





    public async Task<CourseDto?> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);


        if (entity == null)
            return null;



        return new CourseDto
        {
            Id = entity.Id,

            ClassId = entity.ClassId,

            ClassName = entity.Class?.Name ?? "",

            CourseName = entity.CourseName,

            CourseCode = entity.CourseCode,

            Description = entity.Description,

            DurationMonths = entity.DurationMonths,

            IsActive = entity.IsActive,

            SubjectCount = entity.CourseSubjects.Count,

            StudentCount = entity.StudentEnrollments.Count,

            CreatedAt = entity.CreatedAt
        };
    }





    public async Task<CourseDto> CreateAsync(CreateCourseDto dto)
    {
        var exists = await _repository
            .GetByCodeAsync(dto.CourseCode);



        if (exists != null)
            throw new Exception("Course code already exists");



        var entity = new Course
        {
            Id = Guid.NewGuid(),

            ClassId = dto.ClassId,

            CourseName = dto.CourseName,

            CourseCode = dto.CourseCode,

            Description = dto.Description,

            DurationMonths = dto.DurationMonths,

            IsActive = true,

            CreatedAt = DateTime.UtcNow
        };



        await _repository.AddAsync(entity);



        return await GetByIdAsync(entity.Id)
            ?? throw new Exception("Unable to create course");
    }





    public async Task<bool> UpdateAsync(
        Guid id,
        UpdateCourseDto dto)
    {
        var entity = await _repository
            .GetByIdAsync(id);



        if (entity == null)
            return false;



        entity.CourseName = dto.CourseName;

        entity.CourseCode = dto.CourseCode;

        entity.Description = dto.Description;

        entity.DurationMonths = dto.DurationMonths;

        entity.IsActive = dto.IsActive;


        entity.UpdatedAt = DateTime.UtcNow;



        await _repository.UpdateAsync(entity);


        return true;
    }





    public async Task<bool> DeleteAsync(Guid id)
    {
        var exists = await _repository
            .ExistsAsync(id);



        if (!exists)
            return false;



        await _repository.DeleteAsync(id);


        return true;
    }
}