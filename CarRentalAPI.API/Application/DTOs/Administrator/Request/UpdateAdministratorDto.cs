using CarRentalAPI.Domain.Enums;

namespace CarRentalAPI.Application.DTOs.Administrator.Request;

public record UpdateAdministratorDto(int Id, string? Email, AdminRoleEnum? Role);