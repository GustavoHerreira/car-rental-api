using CarRentalAPI.Domain.Entities;
using CarRentalAPI.Domain.Exceptions.Vehicles;
using CarRentalAPI.Application.Interfaces;
using CarRentalAPI.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Infrastructure.Services;

public class VehicleService(AppDbContext context) : IVehicleService
{
    public async Task<Vehicle?> GetByIdAsync(int id) =>
        await context.Vehicles.FindAsync(id);

    public async Task<ICollection<Vehicle>> GetAllAsync(int? page = null, int? itemsPerPage = null)
    {
        var query = context.Vehicles.AsQueryable();
        
        if (page.HasValue && itemsPerPage.HasValue)
        {
            query = query.Skip((page.Value - 1) * itemsPerPage.Value)
                        .Take(itemsPerPage.Value);
        }
        
        return await query.ToListAsync();
    }

    public async Task<Vehicle> AddAsync(Vehicle vehicle)
    {
        context.Vehicles.Add(vehicle);
        await context.SaveChangesAsync();
        return vehicle;
    }

    public async Task<Vehicle> UpdateAsync(int id, Vehicle vehicle)
    {
        if (id != vehicle.Id)
            throw new ArgumentException($"The ID in the URL ({id}) does not match the ID in the vehicle model ({vehicle.Id}).");

        var vehicleToUpdate = await context.Vehicles.FindAsync(vehicle.Id)
            ?? throw new VehicleNotFound(vehicle.Id);

        // Update fields
        vehicleToUpdate.Name = vehicle.Name;
        vehicleToUpdate.Brand = vehicle.Brand;
        vehicleToUpdate.Year = vehicle.Year;

        if (context.Entry(vehicleToUpdate).State == EntityState.Modified)
            await context.SaveChangesAsync();

        return vehicleToUpdate;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var vehicle = await GetByIdAsync(id);
        if (vehicle is null) return false;
        
        context.Vehicles.Remove(vehicle);
        await context.SaveChangesAsync();
        return true;
    }
}