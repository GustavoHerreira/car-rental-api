using CarRentalAPI.Domain.Entities;

namespace CarRentalAPI.Application.Interfaces;

public interface IVehicleService
{
    Task<Vehicle?> GetByIdAsync(int id);
    Task<ICollection<Vehicle>> GetAllAsync(int? page = null, int? itemsPerPage = null);
    Task<Vehicle> AddAsync(Vehicle vehicle);
    Task<Vehicle> UpdateAsync(int id, Vehicle vehicle);
    Task<bool> DeleteAsync(int id);
}