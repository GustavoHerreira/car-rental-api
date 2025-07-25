using CarRentalAPI.Domain.DTOs.Authentication;

namespace CarRentalAPI.Domain.Interfaces;

public interface IAuthenticationService
{
    public Task<bool> Login(LoginDto loginDto);
}