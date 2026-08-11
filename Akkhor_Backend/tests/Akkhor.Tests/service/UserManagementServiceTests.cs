using Akkhor.Application.DTOs.UserManagement;
using Akkhor.Application.Services;
using Akkhor.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Akkhor.Tests.Services;

public class UserManagementServiceTests
{
    private readonly Mock<UserManager<Users>> _userManagerMock;
    private readonly Mock<RoleManager<Roles>> _roleManagerMock;
    private readonly UserManagementService _service;

    public UserManagementServiceTests()
    {
        var userStore =
            new Mock<IUserStore<Users>>();

        _userManagerMock =
            new Mock<UserManager<Users>>(
                userStore.Object,
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<Users>>(),
                Array.Empty<IUserValidator<Users>>(),
                Array.Empty<IPasswordValidator<Users>>(),
                Mock.Of<ILookupNormalizer>(),
                new IdentityErrorDescriber(),
                Mock.Of<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<Users>>>());

        var roleStore =
            new Mock<IRoleStore<Roles>>();

        _roleManagerMock =
            new Mock<RoleManager<Roles>>(
                roleStore.Object,
                Array.Empty<IRoleValidator<Roles>>(),
                Mock.Of<ILookupNormalizer>(),
                new IdentityErrorDescriber(),
                Mock.Of<ILogger<RoleManager<Roles>>>());

        _service =
            new UserManagementService(
                _userManagerMock.Object,
                _roleManagerMock.Object);
    }


    // =====================================================
    // GET USER BY ID
    // =====================================================

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUser_WhenExists()
    {
        var user = new Users
        {
            Id = "user-1",
            UserName = "john@test.com",
            Email = "john@test.com",
            FullName = "John Doe",
            PhoneNumber = "01700000000",
            IsActive = true
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync("user-1"))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(
                new List<string>
                {
                    "Student"
                });

        var result =
            await _service.GetUserByIdAsync("user-1");

        result.Should().NotBeNull();

        result!.Id.Should().Be("user-1");
        result.FullName.Should().Be("John Doe");
        result.Email.Should().Be("john@test.com");
        result.PhoneNumber.Should().Be("01700000000");
        result.IsActive.Should().BeTrue();

        result.Roles
            .Should()
            .ContainSingle()
            .Which
            .Should()
            .Be("Student");
    }


    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        _userManagerMock
            .Setup(x => x.FindByIdAsync("missing"))
            .ReturnsAsync((Users?)null);

        var result =
            await _service.GetUserByIdAsync("missing");

        result.Should().BeNull();
    }


    // =====================================================
    // CREATE USER
    // =====================================================

    [Fact]
    public async Task CreateUserAsync_ShouldCreateUser_WhenValid()
    {
        var dto = new CreateUserDto
        {
            FullName = "John Doe",
            Email = "john@test.com",
            Password = "Password123!",
            Role = "Student"
        };

        _userManagerMock
            .Setup(x => x.CreateAsync(
                It.IsAny<Users>(),
                dto.Password))
            .ReturnsAsync(
                IdentityResult.Success);

        _userManagerMock
            .Setup(x => x.AddToRoleAsync(
                It.IsAny<Users>(),
                dto.Role))
            .ReturnsAsync(
                IdentityResult.Success);

        var result =
            await _service.CreateUserAsync(dto);

        result.Should().BeTrue();

        _userManagerMock.Verify(
            x => x.CreateAsync(
                It.Is<Users>(u =>
                    u.UserName == dto.Email &&
                    u.Email == dto.Email &&
                    u.FullName == dto.FullName &&
                    u.IsActive),
                dto.Password),
            Times.Once);

        _userManagerMock.Verify(
            x => x.AddToRoleAsync(
                It.IsAny<Users>(),
                dto.Role),
            Times.Once);
    }


    [Fact]
    public async Task CreateUserAsync_ShouldReturnFalse_WhenCreateFails()
    {
        var dto = new CreateUserDto
        {
            FullName = "John Doe",
            Email = "john@test.com",
            Password = "Password123!",
            Role = "Student"
        };

        _userManagerMock
            .Setup(x => x.CreateAsync(
                It.IsAny<Users>(),
                dto.Password))
            .ReturnsAsync(
                IdentityResult.Failed(
                    new IdentityError
                    {
                        Description = "Creation failed"
                    }));

        var result =
            await _service.CreateUserAsync(dto);

        result.Should().BeFalse();

        _userManagerMock.Verify(
            x => x.AddToRoleAsync(
                It.IsAny<Users>(),
                It.IsAny<string>()),
            Times.Never);
    }


    [Fact]
    public async Task CreateUserAsync_ShouldCreateUserWithoutRole()
    {
        var dto = new CreateUserDto
        {
            FullName = "John Doe",
            Email = "john@test.com",
            Password = "Password123!",
            Role = null
        };

        _userManagerMock
            .Setup(x => x.CreateAsync(
                It.IsAny<Users>(),
                dto.Password))
            .ReturnsAsync(
                IdentityResult.Success);

        var result =
            await _service.CreateUserAsync(dto);

        result.Should().BeTrue();

        _userManagerMock.Verify(
            x => x.AddToRoleAsync(
                It.IsAny<Users>(),
                It.IsAny<string>()),
            Times.Never);
    }


    // =====================================================
    // UPDATE USER
    // =====================================================

    [Fact]
    public async Task UpdateUserAsync_ShouldReturnFalse_WhenUserNotFound()
    {
        _userManagerMock
            .Setup(x => x.FindByIdAsync("missing"))
            .ReturnsAsync((Users?)null);

        var dto = new UpdateUserDto
        {
            FullName = "Updated User",
            PhoneNumber = "01800000000",
            IsActive = true,
            Role = "Student"
        };

        var result =
            await _service.UpdateUserAsync(
                "missing",
                dto);

        result.Should().BeFalse();
    }


    [Fact]
    public async Task UpdateUserAsync_ShouldUpdateBasicInformation()
    {
        var user = new Users
        {
            Id = "user-1",
            FullName = "Old Name",
            PhoneNumber = "01700000000",
            IsActive = true
        };

        var dto = new UpdateUserDto
        {
            FullName = "New Name",
            PhoneNumber = "01800000000",
            IsActive = false
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync("user-1"))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(x => x.UpdateAsync(user))
            .ReturnsAsync(
                IdentityResult.Success);

        var result =
            await _service.UpdateUserAsync(
                "user-1",
                dto);

        result.Should().BeTrue();

        user.FullName.Should().Be("New Name");
        user.PhoneNumber.Should().Be("01800000000");
        user.IsActive.Should().BeFalse();

        _userManagerMock.Verify(
            x => x.UpdateAsync(user),
            Times.Once);
    }


    [Fact]
    public async Task UpdateUserAsync_ShouldReturnFalse_WhenUpdateFails()
    {
        var user = new Users
        {
            Id = "user-1"
        };

        var dto = new UpdateUserDto
        {
            FullName = "Updated",
            PhoneNumber = "01800000000",
            IsActive = true
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync("user-1"))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(x => x.UpdateAsync(user))
            .ReturnsAsync(
                IdentityResult.Failed(
                    new IdentityError
                    {
                        Description = "Update failed"
                    }));

        var result =
            await _service.UpdateUserAsync(
                "user-1",
                dto);

        result.Should().BeFalse();
    }


    [Fact]
    public async Task UpdateUserAsync_ShouldUpdateRole_WhenRoleIsProvided()
    {
        var user = new Users
        {
            Id = "user-1",
            FullName = "John"
        };

        var dto = new UpdateUserDto
        {
            FullName = "John Updated",
            PhoneNumber = "01800000000",
            IsActive = true,
            Role = "Teacher"
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync("user-1"))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(x => x.UpdateAsync(user))
            .ReturnsAsync(
                IdentityResult.Success);

        _userManagerMock
            .Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(
                new List<string>
                {
                    "Student"
                });

        _userManagerMock
            .Setup(x => x.RemoveFromRolesAsync(
                user,
                It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(
                IdentityResult.Success);

        _roleManagerMock
            .Setup(x => x.RoleExistsAsync("Teacher"))
            .ReturnsAsync(true);

        _userManagerMock
            .Setup(x => x.AddToRoleAsync(
                user,
                "Teacher"))
            .ReturnsAsync(
                IdentityResult.Success);

        var result =
            await _service.UpdateUserAsync(
                "user-1",
                dto);

        result.Should().BeTrue();

        _userManagerMock.Verify(
            x => x.RemoveFromRolesAsync(
                user,
                It.Is<IEnumerable<string>>(
                    roles => roles.Contains("Student"))),
            Times.Once);

        _roleManagerMock.Verify(
            x => x.RoleExistsAsync("Teacher"),
            Times.Once);

        _userManagerMock.Verify(
            x => x.AddToRoleAsync(
                user,
                "Teacher"),
            Times.Once);
    }


    [Fact]
    public async Task UpdateUserAsync_ShouldReturnFalse_WhenRoleDoesNotExist()
    {
        var user = new Users
        {
            Id = "user-1"
        };

        var dto = new UpdateUserDto
        {
            FullName = "John",
            PhoneNumber = "01800000000",
            IsActive = true,
            Role = "InvalidRole"
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync("user-1"))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(x => x.UpdateAsync(user))
            .ReturnsAsync(
                IdentityResult.Success);

        _userManagerMock
            .Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(
                new List<string>
                {
                    "Student"
                });

        _userManagerMock
            .Setup(x => x.RemoveFromRolesAsync(
                user,
                It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(
                IdentityResult.Success);

        _roleManagerMock
            .Setup(x => x.RoleExistsAsync("InvalidRole"))
            .ReturnsAsync(false);

        var result =
            await _service.UpdateUserAsync(
                "user-1",
                dto);

        result.Should().BeFalse();

        _userManagerMock.Verify(
            x => x.AddToRoleAsync(
                user,
                "InvalidRole"),
            Times.Never);
    }


    // =====================================================
    // DELETE USER
    // =====================================================

    [Fact]
    public async Task DeleteUserAsync_ShouldReturnFalse_WhenUserNotFound()
    {
        _userManagerMock
            .Setup(x => x.FindByIdAsync("missing"))
            .ReturnsAsync((Users?)null);

        var result =
            await _service.DeleteUserAsync("missing");

        result.Should().BeFalse();
    }


    [Fact]
    public async Task DeleteUserAsync_ShouldDeleteUser_WhenValid()
    {
        var user = new Users
        {
            Id = "user-1"
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync("user-1"))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(
                new List<string>
                {
                    "Student"
                });

        _userManagerMock
            .Setup(x => x.RemoveFromRolesAsync(
                user,
                It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(
                IdentityResult.Success);

        _userManagerMock
            .Setup(x => x.DeleteAsync(user))
            .ReturnsAsync(
                IdentityResult.Success);

        var result =
            await _service.DeleteUserAsync("user-1");

        result.Should().BeTrue();

        _userManagerMock.Verify(
            x => x.RemoveFromRolesAsync(
                user,
                It.IsAny<IEnumerable<string>>()),
            Times.Once);

        _userManagerMock.Verify(
            x => x.DeleteAsync(user),
            Times.Once);
    }


    [Fact]
    public async Task DeleteUserAsync_ShouldReturnFalse_WhenRemovingRolesFails()
    {
        var user = new Users
        {
            Id = "user-1"
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync("user-1"))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(
                new List<string>
                {
                    "Student"
                });

        _userManagerMock
            .Setup(x => x.RemoveFromRolesAsync(
                user,
                It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(
                IdentityResult.Failed());

        var result =
            await _service.DeleteUserAsync("user-1");

        result.Should().BeFalse();

        _userManagerMock.Verify(
            x => x.DeleteAsync(user),
            Times.Never);
    }


    // =====================================================
    // GET USER ROLES
    // =====================================================

    [Fact]
    public async Task GetUserRolesAsync_ShouldReturnRoles()
    {
        var user = new Users
        {
            Id = "user-1"
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync("user-1"))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(
                new List<string>
                {
                    "Admin",
                    "Teacher"
                });

        var result =
            await _service.GetUserRolesAsync("user-1");

        result.Should().HaveCount(2);
        result.Should().Contain("Admin");
        result.Should().Contain("Teacher");
    }


    [Fact]
    public async Task GetUserRolesAsync_ShouldReturnEmpty_WhenUserNotFound()
    {
        _userManagerMock
            .Setup(x => x.FindByIdAsync("missing"))
            .ReturnsAsync((Users?)null);

        var result =
            await _service.GetUserRolesAsync("missing");

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }


    // =====================================================
    // ASSIGN ROLE
    // =====================================================

    [Fact]
    public async Task AssignRoleAsync_ShouldReturnFalse_WhenUserNotFound()
    {
        var dto = new AssignRoleDto
        {
            UserId = "missing",
            Role = "Student"
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync(dto.UserId))
            .ReturnsAsync((Users?)null);

        var result =
            await _service.AssignRoleAsync(dto);

        result.Should().BeFalse();
    }


    [Fact]
    public async Task AssignRoleAsync_ShouldReturnFalse_WhenRoleDoesNotExist()
    {
        var user = new Users
        {
            Id = "user-1"
        };

        var dto = new AssignRoleDto
        {
            UserId = "user-1",
            Role = "InvalidRole"
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync(dto.UserId))
            .ReturnsAsync(user);

        _roleManagerMock
            .Setup(x => x.RoleExistsAsync(dto.Role))
            .ReturnsAsync(false);

        var result =
            await _service.AssignRoleAsync(dto);

        result.Should().BeFalse();

        _userManagerMock.Verify(
            x => x.AddToRoleAsync(
                It.IsAny<Users>(),
                dto.Role),
            Times.Never);
    }


    [Fact]
    public async Task AssignRoleAsync_ShouldReturnTrue_WhenValid()
    {
        var user = new Users
        {
            Id = "user-1"
        };

        var dto = new AssignRoleDto
        {
            UserId = "user-1",
            Role = "Teacher"
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync(dto.UserId))
            .ReturnsAsync(user);

        _roleManagerMock
            .Setup(x => x.RoleExistsAsync(dto.Role))
            .ReturnsAsync(true);

        _userManagerMock
            .Setup(x => x.AddToRoleAsync(
                user,
                dto.Role))
            .ReturnsAsync(
                IdentityResult.Success);

        var result =
            await _service.AssignRoleAsync(dto);

        result.Should().BeTrue();

        _userManagerMock.Verify(
            x => x.AddToRoleAsync(
                user,
                dto.Role),
            Times.Once);
    }


    [Fact]
    public async Task AssignRoleAsync_ShouldReturnFalse_WhenAddRoleFails()
    {
        var user = new Users
        {
            Id = "user-1"
        };

        var dto = new AssignRoleDto
        {
            UserId = "user-1",
            Role = "Teacher"
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync(dto.UserId))
            .ReturnsAsync(user);

        _roleManagerMock
            .Setup(x => x.RoleExistsAsync(dto.Role))
            .ReturnsAsync(true);

        _userManagerMock
            .Setup(x => x.AddToRoleAsync(
                user,
                dto.Role))
            .ReturnsAsync(
                IdentityResult.Failed());

        var result =
            await _service.AssignRoleAsync(dto);

        result.Should().BeFalse();
    }


    // =====================================================
    // RESET PASSWORD
    // =====================================================

    [Fact]
    public async Task ResetPasswordAsync_ShouldReturnFalse_WhenUserNotFound()
    {
        _userManagerMock
            .Setup(x => x.FindByIdAsync("missing"))
            .ReturnsAsync((Users?)null);

        var result =
            await _service.ResetPasswordAsync(
                "missing",
                "NewPassword123!");

        result.Should().BeFalse();
    }


    [Fact]
    public async Task ResetPasswordAsync_ShouldReturnTrue_WhenValid()
    {
        var user = new Users
        {
            Id = "user-1"
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync("user-1"))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(x =>
                x.GeneratePasswordResetTokenAsync(user))
            .ReturnsAsync("reset-token");

        _userManagerMock
            .Setup(x => x.ResetPasswordAsync(
                user,
                "reset-token",
                "NewPassword123!"))
            .ReturnsAsync(
                IdentityResult.Success);

        var result =
            await _service.ResetPasswordAsync(
                "user-1",
                "NewPassword123!");

        result.Should().BeTrue();

        _userManagerMock.Verify(
            x => x.GeneratePasswordResetTokenAsync(user),
            Times.Once);

        _userManagerMock.Verify(
            x => x.ResetPasswordAsync(
                user,
                "reset-token",
                "NewPassword123!"),
            Times.Once);
    }


    [Fact]
    public async Task ResetPasswordAsync_ShouldReturnFalse_WhenResetFails()
    {
        var user = new Users
        {
            Id = "user-1"
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync("user-1"))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(x =>
                x.GeneratePasswordResetTokenAsync(user))
            .ReturnsAsync("reset-token");

        _userManagerMock
            .Setup(x => x.ResetPasswordAsync(
                user,
                "reset-token",
                "NewPassword123!"))
            .ReturnsAsync(
                IdentityResult.Failed());

        var result =
            await _service.ResetPasswordAsync(
                "user-1",
                "NewPassword123!");

        result.Should().BeFalse();
    }


    // =====================================================
    // UPDATE USER STATUS
    // =====================================================

    [Fact]
    public async Task UpdateUserStatusAsync_ShouldReturnFalse_WhenUserNotFound()
    {
        _userManagerMock
            .Setup(x => x.FindByIdAsync("missing"))
            .ReturnsAsync((Users?)null);

        var result =
            await _service.UpdateUserStatusAsync(
                "missing",
                false);

        result.Should().BeFalse();
    }


    [Fact]
    public async Task UpdateUserStatusAsync_ShouldUpdateStatus()
    {
        var user = new Users
        {
            Id = "user-1",
            IsActive = true
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync("user-1"))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(x => x.UpdateAsync(user))
            .ReturnsAsync(
                IdentityResult.Success);

        var result =
            await _service.UpdateUserStatusAsync(
                "user-1",
                false);

        result.Should().BeTrue();

        user.IsActive.Should().BeFalse();

        _userManagerMock.Verify(
            x => x.UpdateAsync(user),
            Times.Once);
    }


    [Fact]
    public async Task UpdateUserStatusAsync_ShouldReturnFalse_WhenUpdateFails()
    {
        var user = new Users
        {
            Id = "user-1",
            IsActive = true
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync("user-1"))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(x => x.UpdateAsync(user))
            .ReturnsAsync(
                IdentityResult.Failed());

        var result =
            await _service.UpdateUserStatusAsync(
                "user-1",
                false);

        result.Should().BeFalse();

        user.IsActive.Should().BeFalse();
    }
}