using Akkhor.API.Controllers;
using Akkhor.Application.DTOs;
using Akkhor.Application.Interfaces;
using Akkhor.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using Xunit;

namespace Akkhor.API.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<ILogger<AuthController>> _loggerMock;
    private readonly Mock<RoleManager<Roles>> _roleManagerMock;

    public AuthControllerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();

        _tokenServiceMock = new Mock<ITokenService>();

        _loggerMock = new Mock<ILogger<AuthController>>();

        var roleStoreMock = new Mock<IRoleStore<Roles>>();

        _roleManagerMock = new Mock<RoleManager<Roles>>(
            roleStoreMock.Object,
            null!,
            null!,
            null!,
            null!
        );
    }

    // =====================================================
    // LOGIN
    // =====================================================

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsOk()
    {
        // Arrange
        var user = CreateUser(
            "teacher@test.com",
            "Test Teacher",
            true
        );

        var roles = new List<string>
        {
            "Teacher"
        };

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync("teacher@test.com"))
            .ReturnsAsync(user);

        _userRepositoryMock
            .Setup(x => x.CheckPasswordAsync(
                user,
                "Password123!"))
            .ReturnsAsync(true);

        _userRepositoryMock
            .Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(roles);

        _tokenServiceMock
            .Setup(x => x.CreateToken(
                user,
                It.IsAny<IList<string>>()))
            .Returns((
                "test-jwt-token",
                DateTime.UtcNow.AddHours(1)
            ));

        var controller = CreateController();

        var dto = new LoginDto
        {
            Email = "teacher@test.com",
            Password = "Password123!"
        };

        // Act
        var result = await controller.Login(dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.NotNull(okResult.Value);

        _userRepositoryMock.Verify(
            x => x.GetByEmailAsync("teacher@test.com"),
            Times.Once
        );

        _userRepositoryMock.Verify(
            x => x.CheckPasswordAsync(
                user,
                "Password123!"),
            Times.Once
        );

        _userRepositoryMock.Verify(
            x => x.GetRolesAsync(user),
            Times.Once
        );

        _tokenServiceMock.Verify(
            x => x.CreateToken(
                user,
                It.Is<IList<string>>(
                    r => r.Contains("Teacher"))),
            Times.Once
        );
    }

    [Fact]
    public async Task Login_WithInvalidEmail_ReturnsUnauthorized()
    {
        // Arrange
        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync("wrong@test.com"))
            .ReturnsAsync((Users?)null);

        var controller = CreateController();

        var dto = new LoginDto
        {
            Email = "wrong@test.com",
            Password = "Password123!"
        };

        // Act
        var result = await controller.Login(dto);

        // Assert
        Assert.IsType<UnauthorizedObjectResult>(result);

        _tokenServiceMock.Verify(
            x => x.CreateToken(
                It.IsAny<Users>(),
                It.IsAny<IList<string>>()),
            Times.Never
        );
    }

    [Fact]
    public async Task Login_WithInvalidPassword_ReturnsUnauthorized()
    {
        // Arrange
        var user = CreateUser(
            "teacher@test.com",
            "Test Teacher",
            true
        );

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync("teacher@test.com"))
            .ReturnsAsync(user);

        _userRepositoryMock
            .Setup(x => x.CheckPasswordAsync(
                user,
                "WrongPassword"))
            .ReturnsAsync(false);

        var controller = CreateController();

        var dto = new LoginDto
        {
            Email = "teacher@test.com",
            Password = "WrongPassword"
        };

        // Act
        var result = await controller.Login(dto);

        // Assert
        Assert.IsType<UnauthorizedObjectResult>(result);

        _tokenServiceMock.Verify(
            x => x.CreateToken(
                It.IsAny<Users>(),
                It.IsAny<IList<string>>()),
            Times.Never
        );
    }

    [Fact]
    public async Task Login_WithInactiveUser_ReturnsUnauthorized()
    {
        // Arrange
        var user = CreateUser(
            "inactive@test.com",
            "Inactive User",
            false
        );

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync("inactive@test.com"))
            .ReturnsAsync(user);

        _userRepositoryMock
            .Setup(x => x.CheckPasswordAsync(
                user,
                "Password123!"))
            .ReturnsAsync(true);

        var controller = CreateController();

        var dto = new LoginDto
        {
            Email = "inactive@test.com",
            Password = "Password123!"
        };

        // Act
        var result = await controller.Login(dto);

        // Assert
        Assert.IsType<UnauthorizedObjectResult>(result);

        _userRepositoryMock.Verify(
            x => x.GetRolesAsync(It.IsAny<Users>()),
            Times.Never
        );

        _tokenServiceMock.Verify(
            x => x.CreateToken(
                It.IsAny<Users>(),
                It.IsAny<IList<string>>()),
            Times.Never
        );
    }

    [Fact]
    public async Task Login_WithValidCredentials_GeneratesJwt()
    {
        // Arrange
        var user = CreateUser(
            "admin@test.com",
            "Test Admin",
            true
        );

        var roles = new List<string>
        {
            "Admin"
        };

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync("admin@test.com"))
            .ReturnsAsync(user);

        _userRepositoryMock
            .Setup(x => x.CheckPasswordAsync(
                user,
                "Password123!"))
            .ReturnsAsync(true);

        _userRepositoryMock
            .Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(roles);

        _tokenServiceMock
            .Setup(x => x.CreateToken(
                user,
                It.IsAny<IList<string>>()))
            .Returns((
                "test-jwt-token",
                DateTime.UtcNow.AddHours(1)
            ));

        var controller = CreateController();

        var dto = new LoginDto
        {
            Email = "admin@test.com",
            Password = "Password123!"
        };

        // Act
        var result = await controller.Login(dto);

        // Assert
        Assert.IsType<OkObjectResult>(result);

        _tokenServiceMock.Verify(
            x => x.CreateToken(
                user,
                It.Is<IList<string>>(
                    r => r.Contains("Admin"))),
            Times.Once
        );
    }

    [Fact]
    public async Task Login_WhenRepositoryThrows_ReturnsInternalServerError()
    {
        // Arrange
        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync("error@test.com"))
            .ThrowsAsync(new Exception("Database error"));

        var controller = CreateController();

        var dto = new LoginDto
        {
            Email = "error@test.com",
            Password = "Password123!"
        };

        // Act
        var result = await controller.Login(dto);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);

        Assert.Equal(500, objectResult.StatusCode);
        Assert.NotNull(objectResult.Value);
    }

    // =====================================================
    // REGISTER
    // =====================================================

    [Fact]
    public async Task Register_WithNewEmail_ReturnsOk()
    {
        // Arrange
        _userRepositoryMock
            .Setup(x => x.EmailExistsAsync("newuser@test.com"))
            .ReturnsAsync(false);

        _userRepositoryMock
            .Setup(x => x.CreateAsync(
                It.IsAny<Users>(),
                "Password123!"))
            .ReturnsAsync((Users user, string password) =>
            {
                user.Id = "new-user-id";
                return user;
            });

        _roleManagerMock
            .Setup(x => x.RoleExistsAsync("Normal User"))
            .ReturnsAsync(true);

        _userRepositoryMock
            .Setup(x => x.AddRoleAsync(
                It.IsAny<Users>(),
                "Normal User"))
            .Returns(Task.CompletedTask);

        var controller = CreateController();

        var dto = new RegisterDto
        {
            Email = "newuser@test.com",
            FullName = "New User",
            Password = "Password123!"
        };

        // Act
        var result = await controller.Register(dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.NotNull(okResult.Value);

        _userRepositoryMock.Verify(
            x => x.EmailExistsAsync("newuser@test.com"),
            Times.Once
        );

        _userRepositoryMock.Verify(
            x => x.CreateAsync(
                It.Is<Users>(u =>
                    u.Email == "newuser@test.com" &&
                    u.FullName == "New User" &&
                    u.IsActive),
                "Password123!"),
            Times.Once
        );

        _userRepositoryMock.Verify(
            x => x.AddRoleAsync(
                It.Is<Users>(u =>
                    u.Email == "newuser@test.com" &&
                    u.FullName == "New User"),
                "Normal User"),
            Times.Once
        );
    }

    [Fact]
    public async Task Register_WithExistingEmail_ReturnsConflict()
    {
        // Arrange
        _userRepositoryMock
            .Setup(x => x.EmailExistsAsync("existing@test.com"))
            .ReturnsAsync(true);

        var controller = CreateController();

        var dto = new RegisterDto
        {
            Email = "existing@test.com",
            FullName = "Existing User",
            Password = "Password123!"
        };

        // Act
        var result = await controller.Register(dto);

        // Assert
        Assert.IsType<ConflictObjectResult>(result);

        _userRepositoryMock.Verify(
            x => x.CreateAsync(
                It.IsAny<Users>(),
                It.IsAny<string>()),
            Times.Never
        );

        _userRepositoryMock.Verify(
            x => x.AddRoleAsync(
                It.IsAny<Users>(),
                It.IsAny<string>()),
            Times.Never
        );
    }

    [Fact]
    public async Task Register_WhenNormalUserRoleExists_AssignsRole()
    {
        // Arrange
        _userRepositoryMock
            .Setup(x => x.EmailExistsAsync("student@test.com"))
            .ReturnsAsync(false);

        _userRepositoryMock
            .Setup(x => x.CreateAsync(
                It.IsAny<Users>(),
                "Password123!"))
            .ReturnsAsync((Users user, string password) =>
            {
                user.Id = "student-user-id";
                return user;
            });

        _roleManagerMock
            .Setup(x => x.RoleExistsAsync("Normal User"))
            .ReturnsAsync(true);

        _userRepositoryMock
            .Setup(x => x.AddRoleAsync(
                It.IsAny<Users>(),
                "Normal User"))
            .Returns(Task.CompletedTask);

        var controller = CreateController();

        var dto = new RegisterDto
        {
            Email = "student@test.com",
            FullName = "Test Student",
            Password = "Password123!"
        };

        // Act
        var result = await controller.Register(dto);

        // Assert
        Assert.IsType<OkObjectResult>(result);

        _roleManagerMock.Verify(
            x => x.RoleExistsAsync("Normal User"),
            Times.Once
        );

        _userRepositoryMock.Verify(
            x => x.AddRoleAsync(
                It.Is<Users>(u =>
                    u.Email == "student@test.com" &&
                    u.FullName == "Test Student"),
                "Normal User"),
            Times.Once
        );
    }

    [Fact]
    public async Task Register_WhenNormalUserRoleDoesNotExist_DoesNotAssignRole()
    {
        // Arrange
        _userRepositoryMock
            .Setup(x => x.EmailExistsAsync("user@test.com"))
            .ReturnsAsync(false);

        _userRepositoryMock
            .Setup(x => x.CreateAsync(
                It.IsAny<Users>(),
                "Password123!"))
            .ReturnsAsync((Users user, string password) =>
            {
                user.Id = "user-id";
                return user;
            });

        _roleManagerMock
            .Setup(x => x.RoleExistsAsync("Normal User"))
            .ReturnsAsync(false);

        var controller = CreateController();

        var dto = new RegisterDto
        {
            Email = "user@test.com",
            FullName = "Test User",
            Password = "Password123!"
        };

        // Act
        var result = await controller.Register(dto);

        // Assert
        Assert.IsType<OkObjectResult>(result);

        _roleManagerMock.Verify(
            x => x.RoleExistsAsync("Normal User"),
            Times.Once
        );

        _userRepositoryMock.Verify(
            x => x.AddRoleAsync(
                It.IsAny<Users>(),
                "Normal User"),
            Times.Never
        );
    }

    [Fact]
    public async Task Register_WhenRepositoryThrows_ReturnsInternalServerError()
    {
        // Arrange
        _userRepositoryMock
            .Setup(x => x.EmailExistsAsync("error@test.com"))
            .ThrowsAsync(new Exception("Database error"));

        var controller = CreateController();

        var dto = new RegisterDto
        {
            Email = "error@test.com",
            FullName = "Error User",
            Password = "Password123!"
        };

        // Act
        var result = await controller.Register(dto);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);

        Assert.Equal(500, objectResult.StatusCode);
        Assert.NotNull(objectResult.Value);
    }

    // =====================================================
    // ME
    // =====================================================

    [Fact]
    public async Task Me_WithoutUserIdClaim_ReturnsUnauthorized()
    {
        // Arrange
        var controller = CreateController();

        var identity = new ClaimsIdentity();

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(identity)
            }
        };

        // Act
        var result = await controller.Me();

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task Me_WhenUserDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var userId = "missing-user-id";

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync((Users?)null);

        var controller = CreateControllerWithUser(userId);

        // Act
        var result = await controller.Me();

        // Assert
        Assert.IsType<NotFoundResult>(result);

        _userRepositoryMock.Verify(
            x => x.GetByIdAsync(userId),
            Times.Once
        );

        _userRepositoryMock.Verify(
            x => x.GetRolesAsync(It.IsAny<Users>()),
            Times.Never
        );
    }

    [Fact]
    public async Task Me_WithValidUser_ReturnsOk()
    {
        // Arrange
        var userId = "test-user-id";

        var user = CreateUser(
            "teacher@test.com",
            "Test Teacher",
            true
        );

        user.Id = userId;

        var roles = new List<string>
        {
            "Teacher"
        };

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(user);

        _userRepositoryMock
            .Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(roles);

        var controller = CreateControllerWithUser(userId);

        // Act
        var result = await controller.Me();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.NotNull(okResult.Value);

        _userRepositoryMock.Verify(
            x => x.GetByIdAsync(userId),
            Times.Once
        );

        _userRepositoryMock.Verify(
            x => x.GetRolesAsync(user),
            Times.Once
        );
    }

    [Fact]
    public async Task Me_WithValidUser_ReturnsCorrectUserInformation()
    {
        // Arrange
        var userId = "test-user-id";

        var user = CreateUser(
            "teacher@test.com",
            "Test Teacher",
            true
        );

        user.Id = userId;

        var roles = new List<string>
        {
            "Teacher"
        };

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(user);

        _userRepositoryMock
            .Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(roles);

        var controller = CreateControllerWithUser(userId);

        // Act
        var result = await controller.Me();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.NotNull(okResult.Value);

        var value = okResult.Value!;

        var idProperty = value
            .GetType()
            .GetProperty("Id");

        var fullNameProperty = value
            .GetType()
            .GetProperty("FullName");

        var emailProperty = value
            .GetType()
            .GetProperty("Email");

        var isActiveProperty = value
            .GetType()
            .GetProperty("IsActive");

        var roleProperty = value
            .GetType()
            .GetProperty("role");

        Assert.Equal(
            user.Id,
            idProperty?.GetValue(value)
        );

        Assert.Equal(
            user.FullName,
            fullNameProperty?.GetValue(value)
        );

        Assert.Equal(
            user.Email,
            emailProperty?.GetValue(value)
        );

        Assert.Equal(
            user.IsActive,
            isActiveProperty?.GetValue(value)
        );

        Assert.Equal(
            "Teacher",
            roleProperty?.GetValue(value)
        );
    }

    [Fact]
    public async Task Me_WhenRepositoryThrows_ReturnsInternalServerError()
    {
        // Arrange
        var userId = "error-user-id";

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId))
            .ThrowsAsync(new Exception("Database error"));

        var controller = CreateControllerWithUser(userId);

        // Act
        var result = await controller.Me();

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);

        Assert.Equal(500, objectResult.StatusCode);
        Assert.NotNull(objectResult.Value);
    }

    // =====================================================
    // HELPERS
    // =====================================================

    private AuthController CreateController()
    {
        return new AuthController(
            _userRepositoryMock.Object,
            _tokenServiceMock.Object,
            _loggerMock.Object,
            _roleManagerMock.Object
        );
    }

    private AuthController CreateControllerWithUser(
        string userId)
    {
        var controller = CreateController();

        var claims = new List<Claim>
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                userId)
        };

        var identity = new ClaimsIdentity(
            claims,
            "TestAuthentication"
        );

        var principal = new ClaimsPrincipal(identity);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = principal
            }
        };

        return controller;
    }

    private static Users CreateUser(
        string email,
        string fullName,
        bool isActive)
    {
        return new Users
        {
            Id = Guid.NewGuid().ToString(),
            UserName = email,
            Email = email,
            FullName = fullName,
            EmailConfirmed = true,
            IsActive = isActive,
            CreatedAt = DateTime.UtcNow
        };
    }
}