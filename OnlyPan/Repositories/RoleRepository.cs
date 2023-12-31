using Microsoft.EntityFrameworkCore;
using OnlyPan.Models;
using OnlyPan.Models.Dtos.RoleDtos;
using OnlyPan.Models.ViewModels.RolViewModels;

namespace OnlyPan.Repositories;

public class RoleRepository
{
    private readonly OnlyPanDbContext _dbContext;

    public RoleRepository(OnlyPanDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> RequestRoleName(int idRole)
    {
        try
        {
            var role = await _dbContext.Rols.FindAsync(idRole);
            if (role != null)
                return role.NombreRol;
            throw new SystemException();
        }
        catch (SystemException)
        {
            return null!;
        }
    }

    public async Task<List<RoleDto>> RequestRoles()
    {
        try
        {
            var roles = await _dbContext.Rols.Where(r => r.IdRol >= 2 && r.IdRol <= 4).ToListAsync();
            List<RoleDto> result = new List<RoleDto>();
            foreach (var role in roles)
            {
                RoleDto roleDto = new RoleDto()
                {
                    IdRol = role.IdRol,
                    NombreRol = role.NombreRol
                };
                result.Add(roleDto);
            }

            return result;
        }
        catch (SystemException)
        {
            return null!;
        }
    }

    public async Task<bool> RequestUserPetitions(int idUser, int idRole)
    {
        try
        {
            var petition = await _dbContext
                .SolicitudRols.FindAsync(idUser, idRole);
            if (petition == null) return false;
            return true;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<bool> CreatePetition(RoleMakePetitionViewModel model, int idUser)
    {
        try
        {
            var petition = new SolicitudRol()
            {
                IdUsuarioSolicitud = idUser,
                IdRolSolicitud = model.Role,
                IdEstado = 4,
                FechaSolicitud = DateTime.Now
            };
            await _dbContext.AddAsync(petition);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<List<RolePetitionDto>> RequestPetitions()
    {
        try
        {
            var petitions = await _dbContext.SolicitudRols
                .Include(u => u.IdUsuarioSolicitudNavigation)
                .Include(u => u.IdUsuarioSolicitudNavigation.RolNavigation)
                .Include(r => r.IdRolSolicitudNavigation)
                .Where(s => s.IdEstado == 4).ToListAsync();
            List<RolePetitionDto> resultSet = new List<RolePetitionDto>();
            foreach (var request in petitions)
            {
                RolePetitionDto rolePetitionDto = new RolePetitionDto()
                {
                    IdUser = request.IdUsuarioSolicitud,
                    IdRequesedRole = request.IdRolSolicitud,
                    Time = request.FechaSolicitud,
                    UserName = request.IdUsuarioSolicitudNavigation.Nombre,
                    CurrentRoleName = request.IdUsuarioSolicitudNavigation.RolNavigation.NombreRol,
                    RequesedRoleName = request.IdRolSolicitudNavigation.NombreRol
                };
                resultSet.Add(rolePetitionDto);
            }

            return resultSet;
        }
        catch (SystemException)
        {
            return null!;
        }
    }

    public async Task<bool> AcceptPetition(int idUser, int idRol, int idUserAdmin)
    {
        try
        {
            // take the petition from the database
            SolicitudRol? petition = await _dbContext.SolicitudRols.FindAsync(idUser, idRol);
            petition!.IdUsuarioAprovador = idUserAdmin;
            petition.FechaAprovacion = DateTime.UtcNow;
            petition.IdEstado = 5;
            // Add the rol to the user
            Usuario? user = await _dbContext.Usuarios.FindAsync(idUser);
            if (user != null)
            {
                user.Rol = petition.IdRolSolicitud;
                // Save the changes into the database
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<bool> RejectPetition(int idUser, int idRol, int idUserAdmin)
    {
        try
        {
            // take the petition from the database
            SolicitudRol? petition = await _dbContext.SolicitudRols.FindAsync(idUser, idRol);
            petition!.IdUsuarioAprovador = idUserAdmin;
            petition.FechaAprovacion = DateTime.UtcNow;
            petition.IdEstado = 6;
            // Save the changes into the database
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<List<string>> RequestAdminsEmails()
    {
        try
        {
            List<string> emails = new List<string>();
            var admins = await _dbContext.Usuarios
                .Where(u => u.Rol == 2 || u.Rol == 3)
                .ToListAsync();
            foreach (var admin in admins)
            {
                emails.Add(admin.Correo!);
            }

            return emails;
        }
        catch (SystemException)
        {
            return null!;
        }
    }
}