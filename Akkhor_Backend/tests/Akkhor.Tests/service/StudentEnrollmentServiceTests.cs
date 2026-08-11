using Akkhor.Application.DTOs.StudentEnrollments;
using Akkhor.Application.Interfaces.Repositories;
using Akkhor.Application.Services;
using Akkhor.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Akkhor.Tests.Services;

public class StudentEnrollmentServiceTests
{
    private readonly Mock<IStudentEnrollmentRepository> _repositoryMock;
    private readonly StudentEnrollmentService _service;

    public StudentEnrollmentServiceTests()
    {
        _repositoryMock = new Mock<IStudentEnrollmentRepository>();
        _service = new StudentEnrollmentService(_repositoryMock.Object);
    }

    // =====================================================
    // GET ALL
    // =====================================================

    [Fact]
    public async Task GetAllAsync_ShouldReturnEnrollments()
    {
        var enrollment = CreateEnrollment();

        _repositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<StudentEnrollment>
            {
                enrollment
            });

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(1);
        result[0].Id.Should().Be(enrollment.Id);
        result[0].StudentId.Should().Be(enrollment.StudentId);
        result[0].ClassId.Should().Be(enrollment.ClassId);
        result[0].CourseId.Should().Be(enrollment.CourseId);

        _repositoryMock.Verify(
            x => x.GetAllAsync(),
            Times.Once);
    }

    // =====================================================
    // GET BY ID
    // =====================================================

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEnrollment_WhenExists()
    {
        var id = Guid.NewGuid();
        var enrollment = CreateEnrollment(id);

        _repositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(enrollment);

        var result = await _service.GetByIdAsync(id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.StudentId.Should().Be(enrollment.StudentId);
        result.ClassId.Should().Be(enrollment.ClassId);
        result.CourseId.Should().Be(enrollment.CourseId);

        _repositoryMock.Verify(
            x => x.GetByIdAsync(id),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync((StudentEnrollment?)null);

        var result = await _service.GetByIdAsync(id);

        result.Should().BeNull();

        _repositoryMock.Verify(
            x => x.GetByIdAsync(id),
            Times.Once);
    }

    // =====================================================
    // CREATE
    // =====================================================

    [Fact]
    public async Task CreateAsync_ShouldCreateEnrollment()
    {
        var studentId = "student-1";
        var classId = Guid.NewGuid();
        var courseId = Guid.NewGuid();
        var sectionId = Guid.NewGuid();

        var dto = new CreateStudentEnrollmentDto
        {
            StudentId = studentId,
            ClassId = classId,
            CourseId = courseId,
            SectionId = sectionId,
            RollNumber = "10",
            EnrollmentDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Status = "Active"
        };

        StudentEnrollment? createdEntity = null;

        _repositoryMock
            .Setup(x => x.ExistsAsync(
                studentId,
                classId,
                courseId))
            .ReturnsAsync(false);

        _repositoryMock
            .Setup(x => x.AddAsync(
                It.IsAny<StudentEnrollment>()))
            .Callback<StudentEnrollment>(
                entity => createdEntity = entity)
            .Returns(Task.CompletedTask);

        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                It.IsAny<Guid>()))
            .ReturnsAsync(() => createdEntity);

        var result = await _service.CreateAsync(dto);

        result.Should().NotBeNull();
        result.StudentId.Should().Be(studentId);
        result.ClassId.Should().Be(classId);
        result.CourseId.Should().Be(courseId);
        result.SectionId.Should().Be(sectionId);
        result.RollNumber.Should().Be("10");
        result.Status.Should().Be("Active");

        _repositoryMock.Verify(
            x => x.ExistsAsync(
                studentId,
                classId,
                courseId),
            Times.Once);

        _repositoryMock.Verify(
            x => x.AddAsync(
                It.IsAny<StudentEnrollment>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenAlreadyEnrolled()
    {
        var studentId = "student-1";
        var classId = Guid.NewGuid();
        var courseId = Guid.NewGuid();

        var dto = new CreateStudentEnrollmentDto
        {
            StudentId = studentId,
            ClassId = classId,
            CourseId = courseId,
            RollNumber = "10",
            EnrollmentDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Status = "Active"
        };

        _repositoryMock
            .Setup(x => x.ExistsAsync(
                studentId,
                classId,
                courseId))
            .ReturnsAsync(true);

        var act = async () =>
            await _service.CreateAsync(dto);

        await act.Should()
            .ThrowAsync<Exception>()
            .WithMessage(
                "Student already enrolled in this course.");

        _repositoryMock.Verify(
            x => x.AddAsync(
                It.IsAny<StudentEnrollment>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenCreatedEntityCannotBeFound()
    {
        var studentId = "student-1";
        var classId = Guid.NewGuid();
        var courseId = Guid.NewGuid();

        var dto = new CreateStudentEnrollmentDto
        {
            StudentId = studentId,
            ClassId = classId,
            CourseId = courseId,
            RollNumber = "10",
            EnrollmentDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Status = "Active"
        };

        _repositoryMock
            .Setup(x => x.ExistsAsync(
                studentId,
                classId,
                courseId))
            .ReturnsAsync(false);

        _repositoryMock
            .Setup(x => x.AddAsync(
                It.IsAny<StudentEnrollment>()))
            .Returns(Task.CompletedTask);

        _repositoryMock
            .Setup(x => x.GetByIdAsync(
                It.IsAny<Guid>()))
            .ReturnsAsync((StudentEnrollment?)null);

        var act = async () =>
            await _service.CreateAsync(dto);

        await act.Should()
            .ThrowAsync<Exception>()
            .WithMessage(
                "Failed to create student enrollment.");
    }

    // =====================================================
    // UPDATE
    // =====================================================

    [Fact]
    public async Task UpdateAsync_ShouldReturnTrue_WhenEnrollmentExists()
    {
        var id = Guid.NewGuid();

        var entity = CreateEnrollment(id);

        var newClassId = Guid.NewGuid();
        var newCourseId = Guid.NewGuid();
        var newSectionId = Guid.NewGuid();

        var dto = new UpdateStudentEnrollmentDto
        {
            ClassId = newClassId,
            CourseId = newCourseId,
            SectionId = newSectionId,
            RollNumber = "25",
            Status = "Active"
        };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(entity);

        _repositoryMock
            .Setup(x => x.UpdateAsync(
                It.IsAny<StudentEnrollment>()))
            .Returns(Task.CompletedTask);

        var result =
            await _service.UpdateAsync(id, dto);

        result.Should().BeTrue();

        entity.ClassId.Should().Be(newClassId);
        entity.CourseId.Should().Be(newCourseId);
        entity.SectionId.Should().Be(newSectionId);
        entity.RollNumber.Should().Be("25");
        entity.Status.Should().Be("Active");
        entity.UpdatedAt.Should().NotBeNull();

        _repositoryMock.Verify(
            x => x.UpdateAsync(
                It.Is<StudentEnrollment>(
                    e =>
                        e.Id == id &&
                        e.ClassId == newClassId &&
                        e.CourseId == newCourseId &&
                        e.RollNumber == "25" &&
                        e.Status == "Active")),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenEnrollmentNotFound()
    {
        var id = Guid.NewGuid();

        var dto = new UpdateStudentEnrollmentDto
        {
            ClassId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            SectionId = Guid.NewGuid(),
            RollNumber = "20",
            Status = "Active"
        };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync((StudentEnrollment?)null);

        var result =
            await _service.UpdateAsync(id, dto);

        result.Should().BeFalse();

        _repositoryMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<StudentEnrollment>()),
            Times.Never);
    }

    // =====================================================
    // DELETE
    // =====================================================

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenEnrollmentExists()
    {
        var id = Guid.NewGuid();

        var entity = CreateEnrollment(id);

        _repositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(entity);

        _repositoryMock
            .Setup(x => x.DeleteAsync(id))
            .Returns(Task.CompletedTask);

        var result =
            await _service.DeleteAsync(id);

        result.Should().BeTrue();

        _repositoryMock.Verify(
            x => x.DeleteAsync(id),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenEnrollmentNotFound()
    {
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync((StudentEnrollment?)null);

        var result =
            await _service.DeleteAsync(id);

        result.Should().BeFalse();

        _repositoryMock.Verify(
            x => x.DeleteAsync(id),
            Times.Never);
    }

    // =====================================================
    // GET STUDENTS
    // =====================================================

    [Fact]
    public async Task GetStudentsAsync_ShouldReturnStudents()
    {
        var students = new List<Users>
        {
            new Users
            {
                Id = "student-1",
                UserName = "student1"
            },
            new Users
            {
                Id = "student-2",
                UserName = "student2"
            }
        };

        _repositoryMock
            .Setup(x => x.GetStudentsAsync())
            .ReturnsAsync(students);

        var result =
            await _service.GetStudentsAsync();

        result.Should().HaveCount(2);

        result[0].Id.Should().Be("student-1");
        result[0].UserName.Should().Be("student1");

        result[1].Id.Should().Be("student-2");
        result[1].UserName.Should().Be("student2");

        _repositoryMock.Verify(
            x => x.GetStudentsAsync(),
            Times.Once);
    }

    // =====================================================
    // GET CLASSES
    // =====================================================

    [Fact]
    public async Task GetClassesAsync_ShouldReturnClasses()
    {
        var classes = new List<Class>
        {
            new Class
            {
                Id = Guid.NewGuid(),
                Name = "Class 6"
            },
            new Class
            {
                Id = Guid.NewGuid(),
                Name = "Class 7"
            }
        };

        _repositoryMock
            .Setup(x => x.GetClassesAsync())
            .ReturnsAsync(classes);

        var result =
            await _service.GetClassesAsync();

        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Class 6");
        result[1].Name.Should().Be("Class 7");

        _repositoryMock.Verify(
            x => x.GetClassesAsync(),
            Times.Once);
    }

    // =====================================================
    // GET COURSES BY CLASS
    // =====================================================

    [Fact]
    public async Task GetCoursesByClassIdAsync_ShouldReturnCourses()
    {
        var classId = Guid.NewGuid();

        var courses = new List<Course>
        {
            new Course
            {
                Id = Guid.NewGuid(),
                ClassId = classId,
                CourseName = "Mathematics"
            },
            new Course
            {
                Id = Guid.NewGuid(),
                ClassId = classId,
                CourseName = "Science"
            }
        };

        _repositoryMock
            .Setup(x => x.GetCoursesByClassIdAsync(classId))
            .ReturnsAsync(courses);

        var result =
            await _service.GetCoursesByClassIdAsync(classId);

        result.Should().HaveCount(2);

        result[0].CourseName.Should().Be("Mathematics");
        result[1].CourseName.Should().Be("Science");

        result[0].ClassId.Should().Be(classId);
        result[1].ClassId.Should().Be(classId);

        _repositoryMock.Verify(
            x => x.GetCoursesByClassIdAsync(classId),
            Times.Once);
    }

    // =====================================================
    // GET SECTIONS BY CLASS
    // =====================================================

    [Fact]
    public async Task GetSectionsByClassIdAsync_ShouldReturnSections()
    {
        var classId = Guid.NewGuid();

        var sections = new List<ClassSection>
        {
            new ClassSection
            {
                Id = Guid.NewGuid(),
                ClassId = classId,
                SectionName = "A"
            },
            new ClassSection
            {
                Id = Guid.NewGuid(),
                ClassId = classId,
                SectionName = "B"
            }
        };

        _repositoryMock
            .Setup(x => x.GetSectionsByClassIdAsync(classId))
            .ReturnsAsync(sections);

        var result =
            await _service.GetSectionsByClassIdAsync(classId);

        result.Should().HaveCount(2);

        result[0].SectionName.Should().Be("A");
        result[1].SectionName.Should().Be("B");

        result[0].ClassId.Should().Be(classId);
        result[1].ClassId.Should().Be(classId);

        _repositoryMock.Verify(
            x => x.GetSectionsByClassIdAsync(classId),
            Times.Once);
    }

    // =====================================================
    // TEST DATA
    // =====================================================

    private static StudentEnrollment CreateEnrollment(
        Guid? id = null)
    {
        var classId = Guid.NewGuid();
        var courseId = Guid.NewGuid();
        var sectionId = Guid.NewGuid();

        return new StudentEnrollment
        {
            Id = id ?? Guid.NewGuid(),

            StudentId = "student-1",

            ClassId = classId,

            CourseId = courseId,

            SectionId = sectionId,

            RollNumber = "10",

            EnrollmentDate =
                DateOnly.FromDateTime(DateTime.UtcNow),

            Status = "Active",

            CreatedAt = DateTime.UtcNow,

            UpdatedAt = null,

            Student = new Users
            {
                Id = "student-1",
                UserName = "student1"
            },

            Class = new Class
            {
                Id = classId,
                Name = "Class 6"
            },

            Course = new Course
            {
                Id = courseId,
                ClassId = classId,
                CourseName = "Mathematics"
            },

            Section = new ClassSection
            {
                Id = sectionId,
                ClassId = classId,
                SectionName = "A"
            }
        };
    }
}