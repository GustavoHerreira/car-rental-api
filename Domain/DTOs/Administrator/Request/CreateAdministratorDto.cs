namespace CarRentalAPI.Domain.DTOs.Administrator.Request;

public record CreateAdministratorDto(string Email, string Password, string Role);