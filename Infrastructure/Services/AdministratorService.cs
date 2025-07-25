using CarRentalAPI.Domain.DTOs.Administrator.Request;
using CarRentalAPI.Domain.DTOs.Administrator.Response;
using CarRentalAPI.Domain.DTOs.Authentication;
using CarRentalAPI.Domain.Entities;
using CarRentalAPI.Domain.Exceptions;
using CarRentalAPI.Domain.Interfaces;
using CarRentalAPI.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Infrastructure.Services;

public class AdministratorService(AppDbContext context) : IAdministratorService
{
    public async Task<Administrator?> GetAdministratorById(int id)
    {
        var userInDb = await context.Administrators.FirstOrDefaultAsync(x => x.Id == id);
        return userInDb;
    }

    public async Task<Administrator?> GetAdministratorByEmail(string email)
    {
        var userInDb = await context.Administrators.FirstOrDefaultAsync(x => x.Email == email);
        return userInDb;
    }

    public async Task<bool> GetAdministratorByLoginAndPassword(LoginDto loginDto)
    {
        var userInDb =  await context.Administrators.AnyAsync(a => a.Email == loginDto.Email && a.Password == loginDto.Password);
        return userInDb;
    }

    public async Task<ICollection<GetAdministratorDto>> GetAllAdministrators()
    {
        var usersInDb = await context.Administrators
            .Select(a => new GetAdministratorDto(a.Id, a.Email, a.Role))
            .ToListAsync();
        return usersInDb;
    }

    public async Task<GetAdministratorDto> CreateAdministrator(CreateAdministratorDto createAdministratorDto)
    {
        var emailExists = await context.Administrators.AnyAsync(a => a.Email == createAdministratorDto.Email);
        if (emailExists)
            throw new DuplicateEmailException(email: createAdministratorDto.Email);
        
        var newAdministrator = new Administrator
        {
            Email = createAdministratorDto.Email,
            Password = createAdministratorDto.Password,
            Role = "admin"
        };
        
        await context.Administrators.AddAsync(newAdministrator);
        await context.SaveChangesAsync();
        return new GetAdministratorDto(newAdministrator.Id, newAdministrator.Email, newAdministrator.Role);
    }

    public async Task<GetAdministratorDto> UpdateAdministratorEmail(UpdateAdministratorEmailDto updateAdministratorDto)
    {
        var administratorToUpdate = await context.Administrators.FirstOrDefaultAsync(a => a.Id == updateAdministratorDto.Id);

        if (administratorToUpdate == null)
            throw new InvalidOperationException($"Administrador com ID {updateAdministratorDto.Id} não encontrado.");

        administratorToUpdate.Email = updateAdministratorDto.Email;
        await context.SaveChangesAsync();
        
        return new GetAdministratorDto(
            administratorToUpdate.Id,
            administratorToUpdate.Email,
            administratorToUpdate.Role
        );
    }

    public async Task<bool> DeleteAdministrator(int id)
    {
        var userInDb = await context.Administrators.FirstOrDefaultAsync(x => x.Id == id);
        if (userInDb == null)
            throw new UserIdNotFoundException(id);
    
        context.Administrators.Remove(userInDb);
        await context.SaveChangesAsync();
        return true;
    }
}