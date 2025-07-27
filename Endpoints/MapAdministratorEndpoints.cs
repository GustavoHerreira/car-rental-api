using CarRentalAPI.Domain.DTOs.Administrator.Request;
using CarRentalAPI.Domain.Enums;
using CarRentalAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Endpoints;

public static class AdministratorEndpoints
{
    public static void MapAdministratorEndpoints(this WebApplication app)
    {
        // Cria um grupo de endpoints para prefixar todas as rotas com "/admin"
        var adminGroup = app.MapGroup("/admin")
            .WithTags("Admin")
            .RequireAuthorization()
            .WithOpenApi();

        adminGroup.MapGet("/", 
            async ([FromQuery] int? page, [FromQuery] int? itemsPerPage, IAdministratorService administratorService) =>
            {
                var admins = await administratorService.GetAllAdministrators(page, itemsPerPage);
                return Results.Ok(admins);
            });

        adminGroup.MapGet("/{id:int}",
            async (int id, IAdministratorService administratorService) => 
            {
                var admin = await administratorService.GetAdministratorById(id);
                return admin is not null
                    ? Results.Ok(admin)
                    : Results.NotFound();
            });

        adminGroup.MapGet("/{email}",
            async (string email, IAdministratorService administratorService) => 
            {
                var admin = await administratorService.GetAdministratorByEmail(email);
                return admin is not null
                    ? Results.Ok(admin)
                    : Results.NotFound();
            });

        adminGroup.MapPost("/",
            async (CreateAdministratorDto createAdminDto, IAdministratorService administratorService) =>
            {
                try
                {
                    var newAdmin = await administratorService.CreateAdministrator(createAdminDto);
                    return Results.Created($"/admin/{newAdmin.Id}", newAdmin);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { Error = ex.Message });
                }
            }).AllowAnonymous();

        adminGroup.MapPut("/{id:int}",
            async (int id, UpdateAdministratorDto updateAdminDto, IAdministratorService administratorService) =>
            {
                try
                {
                    var updatedAdmin = await administratorService.UpdateAdministrator(id, updateAdminDto);
                    return Results.Ok(updatedAdmin);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { Error = ex.Message });
                }
            });

        adminGroup.MapDelete("/{id:int}",
            async (int id, IAdministratorService administratorService) =>
            {
                var result = await administratorService.DeleteAdministrator(id);
                return result
                    ? Results.NoContent()
                    : Results.NotFound();
            });
    }
}