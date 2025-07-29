namespace CarRentalAPI.Domain.DTOs.Administrator.Request;

using CarRentalAPI.Domain.Enums;

public record CreateAdministratorDto(string Email, string Password, AdminRoleEnum Role);