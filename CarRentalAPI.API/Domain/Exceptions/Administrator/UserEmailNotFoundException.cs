﻿namespace CarRentalAPI.Domain.Exceptions.Administrator;

public class UserEmailNotFoundException : Exception
{
    public UserEmailNotFoundException(string email)
        : base($"O email '{email}' não foi encontrado.")
    {
    }

    public UserEmailNotFoundException(string email, Exception innerException)
        : base($"O email '{email}' não foi encontrado.", innerException)
    {
    }
}