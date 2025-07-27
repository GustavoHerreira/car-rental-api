using CarRentalAPI.Domain.Entities;
using CarRentalAPI.Domain.Exceptions.Vehicles;
using CarRentalAPI.Domain.Interfaces;
using CarRentalAPI.Domain.Validations;
using CarRentalAPI.Infrastructure.Database;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Infrastructure.Services;

public class VehicleService(AppDbContext context) : IVehicleService
{
    public async Task<Vehicle?> GetVehicleById(int id) =>
        await context.Vehicles
            .FindAsync(id);

    public async Task<ICollection<Vehicle>> GetAllVehicles(int? page = null, int? itemsPerPage = null)
    {
        IQueryable<Vehicle> query = context.Vehicles;

        // Padroniza os valores de página e itens por página
        var currentPage = page.GetValueOrDefault(1); // Assume página 1 como padrão se não informado
        var currentItemsPerPage = itemsPerPage.GetValueOrDefault(10); // Assume 10 itens por página como padrão

        // Validação adicional para garantir valores positivos
        if (currentPage < 1) currentPage = 1;
        if (currentItemsPerPage < 1) currentItemsPerPage = 10;
        
        // Se ambos page e itemsPerPage são nulos (ou 0), significa que não queremos paginar
        if (!page.HasValue && !itemsPerPage.HasValue)
        {
            return await query.ToListAsync();
        }

        var skipAmount = (currentPage - 1) * currentItemsPerPage;
        
        query = query.Skip(skipAmount).Take(currentItemsPerPage);
        
        return await query.ToListAsync();
    }

    public async Task<Vehicle> CreateVehicle(Vehicle vehicle)
    {
        await context.Vehicles.AddAsync(vehicle);
        await context.SaveChangesAsync();
        return vehicle;
    }

    public async Task<Vehicle> UpdateVehicle(int id, Vehicle vehicle)
    {
        if (id != vehicle.Id)
            throw new InvalidOperationException(
                $"O ID na URL ({id}) não corresponde ao ID do veículo no corpo da requisição ({vehicle.Id}).");
        
        var vehicleToUpdate = await GetVehicleById(id);
        if (vehicleToUpdate is null)
            throw new VehicleNotFound(id);

        vehicleToUpdate.Name = vehicle.Name;
        vehicleToUpdate.Brand = vehicle.Brand;
        vehicleToUpdate.Year = vehicle.Year;
        
        await context.SaveChangesAsync();
        return vehicleToUpdate;
    }

    public async Task<bool> DeleteVehicle(int id)
    {
        var vehicle = await context.Vehicles.FindAsync(id);
        if (vehicle == null) return false;
        context.Vehicles.Remove(vehicle);
        await context.SaveChangesAsync();
        return true;
    }
}