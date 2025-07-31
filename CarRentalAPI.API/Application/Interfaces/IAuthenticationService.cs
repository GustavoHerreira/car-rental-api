using CarRentalAPI.Application.DTOs.Authentication;
using CarRentalAPI.Application.ModelViews;

namespace CarRentalAPI.Application.Interfaces;

public interface IAuthenticationService
{
    public Task<LoggedAdmin?> Login(LoginDto loginDto);
}