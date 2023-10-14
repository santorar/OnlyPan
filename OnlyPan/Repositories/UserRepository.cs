using Microsoft.EntityFrameworkCore;
using OnlyPan.Models;
using OnlyPan.Models.Dtos;
using OnlyPan.Models.ViewModels;
using OnlyPan.Utilities.Classes;

namespace OnlyPan.Repositories;

public class UserRepository
{
    private readonly OnlyPanContext _context;

    public UserRepository(OnlyPanContext context)
    {
        _context = context;
    }

    public async Task<bool> EmailInDb(string email)
    {
        try
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == email);
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
                Nombre = model.Nombre,
                Correo = model.Correo,
                Contrasena = model.Contrasena,
                Foto = model.Foto,
                CodigoActivacion = model.CodigoActivacion,
                Estado = 1,
                Rol = 1,
                Activo = false,
                Biografia = "¡Hola! Soy nuevo en OnlyPan"
            };
            await _context.Usuarios.AddAsync(user);
            await _context.SaveChangesAsync();
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
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.CodigoActivacion == activationCode);
            if (user == null)
            {
                return false;
            }

            user.Activo = true;
            user.CodigoActivacion = null;
            await _context.SaveChangesAsync();
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
            var users = await _context.Usuarios.FromSqlRaw("EXECUTE sp_validate_user {0}, {1}", email, password).ToListAsync();
            var user = users.FirstOrDefault();
            if (user == null)
            {
                return null!;
            }

            return new UserDto()
            {
                IdUsuario = user.IdUsuario,
                Nombre = user.Nombre,
                Correo = user.Correo,
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
            var user = await _context.Usuarios
                .Where(u => u.Correo == email)
                .FirstOrDefaultAsync();
            string recoveryToken = Guid.NewGuid().ToString();
            user!.ContrasenaToken = recoveryToken;
            await _context.SaveChangesAsync();
            return new RecoveryDto()
            {
                Email = user!.Correo,
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
            var user = await _context.Usuarios
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

    public async Task<UserDto> EditUser(ProfileViewModel model, int idUser)
    {
        try
        {
            var user = await _context.Usuarios.FindAsync(idUser);
            var pu = new PhotoUtilities();
            if (model.Email != user?.Correo)
                user!.Correo = model.Email!;
            if (model.Name != user?.Nombre)
                user!.Nombre = model.Name!;
            if (model.Biography != user?.Biografia)
                user!.Biografia = model.Biography;
            if (model.Photo != null && model.Photo.Length > 0)
                user!.Foto = await pu.convertToBytes(model.Photo);
            _context.Usuarios.Update(user!);
            await _context.SaveChangesAsync();
            return new UserDto()
            {
                IdUsuario = user!.IdUsuario,
                Nombre = user.Nombre,
                Correo = user.Correo,
                Rol = user.Rol,
            };
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
            var user = await _context.Usuarios
                .Where(u => u.ContrasenaToken == recoveryToken)
                .FirstOrDefaultAsync();
            user!.Contrasena = passwordToken;
            user.ContrasenaToken = "";
            await _context.SaveChangesAsync();
            return true;
        }
        catch (SystemException)
        {
            return false;
        }
    }
}