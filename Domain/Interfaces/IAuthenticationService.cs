using CarRentalAPI.Domain.DTOs.Authentication;
using CarRentalAPI.Domain.ModelViews;

namespace CarRentalAPI.Domain.Interfaces;

public interface IAuthenticationService
{
    public Task<LoggedAdmin?> Login(LoginDto loginDto);
}