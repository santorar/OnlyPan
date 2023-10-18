using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using OnlyPan.Models;
using OnlyPan.Models.Dtos;
using OnlyPan.Models.Dtos.UserDtos;
using OnlyPan.Models.ViewModels;
using OnlyPan.Models.ViewModels.UserViewModels;
using OnlyPan.Repositories;
using OnlyPan.Utilities.Classes;
using SystemException = System.SystemException;

namespace OnlyPan.Services;

public class UserServices
{
    private readonly UserRepository _userRepository;

    public UserServices(OnlyPanContext context)
    {
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
                Name = model.Name!,
                Email = model.Email!,
                Password = encryptionKey2,
                Photo = pu.GetPhotoFromFile(Directory.GetCurrentDirectory() + "/Utilities/Images/default.jpeg"),
                ActivationToken = activationToken
            };
            var result = await _userRepository.RegisterUser(user);
            if (!result) return false;
            EmailService em = new EmailService();
            await em.SendVerificationEmail(user.Email, user.Name, activationToken);
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
                new(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
                new(ClaimTypes.Email, user.Email!),
                new(ClaimTypes.Name, user.Name!),
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
            return result;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<bool> ActivateAccount(string token)
    {
        var result = await _userRepository.ActivateUser(token);
        return result;
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
            return await _userRepository.RecoveryValidation(recoveryToken);
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
            EncryptionService enc = new EncryptionService();
            var encryptionHash1 = enc.Encrypt(model.Password!);
            var encryptionHash2 = enc.Encrypt(encryptionHash1);
            var result = await _userRepository
                .ResetPasswordDb(model.Token!, encryptionHash2);
            return result;
        }
        catch (SystemException)
        {
            return false;
        }
    }

    public async Task<ProfileViewModel> Profile(int idUser)
    {
        try
        {
            var user = await _userRepository.RequestProfile(idUser);
            return new ProfileViewModel()
            {
                UserId = idUser,
                Rol = user.RoleName,
                Photo = user.Photo,
                Biography = user.Biography,
                Name = user.Name,
                Email = user.Email,
                Followers = user.Followers,
                Followed = user.Followed
            };
        }
        catch (SystemException)
        {
            return null!;
        }
    }

    public async Task<ProfileViewModel> Profile(int idUserLogged, int idUser)
    {
        try
        {
            var followed = await IsFollowed(idUserLogged, idUser);
            var user = await _userRepository.RequestProfile(idUser);
            return new ProfileViewModel()
            {
                UserId = idUser,
                Rol = user.RoleName,
                Photo = user.Photo,
                Biography = user.Biography,
                Name = user.Name,
                Email = user.Email,
                Followers = user.Followers,
                Followed = user.Followed,
                isFollowed = followed
            };
        }
        catch (SystemException)
        {
            return null!;
        }
    }

    public async Task<ProfileDto> GetUserDataModel(int idUser)
    {
        try
        {
            return await _userRepository.RequestProfile(idUser);
        }
        catch (SystemException)
        {
            return null!;
        }
    }

    public async Task<bool> EditProfile(ProfileEditViewModel model, HttpContext hc)
    {
        var user = await _userRepository.EditUser(model, int.Parse(hc.User.Claims.First().Value));
        await hc.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        var result = await CreateCredentials(user, true, hc);
        if (result)
            return true;
        return false;
    }

    public async Task<ProfileRolViewModel> GetProfileRol(int idUser, int idRol)
    {
        try
        {
            var data = await _userRepository.RequestProfileRolData(idUser, idRol);
            return new ProfileRolViewModel()
            {
                IdUser = data.IdUser,
                IdRolNew = data.IdNewRol,
                CurrentRol = data.CurrentRol,
                NewRol = data.NewRol,
                Photo = data.Photo,
                Biography = data.Biography,
                Name = data.Name,
                Email = data.Email,
                Followers = data.Followers,
                Followed = data.Followed
            };
        }
        catch (SystemException)
        {
            return null!;
        }
    }

    public async Task<bool> IsFollowed(int idUserLogged, int idUser)
    {
        return await _userRepository.RequestFollow(idUserLogged, idUser);
    }

    public async Task<bool> Follow(int idUserLogged, int idUser)
    {
        return await _userRepository.FollowUser(idUserLogged, idUser);
    }

    public async Task<bool> UnFollow(int idUserLogged, int userId)
    {
        return await _userRepository.UnFollowUser(idUserLogged, userId);
    }
}