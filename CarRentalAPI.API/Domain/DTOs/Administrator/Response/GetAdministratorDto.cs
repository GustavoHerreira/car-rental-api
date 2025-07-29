using CarRentalAPI.Domain.Enums;

namespace CarRentalAPI.Domain.DTOs.Administrator.Response;

public record GetAdministratorDto(int Id, string Email, string Role);