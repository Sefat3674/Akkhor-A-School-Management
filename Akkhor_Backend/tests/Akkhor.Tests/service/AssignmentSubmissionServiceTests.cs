
using Akkhor.Application.DTOs.Assignments;
using Akkhor.Application.Interfaces.Repositories;
using Akkhor.Application.Services;
using Akkhor.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Akkhor.Tests.Services;

public class AssignmentSubmissionServiceTests
{
    private readonly Mock<IAssignmentSubmissionRepository> _repositoryMock;
    private readonly AssignmentSubmissionService _service;

    public AssignmentSubmissionServiceTests()
    {
        _repositoryMock =
            new Mock<IAssignmentSubmissionRepository>();

        _service =
            new AssignmentSubmissionService(
                _repositoryMock.Object);
    }


    // =====================================================
    // GET BY ID
    // =====================================================

    [Fact]
    public async Task GetByIdAsync_ShouldReturnSubmission_WhenFound()
    {
        // Arrange
        var submissionId = Guid.NewGuid();

        var submission = new AssignmentSubmission
        {
            Id = submissionId,
            AssignmentId = Guid.NewGuid(),
            StudentId = "student-001",
            SubmissionText = "My submission",
            Status = "Submitted",
            CreatedAt = DateTime.UtcNow
        };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(submissionId))
            .ReturnsAsync(submission);

        // Act
        var result =
            await _service.GetByIdAsync(submissionId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(submissionId);
        result.StudentId.Should().Be("student-001");
        result.SubmissionText.Should().Be("My submission");

        _repositoryMock.Verify(
            x => x.GetByIdAsync(submissionId),
            Times.Once);
    }


    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        var submissionId = Guid.NewGuid();

        _repositoryMock
            .Setup(x => x.GetByIdAsync(submissionId))
            .ReturnsAsync((AssignmentSubmission?)null);

        // Act
        var result =
            await _service.GetByIdAsync(submissionId);

        // Assert
        result.Should().BeNull();
    }


    // =====================================================
    // GET MY SUBMISSIONS
    // =====================================================

    [Fact]
    public async Task GetMySubmissionsAsync_ShouldThrow_WhenStudentIdIsEmpty()
    {
        // Act
        Func<Task> act = async () =>
            await _service.GetMySubmissionsAsync("");

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Student ID is required.");
    }


    [Fact]
    public async Task GetMySubmissionsAsync_ShouldReturnStudentSubmissions()
    {
        // Arrange
        var submissions = new List<AssignmentSubmission>
        {
            new()
            {
                Id = Guid.NewGuid(),
                AssignmentId = Guid.NewGuid(),
                StudentId = "student-001",
                SubmissionText = "Submission 1",
                Status = "Submitted",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                AssignmentId = Guid.NewGuid(),
                StudentId = "student-001",
                SubmissionText = "Submission 2",
                Status = "Submitted",
                CreatedAt = DateTime.UtcNow
            }
        };

        _repositoryMock
            .Setup(x => x.GetByStudentIdAsync("student-001"))
            .ReturnsAsync(submissions);

        // Act
        var result =
            (await _service.GetMySubmissionsAsync("student-001"))
            .ToList();

        // Assert
        result.Should().HaveCount(2);
        result.All(x => x.StudentId == "student-001")
            .Should().BeTrue();
    }


    // =====================================================
    // CREATE / SUBMIT
    // =====================================================

    [Fact]
    public async Task CreateAsync_ShouldCreateSubmission_WhenValid()
    {
        // Arrange
        var assignmentId = Guid.NewGuid();

        var dto = new CreateAssignmentSubmissionDto
        {
            AssignmentId = assignmentId,
            SubmissionText = "  My assignment answer  "
        };

        _repositoryMock
            .Setup(x =>
                x.GetByAssignmentAndStudentAsync(
                    assignmentId,
                    "student-001"))
            .ReturnsAsync((AssignmentSubmission?)null);

        _repositoryMock
            .Setup(x => x.CreateAsync(
                It.IsAny<AssignmentSubmission>()))
            .ReturnsAsync((AssignmentSubmission submission) =>
                submission);

        // Act
        var result =
            await _service.CreateAsync(
                dto,
                "student-001");

        // Assert
        result.Should().NotBeNull();
        result.AssignmentId.Should().Be(assignmentId);
        result.StudentId.Should().Be("student-001");
        result.SubmissionText.Should().Be("My assignment answer");
        result.Status.Should().Be("Submitted");

        _repositoryMock.Verify(
            x => x.CreateAsync(
                It.Is<AssignmentSubmission>(s =>
                    s.AssignmentId == assignmentId &&
                    s.StudentId == "student-001" &&
                    s.SubmissionText == "My assignment answer" &&
                    s.Status == "Submitted")),
            Times.Once);
    }


    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenStudentIdIsEmpty()
    {
        // Arrange
        var dto = new CreateAssignmentSubmissionDto
        {
            AssignmentId = Guid.NewGuid(),
            SubmissionText = "Answer"
        };

        // Act
        Func<Task> act = async () =>
            await _service.CreateAsync(dto, "");

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Student ID is required.");
    }


    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenAssignmentIdIsEmpty()
    {
        // Arrange
        var dto = new CreateAssignmentSubmissionDto
        {
            AssignmentId = Guid.Empty,
            SubmissionText = "Answer"
        };

        // Act
        Func<Task> act = async () =>
            await _service.CreateAsync(
                dto,
                "student-001");

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Assignment ID is required.");
    }


    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenNoTextAndNoAttachment()
    {
        // Arrange
        var dto = new CreateAssignmentSubmissionDto
        {
            AssignmentId = Guid.NewGuid()
        };

        // Act
        Func<Task> act = async () =>
            await _service.CreateAsync(
                dto,
                "student-001");

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage(
                "Submission text or attachment is required.");
    }


    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenAlreadySubmitted()
    {
        // Arrange
        var assignmentId = Guid.NewGuid();

        var existingSubmission =
            new AssignmentSubmission
            {
                Id = Guid.NewGuid(),
                AssignmentId = assignmentId,
                StudentId = "student-001",
                Status = "Submitted"
            };

        var dto =
            new CreateAssignmentSubmissionDto
            {
                AssignmentId = assignmentId,
                SubmissionText = "New answer"
            };

        _repositoryMock
            .Setup(x =>
                x.GetByAssignmentAndStudentAsync(
                    assignmentId,
                    "student-001"))
            .ReturnsAsync(existingSubmission);

        // Act
        Func<Task> act = async () =>
            await _service.CreateAsync(
                dto,
                "student-001");

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage(
                "You have already submitted this assignment.");

        _repositoryMock.Verify(
            x => x.CreateAsync(
                It.IsAny<AssignmentSubmission>()),
            Times.Never);
    }


    // =====================================================
    // UPDATE
    // =====================================================

    [Fact]
    public async Task UpdateAsync_ShouldUpdateSubmission_WhenValid()
    {
        // Arrange
        var submissionId = Guid.NewGuid();

        var submission =
            new AssignmentSubmission
            {
                Id = submissionId,
                AssignmentId = Guid.NewGuid(),
                StudentId = "student-001",
                SubmissionText = "Old answer",
                Status = "Submitted",
                CreatedAt = DateTime.UtcNow
            };

        var dto =
            new UpdateAssignmentSubmissionDto
            {
                SubmissionText = "  Updated answer  "
            };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(submissionId))
            .ReturnsAsync(submission);

        _repositoryMock
            .Setup(x => x.UpdateAsync(
                It.IsAny<AssignmentSubmission>()))
            .ReturnsAsync((AssignmentSubmission s) => s);

        // Act
        var result =
            await _service.UpdateAsync(
                submissionId,
                dto,
                "student-001");

        // Assert
        result.Should().NotBeNull();
        result!.SubmissionText.Should().Be("Updated answer");
        result.Status.Should().Be("Submitted");

        _repositoryMock.Verify(
            x => x.UpdateAsync(
                It.Is<AssignmentSubmission>(s =>
                    s.SubmissionText == "Updated answer" &&
                    s.Status == "Submitted")),
            Times.Once);
    }


    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenSubmissionNotFound()
    {
        // Arrange
        var submissionId = Guid.NewGuid();

        _repositoryMock
            .Setup(x => x.GetByIdAsync(submissionId))
            .ReturnsAsync((AssignmentSubmission?)null);

        var dto =
            new UpdateAssignmentSubmissionDto
            {
                SubmissionText = "Updated"
            };

        // Act
        var result =
            await _service.UpdateAsync(
                submissionId,
                dto,
                "student-001");

        // Assert
        result.Should().BeNull();
    }


    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenStudentIsNotOwner()
    {
        // Arrange
        var submissionId = Guid.NewGuid();

        var submission =
            new AssignmentSubmission
            {
                Id = submissionId,
                StudentId = "student-owner",
                Status = "Submitted"
            };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(submissionId))
            .ReturnsAsync(submission);

        var dto =
            new UpdateAssignmentSubmissionDto
            {
                SubmissionText = "Hacked answer"
            };

        // Act
        Func<Task> act = async () =>
            await _service.UpdateAsync(
                submissionId,
                dto,
                "another-student");

        // Assert
        await act.Should()
            .ThrowAsync<UnauthorizedAccessException>()
            .WithMessage(
                "You are not allowed to update this submission.");

        _repositoryMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<AssignmentSubmission>()),
            Times.Never);
    }


    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenSubmissionIsEvaluated()
    {
        // Arrange
        var submissionId = Guid.NewGuid();

        var submission =
            new AssignmentSubmission
            {
                Id = submissionId,
                StudentId = "student-001",
                Status = "Evaluated"
            };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(submissionId))
            .ReturnsAsync(submission);

        var dto =
            new UpdateAssignmentSubmissionDto
            {
                SubmissionText = "Trying to change"
            };

        // Act
        Func<Task> act = async () =>
            await _service.UpdateAsync(
                submissionId,
                dto,
                "student-001");

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage(
                "An evaluated submission cannot be modified.");
    }


    // =====================================================
    // DELETE
    // =====================================================

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenValid()
    {
        // Arrange
        var submissionId = Guid.NewGuid();

        var submission =
            new AssignmentSubmission
            {
                Id = submissionId,
                StudentId = "student-001",
                Status = "Submitted"
            };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(submissionId))
            .ReturnsAsync(submission);

        _repositoryMock
            .Setup(x => x.DeleteAsync(submissionId))
            .ReturnsAsync(true);

        // Act
        var result =
            await _service.DeleteAsync(
                submissionId,
                "student-001");

        // Assert
        result.Should().BeTrue();

        _repositoryMock.Verify(
            x => x.DeleteAsync(submissionId),
            Times.Once);
    }


    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenNotFound()
    {
        // Arrange
        var submissionId = Guid.NewGuid();

        _repositoryMock
            .Setup(x => x.GetByIdAsync(submissionId))
            .ReturnsAsync((AssignmentSubmission?)null);

        // Act
        var result =
            await _service.DeleteAsync(
                submissionId,
                "student-001");

        // Assert
        result.Should().BeFalse();

        _repositoryMock.Verify(
            x => x.DeleteAsync(It.IsAny<Guid>()),
            Times.Never);
    }


    [Fact]
    public async Task DeleteAsync_ShouldThrow_WhenStudentIsNotOwner()
    {
        // Arrange
        var submissionId = Guid.NewGuid();

        var submission =
            new AssignmentSubmission
            {
                Id = submissionId,
                StudentId = "student-owner",
                Status = "Submitted"
            };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(submissionId))
            .ReturnsAsync(submission);

        // Act
        Func<Task> act = async () =>
            await _service.DeleteAsync(
                submissionId,
                "another-student");

        // Assert
        await act.Should()
            .ThrowAsync<UnauthorizedAccessException>()
            .WithMessage(
                "You are not allowed to delete this submission.");
    }


    [Fact]
    public async Task DeleteAsync_ShouldThrow_WhenSubmissionIsEvaluated()
    {
        // Arrange
        var submissionId = Guid.NewGuid();

        var submission =
            new AssignmentSubmission
            {
                Id = submissionId,
                StudentId = "student-001",
                Status = "Evaluated"
            };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(submissionId))
            .ReturnsAsync(submission);

        // Act
        Func<Task> act = async () =>
            await _service.DeleteAsync(
                submissionId,
                "student-001");

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage(
                "An evaluated submission cannot be deleted.");
    }


    // =====================================================
    // EVALUATE
    // =====================================================

    [Fact]
    public async Task EvaluateAsync_ShouldEvaluateSubmission_WhenValid()
    {
        // Arrange
        var submissionId = Guid.NewGuid();

        var assignment =
            new Assignment
            {
                Id = Guid.NewGuid(),
                MaximumMarks = 100
            };

        var submission =
            new AssignmentSubmission
            {
                Id = submissionId,
                AssignmentId = assignment.Id,
                StudentId = "student-001",
                Status = "Submitted",
                Assignment = assignment
            };

        var dto =
            new EvaluateAssignmentSubmissionDto
            {
                MarksObtained = 85,
                Feedback = "Excellent work"
            };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(submissionId))
            .ReturnsAsync(submission);

        _repositoryMock
            .Setup(x => x.UpdateAsync(
                It.IsAny<AssignmentSubmission>()))
            .ReturnsAsync((AssignmentSubmission s) => s);

        // Act
        var result =
            await _service.EvaluateAsync(
                submissionId,
                dto,
                "teacher-001");

        // Assert
        result.Should().NotBeNull();
        result!.MarksObtained.Should().Be(85);
        result.Feedback.Should().Be("Excellent work");
        result.Status.Should().Be("Evaluated");
        result.GradedBy.Should().Be("teacher-001");

        _repositoryMock.Verify(
            x => x.UpdateAsync(
                It.Is<AssignmentSubmission>(s =>
                    s.Marks == 85 &&
                    s.Status == "Evaluated" &&
                    s.EvaluatedBy == "teacher-001")),
            Times.Once);
    }


    [Fact]
    public async Task EvaluateAsync_ShouldReturnNull_WhenSubmissionNotFound()
    {
        // Arrange
        var submissionId = Guid.NewGuid();

        _repositoryMock
            .Setup(x => x.GetByIdAsync(submissionId))
            .ReturnsAsync((AssignmentSubmission?)null);

        var dto =
            new EvaluateAssignmentSubmissionDto
            {
                MarksObtained = 80
            };

        // Act
        var result =
            await _service.EvaluateAsync(
                submissionId,
                dto,
                "teacher-001");

        // Assert
        result.Should().BeNull();
    }


    [Fact]
    public async Task EvaluateAsync_ShouldThrow_WhenTeacherIdIsEmpty()
    {
        // Arrange
        var dto =
            new EvaluateAssignmentSubmissionDto
            {
                MarksObtained = 80
            };

        // Act
        Func<Task> act = async () =>
            await _service.EvaluateAsync(
                Guid.NewGuid(),
                dto,
                "");

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Teacher ID is required.");
    }


    [Fact]
    public async Task EvaluateAsync_ShouldThrow_WhenMarksAreNegative()
    {
        // Arrange
        var submissionId = Guid.NewGuid();

        var submission =
            new AssignmentSubmission
            {
                Id = submissionId,
                StudentId = "student-001",
                Status = "Submitted"
            };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(submissionId))
            .ReturnsAsync(submission);

        var dto =
            new EvaluateAssignmentSubmissionDto
            {
                MarksObtained = -1
            };

        // Act
        Func<Task> act = async () =>
            await _service.EvaluateAsync(
                submissionId,
                dto,
                "teacher-001");

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Marks cannot be negative.");
    }


    [Fact]
    public async Task EvaluateAsync_ShouldThrow_WhenMarksExceedMaximum()
    {
        // Arrange
        var submissionId = Guid.NewGuid();

        var assignment =
            new Assignment
            {
                Id = Guid.NewGuid(),
                MaximumMarks = 100
            };

        var submission =
            new AssignmentSubmission
            {
                Id = submissionId,
                AssignmentId = assignment.Id,
                StudentId = "student-001",
                Status = "Submitted",
                Assignment = assignment
            };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(submissionId))
            .ReturnsAsync(submission);

        var dto =
            new EvaluateAssignmentSubmissionDto
            {
                MarksObtained = 101
            };

        // Act
        Func<Task> act = async () =>
            await _service.EvaluateAsync(
                submissionId,
                dto,
                "teacher-001");

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage(
                "Obtained marks cannot exceed maximum marks.");

        _repositoryMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<AssignmentSubmission>()),
            Times.Never);
    }


    // =====================================================
    // COUNTS
    // =====================================================

    [Fact]
    public async Task GetSubmissionCountAsync_ShouldReturnCount()
    {
        // Arrange
        var assignmentId = Guid.NewGuid();

        _repositoryMock
            .Setup(x =>
                x.GetSubmissionCountAsync(assignmentId))
            .ReturnsAsync(10);

        // Act
        var result =
            await _service.GetSubmissionCountAsync(
                assignmentId);

        // Assert
        result.Should().Be(10);
    }


    [Fact]
    public async Task GetPendingSubmissionCountAsync_ShouldReturnCount()
    {
        // Arrange
        var assignmentId = Guid.NewGuid();

        _repositoryMock
            .Setup(x =>
                x.GetPendingSubmissionCountAsync(assignmentId))
            .ReturnsAsync(4);

        // Act
        var result =
            await _service.GetPendingSubmissionCountAsync(
                assignmentId);

        // Assert
        result.Should().Be(4);
    }
}
