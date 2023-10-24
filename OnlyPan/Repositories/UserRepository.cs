using Microsoft.EntityFrameworkCore;
using OnlyPan.Models;
using OnlyPan.Models.Dtos;
using OnlyPan.Models.Dtos.UserDtos;
using OnlyPan.Models.ViewModels.UserViewModels;
using OnlyPan.Utilities.Classes;

namespace OnlyPan.Repositories;

public class UserRepository
{
    private readonly OnlyPanDbContext _dbContext;

    public UserRepository(OnlyPanDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> EmailInDb(string email)
    {
        try
        {
            var user = await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.Correo == email);
            if (user == null)
            {
                return false;
            }

            return true;
        }
        catch (SystemException)
        {
            return true;
        }
    }

    public async Task<bool> RegisterUser(RegisterDto model)
    {
        try
        {
            var user = new Usuario()
            {
                FechaInscrito = DateTime.Now,
                Nombre = model.Name,
                Correo = model.Email,
                Contrasena = model.Password!,
                Foto = model.Photo,
                CodigoActivacion = model.ActivationToken,
                Estado = 1,
                Rol = 1,
                Activo = false,
                Biografia = "¡Hola! Soy nuevo en OnlyPan"
            };
            await _dbContext.Usuarios.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<bool> ActivateUser(string activationCode)
    {
        try
        {
            var user = await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.CodigoActivacion == activationCode);
            if (user == null)
            {
                return false;
            }

            user.Activo = true;
            user.CodigoActivacion = null;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<UserDto> LoginUser(string email, string password)
    {
        try
        {
            var users = await _dbContext.Usuarios.FromSqlRaw("EXECUTE sp_validate_user {0}, {1}", email, password)
                .ToListAsync();
            var user = users.FirstOrDefault();
            if (user == null)
            {
                return null!;
            }

            return new UserDto()
            {
                IdUser = user.IdUsuario,
                Name = user.Nombre,
                Email = user.Correo,
                Rol = user.Rol,
            };
        }
        catch (SystemException)
        {
            return null!;
        }
    }

    public async Task<RecoveryDto> CreateRecoveryToken(string email)
    {
        try
        {
            var user = await _dbContext.Usuarios
                .Where(u => u.Correo == email)
                .FirstOrDefaultAsync();
            string recoveryToken = Guid.NewGuid().ToString();
            user!.ContrasenaToken = recoveryToken;
            await _dbContext.SaveChangesAsync();
            return new RecoveryDto()
            {
                Email = user.Correo,
                Name = user.Nombre,
                Token = recoveryToken
            };
        }
        catch (SystemException)
        {
            return null!;
        }
    }

    public async Task<bool> RecoveryValidation(string recoveryToken)
    {
        try
        {
            var user = await _dbContext.Usuarios
                .Where(u => u.ContrasenaToken == recoveryToken)
                .FirstOrDefaultAsync();
            if (user == null)
                return false;
            return true;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<ProfileDto> RequestProfile(int idUser)
    {
        try
        {
            var user = await _dbContext.Usuarios
                .Where(u => u.IdUsuario == idUser)
                .FirstOrDefaultAsync();
            var rol = await _dbContext.Rols
                .Where(r => r.IdRol == user!.Rol)
                .FirstOrDefaultAsync();
            int followers = await _dbContext.SeguirUsuarios.CountAsync(r => r.IdSeguido == idUser);
            int followed = await _dbContext.SeguirUsuarios.CountAsync(r => r.IdSeguidor == idUser);
            return new ProfileDto()
            {
                RoleName = rol!.NombreRol,
                Photo = user!.Foto,
                Biography = user.Biografia,
                Name = user.Nombre,
                Email = user.Correo,
                Followers = followers,
                Followed = followed
            };
        }
        catch (SystemException)
        {
            return null!;
        }
    }

    public async Task<UserDto> EditUser(ProfileEditViewModel model, int idUser)
    {
        try
        {
            var user = await _dbContext.Usuarios.FindAsync(idUser);
            var pu = new PhotoUtilities();
            if (model.Email != user?.Correo)
                user!.Correo = model.Email!;
            if (model.Name != user?.Nombre)
                user!.Nombre = model.Name!;
            if (model.Biography != user?.Biografia)
                user!.Biografia = model.Biography;
            if (model.Photo != null && model.Photo.Length > 0)
                user!.Foto = await pu.convertToBytes(model.Photo);
            _dbContext.Usuarios.Update(user!);
            await _dbContext.SaveChangesAsync();
            return new UserDto()
            {
                IdUser = user!.IdUsuario,
                Name = user.Nombre,
                Email = user.Correo,
                Rol = user.Rol,
            };
        }
        catch (SystemException)
        {
            return null!;
        }
    }

    public async Task<string> RequestUserName(int idUser)
    {
        try
        {
            var user = await _dbContext.Usuarios.FindAsync(idUser);
            if (user != null)
            {
                return user.Nombre!;
            }

            throw new SystemException();
        }
        catch (SystemException)
        {
            return null!;
        }
    }

    public async Task<bool> ResetPasswordDb(string recoveryToken, string passwordToken)
    {
        try
        {
            var user = await _dbContext.Usuarios
                .Where(u => u.ContrasenaToken == recoveryToken)
                .FirstOrDefaultAsync();
            user!.Contrasena = passwordToken;
            user.ContrasenaToken = "";
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<ProfileRolDto> RequestProfileRolData(int idUser, int idRolNew)
    {
        try
        {
            var user = await _dbContext.Usuarios.FindAsync(idUser);
            var currentRol = await _dbContext.Rols.FindAsync(user!.Rol);
            var newRol = await _dbContext.Rols.FindAsync(idRolNew);
            int followers = await _dbContext.SeguirUsuarios.CountAsync(r => r.IdSeguido == idUser);
            int followed = await _dbContext.SeguirUsuarios.CountAsync(r => r.IdSeguidor == idUser);
            return new ProfileRolDto()
            {
                IdUser = user.IdUsuario,
                IdNewRol = idRolNew,
                Photo = user.Foto,
                Name = user.Nombre,
                Email = user.Correo,
                Biography = user.Biografia,
                CurrentRol = currentRol!.NombreRol,
                NewRol = newRol!.NombreRol,
                Followers = followers,
                Followed = followed
            };
        }
        catch (SystemException)
        {
            return null!;
        }
    }

    public async Task<bool> RequestFollow(int idUserLogged, int idUser)
    {
        try
        {
            var follow = await _dbContext.SeguirUsuarios
                .FindAsync(idUserLogged, idUser);
            if (follow == null) return false;
            return true;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<bool> FollowUser(int idUserLogged, int idUser)
    {
        try
        {
            var follow = await _dbContext.SeguirUsuarios
                .FindAsync(idUserLogged, idUser);
            if (follow == null)
            {
                var newFollow = new SeguirUsuario()
                {
                    IdSeguidor = idUserLogged,
                    IdSeguido = idUser,
                    FechaSeguimiento = DateTime.Now
                };
                await _dbContext.SeguirUsuarios.AddAsync(newFollow);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            throw new SystemException();
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<bool> UnFollowUser(int idUserLogged, int idUser)
    {
        try
        {
            var follow = await _dbContext.SeguirUsuarios
                .FindAsync(idUserLogged, idUser);
            if (follow != null)
            {
                _dbContext.SeguirUsuarios.Remove(follow);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            throw new SystemException();
        }
        catch (SystemException)
        {
            return false;
        }
    }
}