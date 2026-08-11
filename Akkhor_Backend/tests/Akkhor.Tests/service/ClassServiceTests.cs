using Akkhor.Application.DTOs.Classes;
using Akkhor.Application.Interfaces.Repositories;
using Akkhor.Application.Services;
using Akkhor.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Akkhor.Tests.Services;

public class ClassServiceTests
{
    private readonly Mock<IClassRepository> _repositoryMock;
    private readonly ClassService _service;

    public ClassServiceTests()
    {
        _repositoryMock = new Mock<IClassRepository>();
        _service = new ClassService(_repositoryMock.Object);
    }


    // =====================================================
    // GET ALL
    // =====================================================

    [Fact]
    public async Task GetAllAsync_ShouldReturnClasses()
    {
        var academicYearId = Guid.NewGuid();

        var classes = new List<Class>
        {
            new Class
            {
                Id = Guid.NewGuid(),
                AcademicYearId = academicYearId,
                AcademicYear = new AcademicYear
                {
                    Id = academicYearId,
                    Name = "2026"
                },
                Name = "Class 10",
                Code = "CLS10",
                Description = "Class Ten",
                DisplayOrder = 1,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Sections = new List<ClassSection>
                {
                    new ClassSection(),
                    new ClassSection()
                }
            }
        };

        _repositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(classes);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(1);

        var item = result.First();

        item.Name.Should().Be("Class 10");
        item.Code.Should().Be("CLS10");
        item.AcademicYearName.Should().Be("2026");
        item.SectionCount.Should().Be(2);
        item.IsActive.Should().BeTrue();

        _repositoryMock.Verify(
            x => x.GetAllAsync(),
            Times.Once);
    }


    // =====================================================
    // GET BY ID - FOUND
    // =====================================================

    [Fact]
    public async Task GetByIdAsync_ShouldReturnClass_WhenExists()
    {
        var id = Guid.NewGuid();
        var academicYearId = Guid.NewGuid();

        var entity = new Class
        {
            Id = id,
            AcademicYearId = academicYearId,
            AcademicYear = new AcademicYear
            {
                Id = academicYearId,
                Name = "2026"
            },
            Name = "Class 9",
            Code = "CLS09",
            Description = "Class Nine",
            DisplayOrder = 2,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            Sections = new List<ClassSection>
            {
                new ClassSection()
            }
        };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(entity);

        var result = await _service.GetByIdAsync(id);

        result.Should().NotBeNull();

        result!.Id.Should().Be(id);
        result.Name.Should().Be("Class 9");
        result.Code.Should().Be("CLS09");
        result.AcademicYearName.Should().Be("2026");
        result.SectionCount.Should().Be(1);
    }


    // =====================================================
    // GET BY ID - NOT FOUND
    // =====================================================

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync((Class?)null);

        var result = await _service.GetByIdAsync(id);

        result.Should().BeNull();

        _repositoryMock.Verify(
            x => x.GetByIdAsync(id),
            Times.Once);
    }


    // =====================================================
    // CREATE - SUCCESS
    // =====================================================

    [Fact]
    public async Task CreateAsync_ShouldCreateClass_WhenCodeDoesNotExist()
    {
        var academicYearId = Guid.NewGuid();

        var dto = new CreateClassDto
        {
            AcademicYearId = academicYearId,
            Name = "Class 8",
            Code = "CLS08",
            Description = "Class Eight",
            DisplayOrder = 3
        };

        _repositoryMock
            .Setup(x => x.GetByCodeAsync(dto.Code))
            .ReturnsAsync((Class?)null);

        Class? createdEntity = null;

        _repositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Class>()))
            .Callback<Class>(x => createdEntity = x)
            .Returns(Task.CompletedTask);

        _repositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() =>
                createdEntity);

        var result = await _service.CreateAsync(dto);

        result.Should().NotBeNull();

        result.Name.Should().Be("Class 8");
        result.Code.Should().Be("CLS08");
        result.AcademicYearId.Should().Be(academicYearId);
        result.IsActive.Should().BeTrue();

        createdEntity.Should().NotBeNull();

        _repositoryMock.Verify(
            x => x.GetByCodeAsync("CLS08"),
            Times.Once);

        _repositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Class>()),
            Times.Once);
    }


    // =====================================================
    // CREATE - DUPLICATE CODE
    // =====================================================

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenCodeAlreadyExists()
    {
        var existingClass = new Class
        {
            Id = Guid.NewGuid(),
            Name = "Existing Class",
            Code = "CLS10"
        };

        var dto = new CreateClassDto
        {
            AcademicYearId = Guid.NewGuid(),
            Name = "New Class",
            Code = "CLS10",
            Description = "Duplicate",
            DisplayOrder = 1
        };

        _repositoryMock
            .Setup(x => x.GetByCodeAsync(dto.Code))
            .ReturnsAsync(existingClass);

        var act = async () =>
            await _service.CreateAsync(dto);

        await act.Should()
            .ThrowAsync<Exception>()
            .WithMessage("Class code already exists");

        _repositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Class>()),
            Times.Never);
    }


    // =====================================================
    // CREATE - CREATED ENTITY NOT FOUND
    // =====================================================

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenCreatedClassCannotBeFound()
    {
        var dto = new CreateClassDto
        {
            AcademicYearId = Guid.NewGuid(),
            Name = "Class 7",
            Code = "CLS07",
            Description = "Class Seven",
            DisplayOrder = 4
        };

        _repositoryMock
            .Setup(x => x.GetByCodeAsync(dto.Code))
            .ReturnsAsync((Class?)null);

        _repositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Class>()))
            .Returns(Task.CompletedTask);

        _repositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Class?)null);

        var act = async () =>
            await _service.CreateAsync(dto);

        await act.Should()
            .ThrowAsync<Exception>()
            .WithMessage("Unable to create class");
    }


    // =====================================================
    // UPDATE - SUCCESS
    // =====================================================

    [Fact]
    public async Task UpdateAsync_ShouldReturnTrue_WhenClassExists()
    {
        var id = Guid.NewGuid();

        var entity = new Class
        {
            Id = id,
            AcademicYearId = Guid.NewGuid(),
            Name = "Old Name",
            Code = "OLD01",
            Description = "Old Description",
            DisplayOrder = 1,
            IsActive = true,
            CreatedAt = DateTime.UtcNow.AddDays(-1)
        };

        var dto = new UpdateClassDto
        {
            Name = "Updated Class",
            Code = "UPD01",
            Description = "Updated Description",
            DisplayOrder = 5,
            IsActive = false
        };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(entity);

        _repositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<Class>()))
            .Returns(Task.CompletedTask);

        var result = await _service.UpdateAsync(id, dto);

        result.Should().BeTrue();

        entity.Name.Should().Be("Updated Class");
        entity.Code.Should().Be("UPD01");
        entity.Description.Should().Be("Updated Description");
        entity.DisplayOrder.Should().Be(5);
        entity.IsActive.Should().BeFalse();

        _repositoryMock.Verify(
            x => x.UpdateAsync(entity),
            Times.Once);
    }


    // =====================================================
    // UPDATE - NOT FOUND
    // =====================================================

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenClassNotFound()
    {
        var id = Guid.NewGuid();

        var dto = new UpdateClassDto
        {
            Name = "Updated Class",
            Code = "UPD01",
            Description = "Updated",
            DisplayOrder = 1,
            IsActive = true
        };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync((Class?)null);

        var result = await _service.UpdateAsync(id, dto);

        result.Should().BeFalse();

        _repositoryMock.Verify(
            x => x.UpdateAsync(It.IsAny<Class>()),
            Times.Never);
    }


    // =====================================================
    // DELETE - SUCCESS
    // =====================================================

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenClassExists()
    {
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(x => x.ExistsAsync(id))
            .ReturnsAsync(true);

        _repositoryMock
            .Setup(x => x.DeleteAsync(id))
            .Returns(Task.CompletedTask);

        var result = await _service.DeleteAsync(id);

        result.Should().BeTrue();

        _repositoryMock.Verify(
            x => x.ExistsAsync(id),
            Times.Once);

        _repositoryMock.Verify(
            x => x.DeleteAsync(id),
            Times.Once);
    }


    // =====================================================
    // DELETE - NOT FOUND
    // =====================================================

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenClassNotFound()
    {
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(x => x.ExistsAsync(id))
            .ReturnsAsync(false);

        var result = await _service.DeleteAsync(id);

        result.Should().BeFalse();

        _repositoryMock.Verify(
            x => x.DeleteAsync(id),
            Times.Never);
    }
}