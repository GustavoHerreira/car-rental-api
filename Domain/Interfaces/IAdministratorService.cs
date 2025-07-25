using CarRentalAPI.Domain.DTOs.Administrator.Response;
using CarRentalAPI.Domain.DTOs.Administrator.Request;
using CarRentalAPI.Domain.DTOs.Authentication;
using CarRentalAPI.Domain.Entities;

namespace CarRentalAPI.Domain.Interfaces;

public interface IAdministratorService
{
    public Task<Administrator?> GetAdministratorById(int id);
    public Task<Administrator?> GetAdministratorByEmail(string email);
    public Task<bool> GetAdministratorByLoginAndPassword(LoginDto loginDto);
    public Task<ICollection<GetAdministratorDto>> GetAllAdministrators();
    public Task<GetAdministratorDto> CreateAdministrator(CreateAdministratorDto createAdministratorDto);
    public Task<GetAdministratorDto> UpdateAdministratorEmail(UpdateAdministratorEmailDto updateAdministratorEmailDto);
    public Task<bool> DeleteAdministrator(int id);
}