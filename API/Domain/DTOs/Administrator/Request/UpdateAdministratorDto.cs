namespace CarRentalAPI.Domain.DTOs.Administrator.Request;

using CarRentalAPI.Domain.Enums;

public record UpdateAdministratorDto(int Id, string? Email, AdminRoleEnum? Role);