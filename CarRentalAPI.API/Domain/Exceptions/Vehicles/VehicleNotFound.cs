namespace CarRentalAPI.Domain.Exceptions.Vehicles;

public class VehicleNotFound : Exception
{
    public VehicleNotFound(int id)
        : base($"O veículo de id '{id}' não foi encontrado.")
    {
    }

    public VehicleNotFound(int id, Exception innerException)
        : base($"O veículo de id '{id}' não foi encontrado.", innerException)
    {
    }
}