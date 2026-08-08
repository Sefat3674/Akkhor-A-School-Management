using Akkhor.Domain.Entities;

namespace Akkhor.Application.Interfaces.Repositories;

public interface IStudentEnrollmentRepository
{

    Task<List<StudentEnrollment>> GetAllAsync();


    Task<StudentEnrollment?> GetByIdAsync(Guid id);


    Task AddAsync(StudentEnrollment entity);


    Task UpdateAsync(StudentEnrollment entity);


    Task DeleteAsync(Guid id);


    Task<bool> ExistsAsync(
        string studentId,
        Guid classId,
        Guid courseId);


        Task<List<Users>> GetStudentsAsync();

    Task<List<Class>> GetClassesAsync();

    Task<List<Course>> GetCoursesByClassIdAsync(
        Guid classId);

    Task<List<ClassSection>> GetSectionsByClassIdAsync(
        Guid classId);

}