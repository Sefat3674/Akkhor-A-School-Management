namespace Akkhor.Application.DTOs.StudentEnrollments;

public class CreateStudentEnrollmentDto
{

    public string StudentId { get; set; } = "";


    public Guid ClassId { get; set; }


    public Guid CourseId { get; set; }


    public Guid? SectionId { get; set; }


    public string? RollNumber { get; set; }


    public DateOnly EnrollmentDate { get; set; }


    public string Status { get; set; }
        = "Active";
}