using CarRentalAPI.Domain.Entities;
using CarRentalAPI.Domain.Validations;
using CarRentalAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Presentation.API;

public static class VehicleEndpoints
{
    public static void MapVehicleEndpoints(this WebApplication app)
    {
        // Grupo de endpoints com prefixo "/vehicle"
        var vehicleGroup = app.MapGroup("/vehicle")
            .RequireAuthorization("AdminOnly")
            .WithTags("Vehicles")
            .WithOpenApi();

        // GET: Listar todos os veículos
        vehicleGroup.MapGet("/", GetAllVehicles)
            .WithName("GetAllVehicles")
            .WithSummary("Lista todos os veículos")
            .WithDescription("Retorna uma lista paginada de todos os veículos cadastrados");

        // GET: Buscar veículo por ID
        vehicleGroup.MapGet("/{id:int}", GetVehicleById)
            .WithName("GetVehicleById")
            .WithSummary("Obtém um veículo pelo ID")
            .WithDescription("Retorna os detalhes de um veículo específico baseado no ID");

        // POST: Criar veículo
        vehicleGroup.MapPost("/", CreateVehicle)
            .RequireAuthorization("EditorOrAdmin")
            .WithName("CreateVehicle")
            .WithSummary("Cria um novo veículo")
            .WithDescription("Cria um novo registro de veículo no sistema");

        // PUT: Atualizar veículo
        vehicleGroup.MapPut("/{id:int}", UpdateVehicle)
            .RequireAuthorization("EditorOrAdmin")
            .WithName("UpdateVehicle")
            .WithSummary("Atualiza um veículo existente")
            .WithDescription("Atualiza os dados de um veículo existente com base no ID");

        // DELETE: Excluir veículo
        vehicleGroup.MapDelete("/{id:int}", DeleteVehicle)
            .WithName("DeleteVehicle")
            .WithSummary("Remove um veículo")
            .WithDescription("Remove permanentemente um veículo do sistema");
    }

    // Handlers
    private static async Task<IResult> GetAllVehicles(
        [FromQuery] int? page, 
        [FromQuery] int? itemsPerPage, 
        IVehicleService vehicleService)
    {
        var vehicles = await vehicleService.GetAllAsync(page, itemsPerPage);
        return Results.Ok(vehicles);
    }

    private static async Task<IResult> GetVehicleById(
        int id, 
        IVehicleService vehicleService)
    {
        var vehicle = await vehicleService.GetByIdAsync(id);
        return vehicle is not null
            ? Results.Ok(vehicle)
            : Results.NotFound();
    }

    private static async Task<IResult> CreateVehicle(
        Vehicle vehicle, 
        IVehicleService vehicleService)
    {
        var validation = VehicleValidation.Validate(vehicle);
        if (!validation.IsValid)
        {
            return Results.BadRequest(validation);
        }

        var createdVehicle = await vehicleService.AddAsync(vehicle);
        return Results.Created($"/vehicle/{createdVehicle.Id}", createdVehicle);
    }

    private static async Task<IResult> UpdateVehicle(
        [FromRoute] int id, 
        [FromBody] Vehicle vehicle, 
        IVehicleService vehicleService)
    {
        var validation = VehicleValidation.Validate(vehicle);
        if (!validation.IsValid)
        {
            return Results.BadRequest(validation);
        }

        try
        {
            vehicle.Id = id;
            var updatedVehicle = await vehicleService.UpdateAsync(vehicle);
            return Results.Ok(updatedVehicle);
        }
        catch (Exception ex)
        {
            return Results.NotFound(new { Error = ex.Message });
        }
    }

    private static async Task<IResult> DeleteVehicle(
        int id, 
        IVehicleService vehicleService)
    {
        var result = await vehicleService.DeleteAsync(id);
        return result
            ? Results.NoContent()
            : Results.NotFound();
    }
}