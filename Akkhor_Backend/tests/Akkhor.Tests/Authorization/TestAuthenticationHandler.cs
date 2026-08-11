using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Akkhor.Tests.Authorization;

public class TestAuthenticationHandler
    : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "Test";

    public TestAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult>
        HandleAuthenticateAsync()
    {
        var role =
            Request.Headers["X-Test-Role"]
                .FirstOrDefault();

        if (string.IsNullOrWhiteSpace(role))
        {
            role = "Student";
        }

        var userId =
            Request.Headers["X-Test-UserId"]
                .FirstOrDefault()
            ?? "test-user-id";

        var claims = new[]
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                userId),

            new Claim(
                ClaimTypes.Name,
                "Test User"),

            new Claim(
                ClaimTypes.Role,
                role)
        };

        var identity =
            new ClaimsIdentity(
                claims,
                SchemeName);

        var principal =
            new ClaimsPrincipal(identity);

        var ticket =
            new AuthenticationTicket(
                principal,
                SchemeName);

        return Task.FromResult(
            AuthenticateResult.Success(ticket));
    }
}