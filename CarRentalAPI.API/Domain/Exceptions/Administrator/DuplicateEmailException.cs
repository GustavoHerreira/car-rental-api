namespace CarRentalAPI.Domain.Exceptions.Administrator;

public class DuplicateEmailException : Exception
{
    public DuplicateEmailException(string email)
        : base($"O email '{email}' já está em uso por outro administrador.")
    {
    }

    public DuplicateEmailException(string email, Exception innerException)
        : base($"O email '{email}' já está em uso por outro administrador.", innerException)
    {
    }
}