using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using OnlyPan.Models;
using OnlyPan.Models.Dtos;
using OnlyPan.Models.ViewModels;
using OnlyPan.Repositories;
using OnlyPan.Utilities.Classes;
using SystemException = System.SystemException;

namespace OnlyPan.Services;

public class UserServices
{
    private readonly OnlyPanContext _context;
    private readonly UserRepository _userRepository;

    public UserServices(OnlyPanContext context)
    {
        _context = context;
        _userRepository = new UserRepository(context);
    }

    public async Task<bool> CheckEmail(string email)
    {
        try
        {
            var result = await _userRepository.EmailInDb(email);
            if (result) return true;
            return false;
        }
        catch (SystemException)
        {
            return true;
        }
    }

    public async Task<bool> RegisterUser(RegisterViewModel model)
    {
        try
        {
            EncryptionService ecr = new EncryptionService();
            var encryptionKey1 = ecr.Encrypt(model.Password!);
            var encryptionKey2 = ecr.Encrypt(encryptionKey1);
            var activationToken = Guid.NewGuid().ToString();
            var pu = new PhotoUtilities();
            var user = new RegisterDto()
            {
                Nombre = model.Name!,
                Correo = model.Email!,
                Contrasena = encryptionKey2,
                Foto = pu.GetPhotoFromFile(Directory.GetCurrentDirectory()+"/Utilities/Images/default.jpeg"),
                CodigoActivacion = activationToken
            };
            var result = await _userRepository.RegisterUser(user);
            if (!result) return false;
            EmailService em = new EmailService();
            await em.SendVerificationEmail(user.Correo, user.Nombre, activationToken);
            return true;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public static async Task<bool> CreateCredentials(Usuario user, bool remember, HttpContext hc)
    {
        try
        {
            List<Claim> c = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, user.IdUsuario.ToString()),
                new(ClaimTypes.Email, user.Correo),
                new(ClaimTypes.Name, user.Nombre),
                new(ClaimTypes.Role, user.Rol.ToString())
            };
            ClaimsIdentity ci = new(c, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties p = new AuthenticationProperties();
            p.AllowRefresh = true;
            p.IsPersistent = remember;
            if (remember)
                p.ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1);
            else
                p.ExpiresUtc = DateTimeOffset.MaxValue;
            await hc.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci), p);
            return true;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<bool> LoginUsuario(LoginViewModel model, HttpContext hc)
    {
        try
        {
            EncryptionService enc = new EncryptionService();
            var encryptionHash1 = enc.Encrypt(model.Password!);
            var encryptionHash2 = enc.Encrypt(encryptionHash1);
            var usr = ValidateUser(model.Email!, encryptionHash2)[0];
            var result = await CreateCredentials(usr, model.Remember, hc);
            if (result)
                return true;
            return false;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    private List<Usuario> ValidateUser(string email, string password)
    {
        var userdb =
            _context.Usuarios.FromSqlRaw("EXECUTE sp_validate_user {0}, {1}", email, password).ToList();
        if (userdb.Count == 0)
        {
            return null!;
        }

        return userdb;
    }

    public async Task<bool> ActivateAccount(string token)
    {
        try
        {
            var users = await _context.Usuarios
                .Where(u => u.CodigoActivacion == token)
                .ToListAsync();
            Usuario user = users[0];
            user.Activo = true;
            await _context.SaveChangesAsync();
            return true;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<bool> ForgotPassword(string email)
    {
        try
        {
            var users = await _context.Usuarios
                .Where(u => u.Correo == email)
                .ToListAsync();
            Usuario user = users.First();
            string recoveryToken = Guid.NewGuid().ToString();
            user.ContrasenaToken = recoveryToken;
            await _context.SaveChangesAsync();
            EmailService em = new EmailService();
            await em.SendForgotPasswordEmail(user.Correo, user.Nombre, recoveryToken);
            return true;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<bool> RecoveryValidation(string recoveryToken)
    {
        try
        {
            var users = await _context.Usuarios
                .Where(u => u.ContrasenaToken == recoveryToken)
                .ToListAsync();
            Usuario user = users.First();
            return true;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<bool> ResetPassword(ResetPasswordViewModel model)
    {
        try
        {
            var users = await _context.Usuarios
                .Where(u => u.ContrasenaToken == model.Token)
                .ToListAsync();
            Usuario user = users.First();
            EncryptionService enc = new EncryptionService();
            var encryptionHash1 = enc.Encrypt(model.Password!);
            var encryptionHash2 = enc.Encrypt(encryptionHash1);
            user.Contrasena = encryptionHash2;
            await _context.SaveChangesAsync();
            //Delete the token for security
            user.ContrasenaToken = "";
            await _context.SaveChangesAsync();
            return true;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<bool> EditProfile(ProfileViewModel model, HttpContext hc)
    {
        try
        {
            var user = _context.Usuarios.Find(int.Parse(hc.User.Claims.First().Value));
            if (model.Email != user?.Correo && model.Email != null)
                user!.Correo = model.Email;
            //TODO send a email for confirm email change
            var pu = new PhotoUtilities();
            if (model.Photo != null && model.Photo.Length > 0)
            {
                user!.Foto = await pu.convertToBytes(model.Photo);
            }

            if (model.Name != user!.Nombre && model.Name != null)
                user.Nombre = model.Name;
            await hc.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _context.Update(user);
            await _context.SaveChangesAsync();
            var result = await CreateCredentials(user, true, hc);
            if (result)
                return true;
            return false;
        }
        catch (SystemException)
        {
            return false;
        }
    }
}