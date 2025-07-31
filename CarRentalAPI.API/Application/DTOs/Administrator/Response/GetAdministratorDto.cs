using CarRentalAPI.Domain.Enums;

namespace CarRentalAPI.Application.DTOs.Administrator.Response;

public record GetAdministratorDto(int Id, string Email, string Role);