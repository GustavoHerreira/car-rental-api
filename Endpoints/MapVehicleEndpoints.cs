using CarRentalAPI.Domain.Entities;
using CarRentalAPI.Domain.Interfaces;
using CarRentalAPI.Domain.Validations;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Endpoints;

public static class VehicleEndpoints
{
    public static void MapVehicleEndpoints(this WebApplication app)
    {
        // Cria um grupo de endpoints para prefixar todas as rotas com "/vehicle"
        // Isso ajuda a organizar ainda mais e permite aplicar filtros ou tags a um grupo inteiro.
        var vehicleGroup = app.MapGroup("/vehicle")
            .WithTags("Veículos")
            .WithOpenApi();

        vehicleGroup.MapGet("/",
            async ([FromQuery] int? page, [FromQuery] int? itemsPerPage, IVehicleService vehicleService) =>
            Results.Ok(await vehicleService.GetAllVehicles(page, itemsPerPage)));

        vehicleGroup.MapGet("/{id:int}", async (int id, IVehicleService vehicleService) =>
        {
            var vehicle = await vehicleService.GetVehicleById(id);
            return vehicle is not null
                ? Results.Ok(vehicle)
                : Results.NotFound();
        });

        vehicleGroup.MapPost("/",
            async (Vehicle vehicle, IVehicleService vehicleService) =>
            {
                var vehicleValidation = new VehicleValidation();
                var validation = VehicleValidation.Validate(vehicle);
                return !validation.IsValid
                    ? Results.BadRequest(validation)
                    : Results.Created(
                    $"/vehicle/{vehicle.Id}", await vehicleService.CreateVehicle(vehicle));
            });

        vehicleGroup.MapPut("/{id:int}", async ([FromRoute] int id, [FromBody] Vehicle vehicle, IVehicleService vehicleService) =>
        {
            var vehicleValidation = new VehicleValidation();
            var validation = VehicleValidation.Validate(vehicle);
            if (!validation.IsValid) return Results.BadRequest(validation);
            
            try
            {
                var vehicleToUpdate = await vehicleService.UpdateVehicle(id, vehicle);
                return Results.Created($"/vehicle/{vehicle.Id}", vehicleToUpdate);
            }
            catch (Exception e)
            {
                return Results.NotFound(new
                {
                    Error = e.Message,
                });
            }
        });

        vehicleGroup.MapDelete("/{id:int}", async (int id, IVehicleService vehicleService) =>
        {
            var vehicleToDelete = await vehicleService.DeleteVehicle(id);
            return vehicleToDelete
                ? Results.NoContent()
                : Results.NotFound();
        });
    }
}