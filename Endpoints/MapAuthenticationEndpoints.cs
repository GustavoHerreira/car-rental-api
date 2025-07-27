using CarRentalAPI.Domain.DTOs.Authentication;
using CarRentalAPI.Domain.Interfaces;
using CarRentalAPI.Domain.ModelViews;

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
            {
                var loggedAdmin = await authenticationService.Login(loginDto);
                return loggedAdmin is not null
                    ? Results.Ok(loggedAdmin)
                    : Results.Unauthorized();
            });
    }
}