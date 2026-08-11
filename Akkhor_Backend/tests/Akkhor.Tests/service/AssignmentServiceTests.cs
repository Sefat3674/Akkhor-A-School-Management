using Akkhor.Application.Interfaces.Repositories;
using Akkhor.Application.Services;
using Akkhor.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Akkhor.Tests.Services;

public class AssignmentServiceTests
{
    private readonly Mock<IAssignmentRepository> _repositoryMock;
    private readonly AssignmentService _service;

    public AssignmentServiceTests()
    {
        _repositoryMock = new Mock<IAssignmentRepository>();

        _service = new AssignmentService(
            _repositoryMock.Object);
    }

    // =====================================================
    // GET ALL
    // =====================================================

    [Fact]
    public async Task GetAllAsync_ShouldReturnAssignments()
    {
        // Arrange
        var assignments = new List<Assignment>
        {
            CreateAssignment("Assignment 1"),
            CreateAssignment("Assignment 2")
        };

        _repositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(assignments);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);

        _repositoryMock.Verify(
            x => x.GetAllAsync(),
            Times.Once);
    }

    // =====================================================
    // GET BY ID
    // =====================================================

    [Fact]
    public async Task GetByIdAsync_ShouldReturnAssignment_WhenExists()
    {
        // Arrange
        var id = Guid.NewGuid();

        var assignment = CreateAssignment("Test Assignment");
        assignment.Id = id;

        _repositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(assignment);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.Title.Should().Be("Test Assignment");

        _repositoryMock.Verify(
            x => x.GetByIdAsync(id),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync((Assignment?)null);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().BeNull();
    }

    // =====================================================
    // GET MY ASSIGNMENTS
    // =====================================================

    [Fact]
    public async Task GetMyAssignmentsAsync_ShouldReturnAssignments()
    {
        // Arrange
        var teacherId = "teacher-001";

        var assignments = new List<Assignment>
        {
            CreateAssignment("Math Assignment"),
            CreateAssignment("Science Assignment")
        };

        _repositoryMock
            .Setup(x => x.GetByTeacherIdAsync(teacherId))
            .ReturnsAsync(assignments);

        // Act
        var result =
            await _service.GetMyAssignmentsAsync(teacherId);

        // Assert
        result.Should().HaveCount(2);

        _repositoryMock.Verify(
            x => x.GetByTeacherIdAsync(teacherId),
            Times.Once);
    }

    [Fact]
    public async Task GetMyAssignmentsAsync_ShouldThrow_WhenTeacherIdIsEmpty()
    {
        // Act
        Func<Task> act = async () =>
            await _service.GetMyAssignmentsAsync("");

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Teacher ID is required.");
    }

    // =====================================================
    // GET MY ASSIGNMENT BY ID
    // =====================================================

    [Fact]
    public async Task GetMyAssignmentByIdAsync_ShouldReturnAssignment()
    {
        // Arrange
        var id = Guid.NewGuid();
        var teacherId = "teacher-001";

        var assignment =
            CreateAssignment("Teacher Assignment");

        assignment.Id = id;

        _repositoryMock
            .Setup(x =>
                x.GetByIdForTeacherAsync(
                    id,
                    teacherId))
            .ReturnsAsync(assignment);

        // Act
        var result =
            await _service.GetMyAssignmentByIdAsync(
                id,
                teacherId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.Title.Should().Be("Teacher Assignment");
    }

    [Fact]
    public async Task GetMyAssignmentByIdAsync_ShouldThrow_WhenTeacherIdIsEmpty()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        Func<Task> act = async () =>
            await _service.GetMyAssignmentByIdAsync(
                id,
                "");

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Teacher ID is required.");
    }

    // =====================================================
    // GET BY CLASS
    // =====================================================

    [Fact]
    public async Task GetByClassAsync_ShouldReturnAssignments()
    {
        // Arrange
        var classId = Guid.NewGuid();

        var assignments = new List<Assignment>
        {
            CreateAssignment("Class Assignment")
        };

        _repositoryMock
            .Setup(x => x.GetByClassIdAsync(classId))
            .ReturnsAsync(assignments);

        // Act
        var result =
            await _service.GetByClassAsync(classId);

        // Assert
        result.Should().HaveCount(1);
        result.First().Title.Should().Be("Class Assignment");
    }

    // =====================================================
    // GET BY COURSE
    // =====================================================

    [Fact]
    public async Task GetByCourseAsync_ShouldReturnAssignments()
    {
        // Arrange
        var courseId = Guid.NewGuid();

        var assignments = new List<Assignment>
        {
            CreateAssignment("Course Assignment")
        };

        _repositoryMock
            .Setup(x => x.GetByCourseIdAsync(courseId))
            .ReturnsAsync(assignments);

        // Act
        var result =
            await _service.GetByCourseAsync(courseId);

        // Assert
        result.Should().HaveCount(1);
        result.First().Title.Should().Be("Course Assignment");
    }

    // =====================================================
    // GET BY SUBJECT
    // =====================================================

    [Fact]
    public async Task GetBySubjectAsync_ShouldReturnAssignments()
    {
        // Arrange
        var subjectId = Guid.NewGuid();

        var assignments = new List<Assignment>
        {
            CreateAssignment("Subject Assignment")
        };

        _repositoryMock
            .Setup(x => x.GetBySubjectIdAsync(subjectId))
            .ReturnsAsync(assignments);

        // Act
        var result =
            await _service.GetBySubjectAsync(subjectId);

        // Assert
        result.Should().HaveCount(1);
        result.First().Title.Should().Be("Subject Assignment");
    }

    // =====================================================
    // GET ASSIGNMENTS FOR STUDENT
    // =====================================================

    [Fact]
    public async Task GetAssignmentsForStudentAsync_ShouldReturnAssignments()
    {
        // Arrange
        var studentId = "student-001";

        var assignments = new List<Assignment>
        {
            CreateAssignment("Student Assignment 1"),
            CreateAssignment("Student Assignment 2")
        };

        _repositoryMock
            .Setup(x =>
                x.GetAssignmentsForStudentAsync(studentId))
            .ReturnsAsync(assignments);

        // Act
        var result =
            await _service.GetAssignmentsForStudentAsync(
                studentId);

        // Assert
        result.Should().HaveCount(2);

        _repositoryMock.Verify(
            x =>
                x.GetAssignmentsForStudentAsync(studentId),
            Times.Once);
    }

    [Fact]
    public async Task GetAssignmentsForStudentAsync_ShouldThrow_WhenStudentIdIsEmpty()
    {
        // Act
        Func<Task> act = async () =>
            await _service.GetAssignmentsForStudentAsync("");

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Student ID is required.");
    }

    // =====================================================
    // GET ASSIGNMENT FOR STUDENT
    // =====================================================

    [Fact]
    public async Task GetAssignmentForStudentAsync_ShouldReturnAssignment()
    {
        // Arrange
        var id = Guid.NewGuid();
        var studentId = "student-001";

        var assignment =
            CreateAssignment("Student Assignment");

        assignment.Id = id;

        _repositoryMock
            .Setup(x =>
                x.GetAssignmentForStudentAsync(
                    id,
                    studentId))
            .ReturnsAsync(assignment);

        // Act
        var result =
            await _service.GetAssignmentForStudentAsync(
                id,
                studentId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.Title.Should().Be("Student Assignment");
    }

    [Fact]
    public async Task GetAssignmentForStudentAsync_ShouldThrow_WhenAssignmentIdIsEmpty()
    {
        // Arrange
        var studentId = "student-001";

        // Act
        Func<Task> act = async () =>
            await _service.GetAssignmentForStudentAsync(
                Guid.Empty,
                studentId);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Assignment ID is required.");
    }

    [Fact]
    public async Task GetAssignmentForStudentAsync_ShouldThrow_WhenStudentIdIsEmpty()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        Func<Task> act = async () =>
            await _service.GetAssignmentForStudentAsync(
                id,
                "");

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Student ID is required.");
    }

    [Fact]
    public async Task GetAssignmentForStudentAsync_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var studentId = "student-001";

        _repositoryMock
            .Setup(x =>
                x.GetAssignmentForStudentAsync(
                    id,
                    studentId))
            .ReturnsAsync((Assignment?)null);

        // Act
        var result =
            await _service.GetAssignmentForStudentAsync(
                id,
                studentId);

        // Assert
        result.Should().BeNull();
    }

    // =====================================================
    // TEST DATA
    // =====================================================

    private static Assignment CreateAssignment(
        string title)
    {
        return new Assignment
        {
            Id = Guid.NewGuid(),

            TeacherId = "teacher-001",

            AcademicYearId = Guid.NewGuid(),

            ClassId = Guid.NewGuid(),

            SectionId = Guid.NewGuid(),

            CourseId = Guid.NewGuid(),

            SubjectId = Guid.NewGuid(),

            Title = title,

            Description = "Test assignment",

            Deadline = DateTime.UtcNow.AddDays(7),

            MaximumMarks = 100,

            AttachmentUrl = null,

            AttachmentFileName = null,

            AttachmentContentType = null,

            AttachmentFileSize = null,

            IsPublished = true,

            PublishedAt = DateTime.UtcNow,

            IsActive = true,

            CreatedAt = DateTime.UtcNow,

            CreatedBy = "test",

            UpdatedAt = null,

            UpdatedBy = null,

            Submissions = new List<AssignmentSubmission>()
        };
    }
}