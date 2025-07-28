using CarRentalAPI.Domain.DTOs.Administrator.Request;
using CarRentalAPI.Domain.DTOs.Administrator.Response;
using CarRentalAPI.Domain.DTOs.Authentication;
using CarRentalAPI.Domain.Entities;
using CarRentalAPI.Domain.Enums;
using CarRentalAPI.Domain.Exceptions.Users;
using CarRentalAPI.Domain.Interfaces;
using CarRentalAPI.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Infrastructure.Services;

public class AdministratorService(AppDbContext context) : IAdministratorService
{
    public async Task<GetAdministratorDto?> GetAdministratorById(int id)
    {
        var userInDb = await context.Administrators.FindAsync(id);
        return userInDb is null
            ? null
            : new GetAdministratorDto(userInDb.Id, userInDb.Email, userInDb.Role.ToString());
    }

    public async Task<GetAdministratorDto?> GetAdministratorByEmail(string email)
    {
        var userInDb = await context
            .Administrators
            .FirstOrDefaultAsync(a =>a.Email == email);
        return userInDb is null
            ? null
            : new GetAdministratorDto(userInDb.Id, userInDb.Email, userInDb.Role.ToString());
    }

    public async Task<Administrator?> GetAdministratorByLoginAndPassword(LoginDto loginDto)
    {
        var userInDb =  await context.Administrators.FirstOrDefaultAsync(a => a.Email == loginDto.Email && a.Password == loginDto.Password);
        return userInDb;
    }

    public async Task<ICollection<GetAdministratorDto>> GetAllAdministrators(int? page = null, int? itemsPerPage = null)
    {
        var query = context.Administrators
            .Select(a => new GetAdministratorDto(a.Id, a.Email, a.Role.ToString()));

        var currentPage = page.GetValueOrDefault(1);
        var currentItemsPerPage = itemsPerPage.GetValueOrDefault(10);

        if (currentPage < 1) currentPage = 1;
        if (currentItemsPerPage < 1) currentItemsPerPage = 10;

        if (!page.HasValue && !itemsPerPage.HasValue)
        {
            return await query.ToListAsync();
        }

        var skipAmount = (currentPage - 1) * currentItemsPerPage;

        query = query.Skip(skipAmount).Take(currentItemsPerPage);

        return await query.ToListAsync();
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
            Role = createAdministratorDto.Role
        };
        
        await context.Administrators.AddAsync(newAdministrator);
        await context.SaveChangesAsync();
        return new GetAdministratorDto(newAdministrator.Id, newAdministrator.Email, newAdministrator.Role.ToString());
    }

    public async Task<GetAdministratorDto> UpdateAdministrator(int id, UpdateAdministratorDto updateAdministratorDto)
    {
        if (id != updateAdministratorDto.Id)
            throw new InvalidOperationException(
                $"O ID na URL ({id}) não corresponde ao ID do administrador no corpo da requisição ({updateAdministratorDto.Id}).");

        var administratorToUpdate = await context.Administrators
                                        .FindAsync(updateAdministratorDto.Id)
                                    ?? throw new InvalidOperationException($"Administrador com ID {updateAdministratorDto.Id} não encontrado.");

        if (!string.IsNullOrEmpty(updateAdministratorDto.Email))
            administratorToUpdate.Email = updateAdministratorDto.Email;

        if (updateAdministratorDto.Role.HasValue)
        {
            administratorToUpdate.Role = updateAdministratorDto.Role.Value;
        }

        if (context.Entry(administratorToUpdate).State == EntityState.Modified)
            await context.SaveChangesAsync();

        return new GetAdministratorDto(
            administratorToUpdate.Id,
            administratorToUpdate.Email,
            administratorToUpdate.Role.ToString()
        );
    }


    public async Task<bool> DeleteAdministrator(int id)
    {
        var userInDb = await context.Administrators.FirstOrDefaultAsync(x => x.Id == id);
        if (userInDb is null) throw new UserIdNotFoundException(id);
    
        context.Administrators.Remove(userInDb);
        await context.SaveChangesAsync();
        return true;
    }
}