namespace CarRentalAPI.Domain.ModelViews;

public record LoggedAdmin(int Id, string Email, string Role, string Token);