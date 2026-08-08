using Akkhor.Application.DTOs.StudentEnrollments;

namespace Akkhor.Application.Interfaces.Services;

public interface IStudentEnrollmentService
{
    // =====================================================
    // ENROLLMENT
    // =====================================================

    Task<List<StudentEnrollmentDto>>
        GetAllAsync();

    Task<StudentEnrollmentDto?>
        GetByIdAsync(Guid id);

    Task<StudentEnrollmentDto>
        CreateAsync(
            CreateStudentEnrollmentDto dto);

    Task<bool>
        UpdateAsync(
            Guid id,
            UpdateStudentEnrollmentDto dto);

    Task<bool>
        DeleteAsync(Guid id);


    // =====================================================
    // LOOKUPS
    // =====================================================

    Task<List<StudentLookupDto>>
        GetStudentsAsync();

    Task<List<ClassLookupDto>>
        GetClassesAsync();

    Task<List<CourseLookupDto>>
        GetCoursesByClassIdAsync(
            Guid classId);

    Task<List<SectionLookupDto>>
        GetSectionsByClassIdAsync(
            Guid classId);
}