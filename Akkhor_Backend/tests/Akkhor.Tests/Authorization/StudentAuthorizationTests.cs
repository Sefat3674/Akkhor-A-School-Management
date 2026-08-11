using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net;

namespace Akkhor.Tests.Authorization;

public class StudentAuthorizationTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public StudentAuthorizationTests(
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
            "test-student-user-id");

        return client;
    }

    // =====================================================
    // 401
    // =====================================================

    [Fact]
    public async Task StudentDashboard_WithoutAuthentication_Returns401()
    {
        var client = _factory.CreateClient();

        var response =
            await client.GetAsync(
                "/api/student/dashboard");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task StudentAssignments_WithoutAuthentication_Returns401()
    {
        var client = _factory.CreateClient();

        var response =
            await client.GetAsync(
                "/api/student-assignments");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task AssignmentSubmissions_WithoutAuthentication_Returns401()
    {
        var client = _factory.CreateClient();

        var response =
            await client.GetAsync(
                "/api/assignment-submissions");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Unauthorized);
    }

    // =====================================================
    // ADMIN
    // =====================================================

    [Fact]
    public async Task StudentDashboard_AsAdmin_Returns403()
    {
        var client =
            CreateAuthenticatedClient("Admin");

        var response =
            await client.GetAsync(
                "/api/student/dashboard");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task StudentAssignments_AsAdmin_Returns403()
    {
        var client =
            CreateAuthenticatedClient("Admin");

        var response =
            await client.GetAsync(
                "/api/student-assignments");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task MyAssignmentSubmissions_AsAdmin_Returns403()
    {
        var client =
            CreateAuthenticatedClient("Admin");

        var response =
            await client.GetAsync(
                "/api/assignment-submissions/my");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Forbidden);
    }

    // =====================================================
    // TEACHER
    // =====================================================

    [Fact]
    public async Task StudentDashboard_AsTeacher_Returns403()
    {
        var client =
            CreateAuthenticatedClient("Teacher");

        var response =
            await client.GetAsync(
                "/api/student/dashboard");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task StudentAssignments_AsTeacher_Returns403()
    {
        var client =
            CreateAuthenticatedClient("Teacher");

        var response =
            await client.GetAsync(
                "/api/student-assignments");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task MyAssignmentSubmissions_AsTeacher_Returns403()
    {
        var client =
            CreateAuthenticatedClient("Teacher");

        var response =
            await client.GetAsync(
                "/api/assignment-submissions/my");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Forbidden);
    }

    // =====================================================
    // STUDENT
    // =====================================================

    [Fact]
    public async Task StudentDashboard_AsStudent_AllowsAccess()
    {
        var client =
            CreateAuthenticatedClient("Student");

        var response =
            await client.GetAsync(
                "/api/student/dashboard");

        response.StatusCode
            .Should()
            .NotBe(HttpStatusCode.Unauthorized);

        response.StatusCode
            .Should()
            .NotBe(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task StudentAssignments_AsStudent_AllowsAccess()
    {
        var client =
            CreateAuthenticatedClient("Student");

        var response =
            await client.GetAsync(
                "/api/student-assignments");

        response.StatusCode
            .Should()
            .NotBe(HttpStatusCode.Unauthorized);

        response.StatusCode
            .Should()
            .NotBe(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task MyAssignmentSubmissions_AsStudent_AllowsAccess()
    {
        var client =
            CreateAuthenticatedClient("Student");

        var response =
            await client.GetAsync(
                "/api/assignment-submissions/my");

        response.StatusCode
            .Should()
            .NotBe(HttpStatusCode.Unauthorized);

        response.StatusCode
            .Should()
            .NotBe(HttpStatusCode.Forbidden);
    }

    // =====================================================
    // ALL SUBMISSIONS
    // =====================================================

    [Fact]
    public async Task AssignmentSubmissions_AsStudent_Returns403()
    {
        var client =
            CreateAuthenticatedClient("Student");

        var response =
            await client.GetAsync(
                "/api/assignment-submissions");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AssignmentSubmissions_AsTeacher_AllowsAccess()
    {
        var client =
            CreateAuthenticatedClient("Teacher");

        var response =
            await client.GetAsync(
                "/api/assignment-submissions");

        response.StatusCode
            .Should()
            .NotBe(HttpStatusCode.Unauthorized);

        response.StatusCode
            .Should()
            .NotBe(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AssignmentSubmissions_AsAdmin_AllowsAccess()
    {
        var client =
            CreateAuthenticatedClient("Admin");

        var response =
            await client.GetAsync(
                "/api/assignment-submissions");

        response.StatusCode
            .Should()
            .NotBe(HttpStatusCode.Unauthorized);

        response.StatusCode
            .Should()
            .NotBe(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AssignmentSubmissions_AsSuperAdmin_AllowsAccess()
    {
        var client =
            CreateAuthenticatedClient("SuperAdmin");

        var response =
            await client.GetAsync(
                "/api/assignment-submissions");

        response.StatusCode
            .Should()
            .NotBe(HttpStatusCode.Unauthorized);

        response.StatusCode
            .Should()
            .NotBe(HttpStatusCode.Forbidden);
    }
}