using Akkhor.Application.DTOs;
using Akkhor.Application.Interfaces;
using Akkhor.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Akkhor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthController> _logger;
    private readonly RoleManager<Roles> _roleManager;


    public AuthController(
        IUserRepository userRepository,
        ITokenService tokenService,
        ILogger<AuthController> logger,
        RoleManager<Roles> roleManager)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _logger = logger;
        _roleManager = roleManager;
    }



    // ==============================
    // REGISTER
    // ==============================

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        try
        {
            var exists =
                await _userRepository
                .EmailExistsAsync(dto.Email);


            if (exists)
            {
                return Conflict(new
                {
                    message = "Email already exists"
                });
            }



            var user = new Users
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
                EmailConfirmed = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };



            await _userRepository.CreateAsync(
                user,
                dto.Password
            );



            // Assign Default Role
            var normalUserRole =
                await _roleManager
                .RoleExistsAsync("Normal User");


            if (normalUserRole)
            {
                await _userRepository.AddRoleAsync(
                    user,
                    "Normal User"
                );
            }



            return Ok(new
            {
                message = "Registration successful",
                userId = user.Id,
                email = user.Email,
                role = "Normal User"
            });

        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Registration failed");


            return StatusCode(500, new
            {
                message = "Server error",
                error = ex.Message
            });
        }
    }





    // ==============================
    // LOGIN
    // ==============================

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        try
        {

            var user =
                await _userRepository
                .GetByEmailAsync(dto.Email);



            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password"
                });
            }



            var passwordValid =
                await _userRepository
                .CheckPasswordAsync(
                    user,
                    dto.Password
                );



            if (!passwordValid)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password"
                });
            }



            if (!user.IsActive)
            {
                return Unauthorized(new
                {
                    message = "Account inactive"
                });
            }



            // Get User Roles
            var roles =
                await _userRepository
                .GetRolesAsync(user);



            var (token, expiresAt) =
                _tokenService.CreateToken(
                    user,
                    roles
                );



            return Ok(new
            {
                token,
                expiresAt,

                userId = user.Id,

                fullName = user.FullName,

                email = user.Email,

                role = roles.FirstOrDefault()
            });

        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Login failed");


            return StatusCode(500, new
            {
                message = "Server error",
                error = ex.Message
            });
        }
    }





    // ==============================
    // CURRENT USER
    // ==============================

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        try
        {
            var userId =
                User.FindFirst(
                    System.Security.Claims.ClaimTypes.NameIdentifier
                )?.Value;



            if (userId == null)
                return Unauthorized();



            var user =
                await _userRepository
                .GetByIdAsync(userId);



            if (user == null)
                return NotFound();



            var roles =
                await _userRepository
                .GetRolesAsync(user);



            return Ok(new
            {
                user.Id,
                user.FullName,
                user.Email,
                user.IsActive,
                role = roles.FirstOrDefault()
            });

        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Current user failed");


            return StatusCode(500, new
            {
                message = "Server error",
                error = ex.Message
            });
        }
    }
}