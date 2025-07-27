namespace CarRentalAPI.Domain.DTOs.Administrator.Request;

public record UpdateAdministratorDto(int Id, string? Email, string? Role);