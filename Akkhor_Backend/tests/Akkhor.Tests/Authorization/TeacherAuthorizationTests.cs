using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net;

namespace Akkhor.Tests.Authorization;

public class TeacherAuthorizationTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public TeacherAuthorizationTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    private HttpClient CreateAuthenticatedClient(
        string role)
    {
        var factory =
            _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll<
                        IAuthenticationSchemeProvider>();

                    services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme =
                            TestAuthenticationHandler.SchemeName;

                        options.DefaultChallengeScheme =
                            TestAuthenticationHandler.SchemeName;

                        options.DefaultScheme =
                            TestAuthenticationHandler.SchemeName;
                    })
                    .AddScheme<
                        AuthenticationSchemeOptions,
                        TestAuthenticationHandler>(
                        TestAuthenticationHandler.SchemeName,
                        _ => { });
                });
            });

        var client = factory.CreateClient();

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers
                .AuthenticationHeaderValue(
                    TestAuthenticationHandler.SchemeName);

        client.DefaultRequestHeaders.Add(
            "X-Test-Role",
            role);

        client.DefaultRequestHeaders.Add(
            "X-Test-UserId",
            "test-teacher-user-id");

        return client;
    }

    // =====================================================
    // NO AUTHENTICATION
    // =====================================================

    [Fact]
    public async Task Assignments_WithoutAuthentication_Returns401()
    {
        var client = _factory.CreateClient();

        var response =
            await client.GetAsync(
                "/api/assignments");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task TeacherClasses_WithoutAuthentication_Returns401()
    {
        var client = _factory.CreateClient();

        var response =
            await client.GetAsync(
                "/api/teacher-classes/my-classes");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task TeacherDashboard_WithoutAuthentication_Returns401()
    {
        var client = _factory.CreateClient();

        var response =
            await client.GetAsync(
                "/api/teacher-dashboard");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Unauthorized);
    }

    // =====================================================
    // STUDENT
    // =====================================================

    [Fact]
    public async Task Assignments_AsStudent_Returns403()
    {
        var client =
            CreateAuthenticatedClient("Student");

        var response =
            await client.GetAsync(
                "/api/assignments");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task TeacherClasses_AsStudent_Returns403()
    {
        var client =
            CreateAuthenticatedClient("Student");

        var response =
            await client.GetAsync(
                "/api/teacher-classes/my-classes");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task TeacherDashboard_AsStudent_Returns403()
    {
        var client =
            CreateAuthenticatedClient("Student");

        var response =
            await client.GetAsync(
                "/api/teacher-dashboard");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Forbidden);
    }

    // =====================================================
    // ADMIN
    // =====================================================

    [Fact]
    public async Task Assignments_AsAdmin_Returns403()
    {
        var client =
            CreateAuthenticatedClient("Admin");

        var response =
            await client.GetAsync(
                "/api/assignments");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task TeacherClasses_AsAdmin_Returns403()
    {
        var client =
            CreateAuthenticatedClient("Admin");

        var response =
            await client.GetAsync(
                "/api/teacher-classes/my-classes");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task TeacherDashboard_AsAdmin_Returns403()
    {
        var client =
            CreateAuthenticatedClient("Admin");

        var response =
            await client.GetAsync(
                "/api/teacher-dashboard");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Forbidden);
    }

    // =====================================================
    // SUPER ADMIN
    // =====================================================

    [Fact]
    public async Task TeacherDashboard_AsSuperAdmin_Returns403()
    {
        var client =
            CreateAuthenticatedClient("SuperAdmin");

        var response =
            await client.GetAsync(
                "/api/teacher-dashboard");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Forbidden);
    }

    // =====================================================
    // TEACHER
    // =====================================================

    [Fact]
    public async Task Assignments_AsTeacher_AllowsAccess()
    {
        var client =
            CreateAuthenticatedClient("Teacher");

        var response =
            await client.GetAsync(
                "/api/assignments");

        response.StatusCode
            .Should()
            .NotBe(HttpStatusCode.Unauthorized);

        response.StatusCode
            .Should()
            .NotBe(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task TeacherClasses_AsTeacher_AllowsAccess()
    {
        var client =
            CreateAuthenticatedClient("Teacher");

        var response =
            await client.GetAsync(
                "/api/teacher-classes/my-classes");

        response.StatusCode
            .Should()
            .NotBe(HttpStatusCode.Unauthorized);

        response.StatusCode
            .Should()
            .NotBe(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task TeacherDashboard_AsTeacher_AllowsAccess()
    {
        var client =
            CreateAuthenticatedClient("Teacher");

        var response =
            await client.GetAsync(
                "/api/teacher-dashboard");

        response.StatusCode
            .Should()
            .NotBe(HttpStatusCode.Unauthorized);

        response.StatusCode
            .Should()
            .NotBe(HttpStatusCode.Forbidden);
    }
}