using Akkhor.Application.Interfaces;
using Akkhor.Application.Interfaces.Repositories;
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
    options.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "Akkhor API",
            Version = "v1"
        });


    options.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",

            Type = SecuritySchemeType.Http,

            Scheme = "Bearer",

            BearerFormat = "JWT",

            In = ParameterLocation.Header,

            Description =
            "Enter JWT Token: Bearer {token}"
        });


    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference =
                    new OpenApiReference
                    {
                        Type =
                        ReferenceType.SecurityScheme,

                        Id = "Bearer"
                    }
                },

                Array.Empty<string>()
            }
        });
});




// =====================================================
// Database PostgreSQL
// =====================================================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration
        .GetConnectionString("DefaultConnection"));
});





// =====================================================
// Identity
// =====================================================

builder.Services
    .AddIdentity<Users, Roles>(options =>
    {

        options.Password.RequireDigit = true;

        options.Password.RequiredLength = 8;

        options.Password.RequireUppercase = true;

        options.Password.RequireLowercase = true;

        options.Password.RequireNonAlphanumeric = false;


        options.User.RequireUniqueEmail = true;

    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();






// =====================================================
// JWT Authentication
// =====================================================

var jwtSettings =
    builder.Configuration.GetSection("Jwt");


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
                        Encoding.UTF8.GetBytes(
                            jwtSettings["Key"]!
                        ))
            };

    });




builder.Services.AddAuthorization();





// =====================================================
// Dependency Injection
// =====================================================

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserManagementService, UserManagementService>();
builder.Services.AddScoped<IAcademicYearRepository, AcademicYearRepository>();





// =====================================================
// CORS Angular
// =====================================================

var allowedOrigins =
    builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularClient",
        policy =>
        {
            policy
            .WithOrigins(
                allowedOrigins ?? Array.Empty<string>()
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});







// =====================================================
// Build App
// =====================================================

var app = builder.Build();





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


app.MapControllers();


app.Run();