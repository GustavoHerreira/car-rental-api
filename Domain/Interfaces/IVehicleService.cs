using CarRentalAPI.Domain.Entities;

namespace CarRentalAPI.Domain.Interfaces;

public interface IVehicleService
{
    public Task<Vehicle?> GetVehicleById(int id);
    public Task<ICollection<Vehicle>> GetAllVehicles(int? page = null, int? itemsPerPage = null);
    public Task<Vehicle> CreateVehicle(Vehicle vehicle);
    public Task<Vehicle> UpdateVehicle(int id, Vehicle vehicle);
    public Task<bool> DeleteVehicle(int id);
}