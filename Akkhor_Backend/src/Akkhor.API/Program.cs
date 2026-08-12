using Akkhor.Application.Interfaces;
using Akkhor.Application.Interfaces.Repositories;
using Akkhor.Application.Interfaces.Services;
using Akkhor.Application.Services;
using Akkhor.Domain.Entities;
using Akkhor.Infrastructure.Data;
using Akkhor.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =====================================================
// Controllers
// =====================================================

builder.Services.AddControllers();


// =====================================================
// Swagger
// =====================================================

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "Akkhor API",
            Version = "v1"
        });

    // -------------------------------------------------
    // JWT Authentication in Swagger
    // -------------------------------------------------

    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",

            Type = SecuritySchemeType.Http,

            Scheme = "Bearer",

            BearerFormat = "JWT",

            In = ParameterLocation.Header,

            Description = "Enter JWT Token: Bearer {token}"
        });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },

                Array.Empty<string>()
            }
        });
});


// =====================================================
// Database - PostgreSQL
// =====================================================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString(
            "DefaultConnection"));
});


// =====================================================
// Identity
// =====================================================

builder.Services
    .AddIdentity<Users, Roles>(options =>
    {
        // -------------------------------------------------
        // Password settings
        // -------------------------------------------------

        options.Password.RequireDigit = true;

        options.Password.RequiredLength = 8;

        options.Password.RequireUppercase = true;

        options.Password.RequireLowercase = true;

        options.Password.RequireNonAlphanumeric = false;


        // -------------------------------------------------
        // User settings
        // -------------------------------------------------

        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// =====================================================
// JWT Authentication
// =====================================================

var jwtSettings =
    builder.Configuration.GetSection("Jwt");

var jwtKey = jwtSettings["Key"];

if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException(
        "JWT Key is not configured. " +
        "Please configure Jwt:Key in appsettings.json or environment variables.");
}

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;

        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,

                ValidateAudience = true,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,

                ValidIssuer =
                    jwtSettings["Issuer"],

                ValidAudience =
                    jwtSettings["Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey))
            };
    });


// =====================================================
// Authorization
// =====================================================

builder.Services.AddAuthorization();


// =====================================================
// Dependency Injection
// =====================================================

// -----------------------------------------------------
// Authentication / User
// -----------------------------------------------------

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IUserManagementService, UserManagementService>();


// -----------------------------------------------------
// Academic Year
// -----------------------------------------------------

builder.Services.AddScoped<
    IAcademicYearRepository,
    AcademicYearRepository>();


// -----------------------------------------------------
// Class
// -----------------------------------------------------

builder.Services.AddScoped<
    IClassRepository,
    ClassRepository>();

builder.Services.AddScoped<
    IClassService,
    ClassService>();


// -----------------------------------------------------
// Section
// -----------------------------------------------------

builder.Services.AddScoped<
    ISectionRepository,
    SectionRepository>();

builder.Services.AddScoped<
    ISectionService,
    SectionService>();


// -----------------------------------------------------
// Course
// -----------------------------------------------------

builder.Services.AddScoped<
    ICourseRepository,
    CourseRepository>();

builder.Services.AddScoped<
    ICourseService,
    CourseService>();


// -----------------------------------------------------
// Subject
// -----------------------------------------------------

builder.Services.AddScoped<
    ISubjectRepository,
    SubjectRepository>();

builder.Services.AddScoped<
    ISubjectService,
    SubjectService>();


// -----------------------------------------------------
// Course Subject
// -----------------------------------------------------

builder.Services.AddScoped<
    ICourseSubjectRepository,
    CourseSubjectRepository>();

builder.Services.AddScoped<
    ICourseSubjectService,
    CourseSubjectService>();


// -----------------------------------------------------
// Student Enrollment
// -----------------------------------------------------

builder.Services.AddScoped<
    IStudentEnrollmentRepository,
    StudentEnrollmentRepository>();

builder.Services.AddScoped<
    IStudentEnrollmentService,
    StudentEnrollmentService>();


// -----------------------------------------------------
// Teacher Assignment
// -----------------------------------------------------

builder.Services.AddScoped<
    ITeacherAssignmentRepository,
    TeacherAssignmentRepository>();

builder.Services.AddScoped<
    ITeacherAssignmentService,
    TeacherAssignmentService>();


// -----------------------------------------------------
// Teacher Class
// -----------------------------------------------------

builder.Services.AddScoped<
    ITeacherClassRepository,
    TeacherClassRepository>();

builder.Services.AddScoped<
    ITeacherClassService,
    TeacherClassService>();


// -----------------------------------------------------
// Assignment
// -----------------------------------------------------

builder.Services.AddScoped<
    IAssignmentRepository,
    AssignmentRepository>();

builder.Services.AddScoped<
    IAssignmentService,
    AssignmentService>();


// -----------------------------------------------------
// Assignment Submission
// -----------------------------------------------------

builder.Services.AddScoped<
    IAssignmentSubmissionRepository,
    AssignmentSubmissionRepository>();

builder.Services.AddScoped<
    IAssignmentSubmissionService,
    AssignmentSubmissionService>();


// -----------------------------------------------------
// Student Dashboard
// -----------------------------------------------------

builder.Services.AddScoped<
    IStudentDashboardService,
    StudentDashboardService>();


// -----------------------------------------------------
// Application Settings
// -----------------------------------------------------

builder.Services.AddScoped<
    IApplicationSettingRepository,
    ApplicationSettingRepository>();

builder.Services.AddScoped<
    IApplicationSettingService,
    ApplicationSettingService>();


// =====================================================
// CORS - Angular
// =====================================================

var allowedOrigins =
    builder.Configuration
        .GetSection("Cors:AllowedOrigins")
        .Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AngularClient",
        policy =>
        {
            policy
                .WithOrigins(
                    allowedOrigins ?? Array.Empty<string>())
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});


// =====================================================
// Build Application
// =====================================================

var app = builder.Build();


// =====================================================
// Database / Demo Data Seeding
// =====================================================

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        // -------------------------------------------------
        // User Manager
        // -------------------------------------------------

        var userManager =
            services.GetRequiredService<UserManager<Users>>();


        // -------------------------------------------------
        // Role Manager
        // -------------------------------------------------

        var roleManager =
            services.GetRequiredService<RoleManager<Roles>>();


        // -------------------------------------------------
        // Seed Roles + Demo Users
        // -------------------------------------------------

        await DbSeeder.SeedAsync(
            userManager,
            roleManager);
    }
    catch (Exception ex)
    {
        var logger =
            services.GetRequiredService<
                ILogger<Program>>();

        logger.LogError(
            ex,
            "An error occurred while seeding demo users and roles.");
    }
}


// =====================================================
// Swagger
// =====================================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}


// =====================================================
// Middleware
// =====================================================

app.UseHttpsRedirection();

app.UseCors("AngularClient");

app.UseAuthentication();

app.UseAuthorization();


// =====================================================
// Controllers
// =====================================================

app.MapControllers();


// =====================================================
// Run Application
// =====================================================

app.Run();


// =====================================================
// Program Class
// =====================================================

public partial class Program
{
}