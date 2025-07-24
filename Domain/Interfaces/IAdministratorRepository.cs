using CarRentalAPI.Domain.DTOs.Administrator.Response;
using CarRentalAPI.Domain.DTOs.Administrator.Request;
using CarRentalAPI.Domain.Entities;

namespace CarRentalAPI.Domain.Interfaces;

public interface IAdministratorRepository
{
    public Task<Administrator?> GetAdministratorById(int id);
    public Task<ICollection<GetAdministratorDto>> GetAllAdministrators();
    public Task<GetAdministratorDto> CreateAdministrator(CreateAdministratorDto createAdministratorDto);
    public Task<GetAdministratorDto> UpdateAdministrator(UpdateAdministratorDto updateAdministratorDto);
    public Task<bool> DeleteAdministrator(int id);
}