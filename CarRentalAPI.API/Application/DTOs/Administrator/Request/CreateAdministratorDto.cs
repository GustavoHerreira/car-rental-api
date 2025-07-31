using CarRentalAPI.Domain.Enums;

namespace CarRentalAPI.Application.DTOs.Administrator.Request;

public record CreateAdministratorDto(string Email, string Password, AdminRoleEnum Role);