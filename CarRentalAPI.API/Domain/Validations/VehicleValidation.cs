using CarRentalAPI.Domain.Entities;

namespace CarRentalAPI.Domain.Validations;

public class VehicleValidation
{
    public static VehicleValidationStruct Validate(Vehicle vehicle)
    {
        var validation = new VehicleValidationStruct
        {
            Errors = []
        };

        if (string.IsNullOrEmpty(vehicle.Name))
            validation.Errors.Add("Nome do Modelo deve ser menor que 150 caracteres.");
        if (string.IsNullOrEmpty(vehicle.Brand))
            validation.Errors.Add("Nome da Fabricante deve ser menor que 150 caracteres.");
        if (vehicle.Year < 1950)
            validation.Errors.Add("Ano deve ser superior a 1950");
        
        if (vehicle.Year > DateTime.Now.Year)
            validation.Errors.Add("O ano deve ser menor que o ano atual.");
        
        if (vehicle.Name.Length > 150)
            validation.Errors.Add("Nome deve ser menor que 150 caracteres.");
        
        if (vehicle.Brand.Length > 150)
            validation.Errors.Add("Nome da marca deve ser menor que 150 caracteres.");
        
        return validation;
    }
}

public struct VehicleValidationStruct
{
    public List<string> Errors { get; set; }
    public bool IsValid => Errors.Count == 0;
}