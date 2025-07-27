namespace CarRentalAPI.Domain.Exceptions.Users;

public class UserIdNotFoundException : Exception
{
    public UserIdNotFoundException(int id)
        : base($"O usuário de id '{id}' não foi encontrado.")
    {
    }

    public UserIdNotFoundException(int id, Exception innerException)
        : base($"O usuário de id '{id}' não foi encontrado.", innerException)
    {
    }
}