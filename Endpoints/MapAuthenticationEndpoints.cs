using CarRentalAPI.Domain.DTOs.Authentication;
using CarRentalAPI.Domain.Interfaces;

namespace CarRentalAPI.Endpoints;

public static class AuthenticationEndpoints
{
    public static void MapAuthenticationEndpoints(this WebApplication app)
    {
        var authGroup = app.MapGroup("/auth")
            .WithTags("Authentication")
            .WithOpenApi();
        
        authGroup.MapPost("/admin/login",
            async (LoginDto loginDto, IAuthenticationService authenticationService) =>
                await authenticationService.Login(loginDto)
                    ? Results.Ok("Login com sucesso")
                    : Results.Unauthorized());
    }
}
