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
                Foto = pu.GetPhotoFromFile(Directory.GetCurrentDirectory() + "/Utilities/Images/default.jpeg"),
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

    public static async Task<bool> CreateCredentials(UserDto user, bool remember, HttpContext hc)
    {
        try
        {
            List<Claim> c = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, user.IdUsuario.ToString()),
                new(ClaimTypes.Email, user.Correo!),
                new(ClaimTypes.Name, user.Nombre!),
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
            var usr = await _userRepository.LoginUser(model.Email!, encryptionHash2);
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

    public async Task<bool> ActivateAccount(string token)
    {
        var result = await _userRepository.ActivateUser(token);
        if (!result) return false;
        return true;
    }

    public async Task<bool> ForgotPassword(string email)
    {
        try
        {
            RecoveryDto model = await _userRepository.CreateRecoveryToken(email);
            EmailService em = new EmailService();
            await em.SendForgotPasswordEmail(model.Email!, model.Name!, model.Token!);
            return true;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<bool> RecoveryTokenExist(string recoveryToken)
    {
        try
        {
            if (await _userRepository.RecoveryValidation(recoveryToken))
                return true;
            return false;
        }catch (SystemException)
        {
            return false;
        }
    }

    public async Task<bool> ResetPassword(ResetPasswordViewModel model)
    {
        try
        {
            EncryptionService enc = new EncryptionService();
            var encryptionHash1 = enc.Encrypt(model.Password!);
            var encryptionHash2 = enc.Encrypt(encryptionHash1);
            var result = await _userRepository
                .ResetPasswordDb(model.Token!, encryptionHash2);
            return true;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<bool> EditProfile(ProfileViewModel model, HttpContext hc)
    {
        var user = await _userRepository.EditUser(model, int.Parse(hc.User.Claims.First().Value));
        await hc.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        var result = await CreateCredentials(user, true, hc);
        if (result)
            return true;
        return false;
    }
}