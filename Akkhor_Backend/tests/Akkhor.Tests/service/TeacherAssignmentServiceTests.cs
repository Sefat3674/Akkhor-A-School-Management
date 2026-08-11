using Akkhor.Application.DTOs.TeacherAssignments;
using Akkhor.Application.Interfaces.Repositories;
using Akkhor.Application.Services;
using Akkhor.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Akkhor.Tests.Services;

public class TeacherAssignmentServiceTests
{
    private readonly Mock<ITeacherAssignmentRepository> _repositoryMock;
    private readonly TeacherAssignmentService _service;

    public TeacherAssignmentServiceTests()
    {
        _repositoryMock =
            new Mock<ITeacherAssignmentRepository>();

        _service =
            new TeacherAssignmentService(
                _repositoryMock.Object);
    }


    // =====================================================
    // GET ALL
    // =====================================================

    [Fact]
    public async Task GetAllAsync_ShouldReturnAssignments()
    {
        var assignmentId = Guid.NewGuid();
        var academicYearId = Guid.NewGuid();
        var classId = Guid.NewGuid();
        var sectionId = Guid.NewGuid();
        var courseId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();

        var teacher = new Users
        {
            Id = "teacher-1",
            UserName = "teacher@test.com",
            FullName = "Test Teacher"
        };

        var academicYear = new AcademicYear
        {
            Id = academicYearId,
            Name = "2026"
        };

        var @class = new Class
        {
            Id = classId,
            Name = "Class 10"
        };

        var section = new ClassSection
        {
            Id = sectionId,
            ClassId = classId,
            SectionName = "A"
        };

        var course = new Course
        {
            Id = courseId,
            ClassId = classId,
            CourseName = "Science"
        };

        var subject = new Subject
        {
            Id = subjectId,
            Name = "Physics"
        };

        var assignment = new TeacherAssignment
        {
            Id = assignmentId,

            TeacherId = teacher.Id,
            Teacher = teacher,

            AcademicYearId = academicYearId,
            AcademicYear = academicYear,

            ClassId = classId,
            Class = @class,

            SectionId = sectionId,
            Section = section,

            CourseId = courseId,
            Course = course,

            SubjectId = subjectId,
            Subject = subject,

            IsPrimary = true,
            IsActive = true
        };

        _repositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(
                new List<TeacherAssignment>
                {
                    assignment
                });

        var result =
            await _service.GetAllAsync();

        result.Should().HaveCount(1);

        var dto = result.First();

        dto.Id.Should().Be(assignmentId);
        dto.TeacherId.Should().Be("teacher-1");
        dto.TeacherName.Should().Be("Test Teacher");
        dto.AcademicYearId.Should().Be(academicYearId);
        dto.AcademicYearName.Should().Be("2026");
        dto.ClassId.Should().Be(classId);
        dto.ClassName.Should().Be("Class 10");
        dto.SectionId.Should().Be(sectionId);
        dto.SectionName.Should().Be("A");
        dto.CourseId.Should().Be(courseId);
        dto.CourseName.Should().Be("Science");
        dto.SubjectId.Should().Be(subjectId);
        dto.SubjectName.Should().Be("Physics");
        dto.IsPrimary.Should().BeTrue();
        dto.IsActive.Should().BeTrue();
    }


    // =====================================================
    // GET BY ID
    // =====================================================

    [Fact]
    public async Task GetByIdAsync_ShouldReturnAssignment_WhenExists()
    {
        var id = Guid.NewGuid();

        var assignment = new TeacherAssignment
        {
            Id = id,
            TeacherId = "teacher-1",
            Teacher = new Users
            {
                Id = "teacher-1",
                FullName = "Test Teacher",
                UserName = "teacher@test.com"
            },
            AcademicYearId = Guid.NewGuid(),
            AcademicYear = new AcademicYear
            {
                Id = Guid.NewGuid(),
                Name = "2026"
            },
            ClassId = Guid.NewGuid(),
            Class = new Class
            {
                Id = Guid.NewGuid(),
                Name = "Class 10"
            },
            CourseId = Guid.NewGuid(),
            Course = new Course
            {
                Id = Guid.NewGuid(),
                CourseName = "Science"
            },
            SubjectId = Guid.NewGuid(),
            Subject = new Subject
            {
                Id = Guid.NewGuid(),
                Name = "Physics"
            }
        };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(assignment);

        var result =
            await _service.GetByIdAsync(id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.TeacherId.Should().Be("teacher-1");
        result.TeacherName.Should().Be("Test Teacher");
    }


    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync((TeacherAssignment?)null);

        var result =
            await _service.GetByIdAsync(id);

        result.Should().BeNull();
    }


    // =====================================================
    // CREATE
    // =====================================================

    [Fact]
    public async Task CreateAsync_ShouldCreateAssignment_WhenValid()
    {
        var dto = new CreateTeacherAssignmentDto
        {
            TeacherId = "teacher-1",
            AcademicYearId = Guid.NewGuid(),
            ClassId = Guid.NewGuid(),
            SectionId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            SubjectId = Guid.NewGuid(),
            IsPrimary = true,
            IsActive = true,
            CreatedBy = "admin"
        };

        _repositoryMock
            .Setup(x => x.ExistsAsync(
                dto.TeacherId,
                dto.AcademicYearId,
                dto.ClassId,
                dto.SectionId,
                dto.CourseId,
                dto.SubjectId,
                null))
            .ReturnsAsync(false);

        _repositoryMock
            .Setup(x => x.CreateAsync(
                It.IsAny<TeacherAssignment>()))
            .ReturnsAsync(
                (TeacherAssignment entity) => entity);

        var result =
            await _service.CreateAsync(dto);

        result.Should().NotBeNull();

        result.TeacherId
            .Should()
            .Be(dto.TeacherId);

        result.AcademicYearId
            .Should()
            .Be(dto.AcademicYearId);

        result.ClassId
            .Should()
            .Be(dto.ClassId);

        result.SectionId
            .Should()
            .Be(dto.SectionId);

        result.CourseId
            .Should()
            .Be(dto.CourseId);

        result.SubjectId
            .Should()
            .Be(dto.SubjectId);

        result.IsPrimary
            .Should()
            .BeTrue();

        result.IsActive
            .Should()
            .BeTrue();

        _repositoryMock.Verify(
            x => x.CreateAsync(
                It.Is<TeacherAssignment>(
                    a =>
                        a.TeacherId == dto.TeacherId &&
                        a.AcademicYearId == dto.AcademicYearId &&
                        a.ClassId == dto.ClassId &&
                        a.SectionId == dto.SectionId &&
                        a.CourseId == dto.CourseId &&
                        a.SubjectId == dto.SubjectId)),
            Times.Once);
    }


    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenTeacherIdIsEmpty()
    {
        var dto = new CreateTeacherAssignmentDto
        {
            TeacherId = "",
            AcademicYearId = Guid.NewGuid(),
            ClassId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            SubjectId = Guid.NewGuid()
        };

        var act = async () =>
            await _service.CreateAsync(dto);

        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Teacher is required.");
    }


    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenAcademicYearIdIsEmpty()
    {
        var dto = new CreateTeacherAssignmentDto
        {
            TeacherId = "teacher-1",
            AcademicYearId = Guid.Empty,
            ClassId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            SubjectId = Guid.NewGuid()
        };

        var act = async () =>
            await _service.CreateAsync(dto);

        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Academic year is required.");
    }


    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenClassIdIsEmpty()
    {
        var dto = new CreateTeacherAssignmentDto
        {
            TeacherId = "teacher-1",
            AcademicYearId = Guid.NewGuid(),
            ClassId = Guid.Empty,
            CourseId = Guid.NewGuid(),
            SubjectId = Guid.NewGuid()
        };

        var act = async () =>
            await _service.CreateAsync(dto);

        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Class is required.");
    }


    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenCourseIdIsEmpty()
    {
        var dto = new CreateTeacherAssignmentDto
        {
            TeacherId = "teacher-1",
            AcademicYearId = Guid.NewGuid(),
            ClassId = Guid.NewGuid(),
            CourseId = Guid.Empty,
            SubjectId = Guid.NewGuid()
        };

        var act = async () =>
            await _service.CreateAsync(dto);

        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Course is required.");
    }


    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenSubjectIdIsEmpty()
    {
        var dto = new CreateTeacherAssignmentDto
        {
            TeacherId = "teacher-1",
            AcademicYearId = Guid.NewGuid(),
            ClassId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            SubjectId = Guid.Empty
        };

        var act = async () =>
            await _service.CreateAsync(dto);

        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Subject is required.");
    }


    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenDuplicateAssignmentExists()
    {
        var dto = new CreateTeacherAssignmentDto
        {
            TeacherId = "teacher-1",
            AcademicYearId = Guid.NewGuid(),
            ClassId = Guid.NewGuid(),
            SectionId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            SubjectId = Guid.NewGuid()
        };

        _repositoryMock
            .Setup(x => x.ExistsAsync(
                dto.TeacherId,
                dto.AcademicYearId,
                dto.ClassId,
                dto.SectionId,
                dto.CourseId,
                dto.SubjectId,
                null))
            .ReturnsAsync(true);

        var act = async () =>
            await _service.CreateAsync(dto);

        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage(
                "This teacher is already assigned to the selected class, section, course and subject.");

        _repositoryMock.Verify(
            x => x.CreateAsync(
                It.IsAny<TeacherAssignment>()),
            Times.Never);
    }


    // =====================================================
    // UPDATE
    // =====================================================

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedAssignment_WhenValid()
    {
        var id = Guid.NewGuid();

        var existing = new TeacherAssignment
        {
            Id = id,
            TeacherId = "old-teacher",
            AcademicYearId = Guid.NewGuid(),
            ClassId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            SubjectId = Guid.NewGuid()
        };

        var dto = new UpdateTeacherAssignmentDto
        {
            TeacherId = "new-teacher",
            AcademicYearId = Guid.NewGuid(),
            ClassId = Guid.NewGuid(),
            SectionId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            SubjectId = Guid.NewGuid(),
            IsPrimary = true,
            IsActive = true,
            UpdatedBy = "admin"
        };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(existing);

        _repositoryMock
            .Setup(x => x.ExistsAsync(
                dto.TeacherId,
                dto.AcademicYearId,
                dto.ClassId,
                dto.SectionId,
                dto.CourseId,
                dto.SubjectId,
                id))
            .ReturnsAsync(false);

        _repositoryMock
            .Setup(x => x.UpdateAsync(
                It.IsAny<TeacherAssignment>()))
            .ReturnsAsync(
                (TeacherAssignment entity) => entity);

        var result =
            await _service.UpdateAsync(id, dto);

        result.Should().NotBeNull();

        result!.Id.Should().Be(id);
        result.TeacherId.Should().Be(dto.TeacherId);
        result.AcademicYearId.Should().Be(dto.AcademicYearId);
        result.ClassId.Should().Be(dto.ClassId);
        result.SectionId.Should().Be(dto.SectionId);
        result.CourseId.Should().Be(dto.CourseId);
        result.SubjectId.Should().Be(dto.SubjectId);
        result.IsPrimary.Should().Be(dto.IsPrimary);
        result.IsActive.Should().Be(dto.IsActive);
        result.UpdatedBy.Should().Be(dto.UpdatedBy);

        _repositoryMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<TeacherAssignment>()),
            Times.Once);
    }


    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenAssignmentNotFound()
    {
        var id = Guid.NewGuid();

        var dto = new UpdateTeacherAssignmentDto
        {
            TeacherId = "teacher-1",
            AcademicYearId = Guid.NewGuid(),
            ClassId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            SubjectId = Guid.NewGuid()
        };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync((TeacherAssignment?)null);

        var result =
            await _service.UpdateAsync(id, dto);

        result.Should().BeNull();

        _repositoryMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<TeacherAssignment>()),
            Times.Never);
    }


    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenDuplicateAssignmentExists()
    {
        var id = Guid.NewGuid();

        var existing = new TeacherAssignment
        {
            Id = id
        };

        var dto = new UpdateTeacherAssignmentDto
        {
            TeacherId = "teacher-1",
            AcademicYearId = Guid.NewGuid(),
            ClassId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            SubjectId = Guid.NewGuid()
        };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(existing);

        _repositoryMock
            .Setup(x => x.ExistsAsync(
                dto.TeacherId,
                dto.AcademicYearId,
                dto.ClassId,
                dto.SectionId,
                dto.CourseId,
                dto.SubjectId,
                id))
            .ReturnsAsync(true);

        var act = async () =>
            await _service.UpdateAsync(id, dto);

        await act.Should()
            .ThrowAsync<InvalidOperationException>();

        _repositoryMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<TeacherAssignment>()),
            Times.Never);
    }


    // =====================================================
    // DELETE
    // =====================================================

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenAssignmentExists()
    {
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(
                new TeacherAssignment
                {
                    Id = id
                });

        _repositoryMock
            .Setup(x => x.DeleteAsync(id))
            .ReturnsAsync(true);

        var result =
            await _service.DeleteAsync(id);

        result.Should().BeTrue();

        _repositoryMock.Verify(
            x => x.DeleteAsync(id),
            Times.Once);
    }


    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenAssignmentNotFound()
    {
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync((TeacherAssignment?)null);

        var result =
            await _service.DeleteAsync(id);

        result.Should().BeFalse();

        _repositoryMock.Verify(
            x => x.DeleteAsync(id),
            Times.Never);
    }
}