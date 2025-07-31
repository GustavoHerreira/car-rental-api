using CarRentalAPI.Domain.DTOs.Authentication;
using CarRentalAPI.Domain.Entities;
using CarRentalAPI.Domain.Interfaces;
using CarRentalAPI.Domain.ModelViews;
using CarRentalAPI.Domain.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarRentalAPI.Infrastructure.Services;

public class AuthenticationService(IAdministratorService adminService, IOptionsSnapshot<JwtOptions> jwtOptions) : IAuthenticationService
{
    private string GenerateJwtForUser(Administrator admin)
    {
        var jwtKey = jwtOptions.Value.Key;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, admin.Id.ToString()),
            new(ClaimTypes.Email, admin.Email),
            new(ClaimTypes.Role, admin.Role.ToString())
        };

        var token = new JwtSecurityToken(
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials,
            claims: claims
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<LoggedAdmin?> Login(LoginDto loginDto)
    {
        var admin = await adminService.GetAdministratorByLoginAndPassword(loginDto);
        if (admin is null) return null;
        
        return new LoggedAdmin(
            Id: admin.Id,
            Email: admin.Email,
            Role: admin.Role.ToString(),
            Token: GenerateJwtForUser(admin)
        );
    }
}