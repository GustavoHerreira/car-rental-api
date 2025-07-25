using CarRentalAPI.Domain.DTOs.Authentication;
using CarRentalAPI.Domain.Interfaces;

namespace CarRentalAPI.Infrastructure.Services;

public class AuthenticationService(IAdministratorService adminService) : IAuthenticationService 
{
    public async Task<bool> Login(LoginDto loginDto)
    {
        return await adminService.GetAdministratorByLoginAndPassword(loginDto);
    }
}