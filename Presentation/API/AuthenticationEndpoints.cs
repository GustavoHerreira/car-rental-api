using CarRentalAPI.Domain.DTOs.Authentication;
using CarRentalAPI.Domain.Interfaces;

namespace CarRentalAPI.Presentation.API;

public static class AuthenticationEndpoints
{
    public static void MapAuthenticationEndpoints(this WebApplication app)
    {
        var authGroup = app.MapGroup("/auth")
            .WithTags("Authentication")
            .WithOpenApi();

        // POST: Login do administrador
        authGroup.MapPost("/admin/login", AdminLogin)
            .WithName("AdminLogin")
            .WithSummary("Autentica um administrador")
            .WithDescription("Realiza login de um administrador e retorna um token JWT");
    }

    // Handlers
    private static async Task<IResult> AdminLogin(
        LoginDto loginDto, 
        IAuthenticationService authenticationService)
    {
        var loggedAdmin = await authenticationService.Login(loginDto);
        return loggedAdmin is not null
            ? Results.Ok(loggedAdmin)
            : Results.Unauthorized();
    }
}