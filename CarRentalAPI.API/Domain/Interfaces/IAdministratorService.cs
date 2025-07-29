using CarRentalAPI.Domain.DTOs.Administrator.Response;
using CarRentalAPI.Domain.DTOs.Administrator.Request;
using CarRentalAPI.Domain.DTOs.Authentication;
using CarRentalAPI.Domain.Entities;

namespace CarRentalAPI.Domain.Interfaces;

public interface IAdministratorService
{
    public Task<GetAdministratorDto?> GetAdministratorById(int id);
    public Task<GetAdministratorDto?> GetAdministratorByEmail(string email);
    public Task<Administrator?> GetAdministratorByLoginAndPassword(LoginDto loginDto);
    public Task<ICollection<GetAdministratorDto>> GetAllAdministrators(int? page = null, int? itemsPerPage = null);
    public Task<GetAdministratorDto> CreateAdministrator(CreateAdministratorDto createAdministratorDto);
    public Task<GetAdministratorDto> UpdateAdministrator(int id, UpdateAdministratorDto updateAdministratorDto);
    public Task<bool> DeleteAdministrator(int id);
}