using CarRentalAPI.Domain.DTOs.Administrator.Request;
using CarRentalAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Presentation.API;

public static class AdministratorEndpoints
{
    public static void MapAdministratorEndpoints(this WebApplication app)
    {
        // Grupo de endpoints com prefixo "/admin"
        var adminGroup = app.MapGroup("/admin")
            .WithTags("Administrators")
            .RequireAuthorization("AdminOnly")
            .WithOpenApi();

        // GET: Listar todos os administradores
        adminGroup.MapGet("/", GetAllAdministrators)
            .WithName("GetAllAdministrators")
            .WithSummary("Lista todos os administradores")
            .WithDescription("Retorna uma lista paginada de todos os administradores cadastrados");

        // GET: Buscar administrador por 'ID'
        adminGroup.MapGet("/id/{id:int}", GetAdministratorById)
            .WithName("GetAdministratorById")
            .WithSummary("Obtém um administrador pelo ID")
            .WithDescription("Retorna os detalhes de um administrador específico baseado no ID");

        // GET: Buscar administrador por 'email'
        adminGroup.MapGet("/email/{email}", GetAdministratorByEmail)
            .WithName("GetAdministratorByEmail")
            .WithSummary("Obtém um administrador pelo email")
            .WithDescription("Retorna os detalhes de um administrador específico baseado no email");

        // POST: Criar administrador
        adminGroup.MapPost("/", CreateAdministrator)
            .AllowAnonymous() // Não precisa de autenticação
            .WithName("CreateAdministrator")
            .WithSummary("Cria um novo administrador")
            .WithDescription("Cria um novo registro de administrador no sistema");

        // PUT: Atualizar administrador
        adminGroup.MapPut("/{id:int}", UpdateAdministrator)
            .WithName("UpdateAdministrator")
            .WithSummary("Atualiza um administrador existente")
            .WithDescription("Atualiza os dados de um administrador existente com base no ID");

        // DELETE: Excluir administrador
        adminGroup.MapDelete("/{id:int}", DeleteAdministrator)
            .WithName("DeleteAdministrator")
            .WithSummary("Remove um administrador")
            .WithDescription("Remove permanentemente um administrador do sistema");
    }

    // Handlers
    private static async Task<IResult> GetAllAdministrators(
        [FromQuery] int? page, 
        [FromQuery] int? itemsPerPage, 
        IAdministratorService administratorService)
    {
        var admins = await administratorService.GetAllAdministrators(page, itemsPerPage);
        return Results.Ok(admins);
    }

    private static async Task<IResult> GetAdministratorById(
        int id, 
        IAdministratorService administratorService)
    {
        var admin = await administratorService.GetAdministratorById(id);
        return admin is not null
            ? Results.Ok(admin)
            : Results.NotFound();
    }

    private static async Task<IResult> GetAdministratorByEmail(
        string email, 
        IAdministratorService administratorService)
    {
        var admin = await administratorService.GetAdministratorByEmail(email);
        return admin is not null
            ? Results.Ok(admin)
            : Results.NotFound();
    }

    private static async Task<IResult> CreateAdministrator(
        CreateAdministratorDto createAdminDto, 
        IAdministratorService administratorService)
    {
        try
        {
            var newAdmin = await administratorService.CreateAdministrator(createAdminDto);
            return Results.Created($"/admin/id/{newAdmin.Id}", newAdmin);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { Error = ex.Message });
        }
    }

    private static async Task<IResult> UpdateAdministrator(
        int id, 
        UpdateAdministratorDto updateAdminDto, 
        IAdministratorService administratorService)
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
    }

    private static async Task<IResult> DeleteAdministrator(
        int id, 
        IAdministratorService administratorService)
    {
        var result = await administratorService.DeleteAdministrator(id);
        return result
            ? Results.NoContent()
            : Results.NotFound();
    }
}