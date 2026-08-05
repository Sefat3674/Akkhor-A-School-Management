using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Akkhor.Application.Interfaces;
using Akkhor.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Akkhor.Application.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
    }


    public (string token, DateTime expiresAt) CreateToken(
        Users user,
        IList<string> roles)
    {
        var jwtSection = _config.GetSection("Jwt");


        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSection["Key"]!)
        );


        var creds = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );


        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),

            new(
                JwtRegisteredClaimNames.Email,
                user.Email ?? string.Empty
            ),

            new(
                ClaimTypes.NameIdentifier,
                user.Id
            ),

            new(
                ClaimTypes.Name,
                user.FullName ?? user.UserName ?? string.Empty
            )
        };


        // Add Role Claims
        claims.AddRange(
            roles.Select(role =>
                new Claim(
                    ClaimTypes.Role,
                    role
                )
            )
        );


        var expiryMinutes = int.Parse(
            jwtSection["ExpiryMinutes"] ?? "480"
        );


        var expires = DateTime.UtcNow.AddMinutes(expiryMinutes);


        var token = new JwtSecurityToken(

            issuer: jwtSection["Issuer"],

            audience: jwtSection["Audience"],

            claims: claims,

            expires: expires,

            signingCredentials: creds
        );


        return (
            new JwtSecurityTokenHandler()
                .WriteToken(token),

            expires
        );
    }
}