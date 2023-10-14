using Microsoft.EntityFrameworkCore;
using OnlyPan.Models;
using OnlyPan.Models.Dtos;

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
}