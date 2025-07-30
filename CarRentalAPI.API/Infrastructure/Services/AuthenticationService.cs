using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CarRentalAPI.Domain.DTOs.Authentication;
using CarRentalAPI.Domain.Entities;
using CarRentalAPI.Domain.Interfaces;
using CarRentalAPI.Domain.ModelViews;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using CarRentalAPI.Domain.Options;

namespace CarRentalAPI.Infrastructure.Services;

public class AuthenticationService(IAdministratorService adminService, IOptions<JwtOptions> jwtOptions) : IAuthenticationService
{
    private readonly string _jwtKey = jwtOptions.Value.Key;

    private string GenerateJwtForUser(Administrator admin)
    {
        var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtKey));
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