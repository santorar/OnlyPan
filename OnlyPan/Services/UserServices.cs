using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using OnlyPan.Models;
using OnlyPan.Models.ViewModels;
using OnlyPan.Utilities.Classes;

namespace OnlyPan.Services;

public class UserServices
{
  public async Task<bool> CheckEmail(OnlyPanContext context, string email)
  {
    //find by the column Correo
    var userdb = await context.Usuarios.Where(u => u.Correo == email).ToListAsync();
    if (userdb.Count != 0)
    {
      return true;
    }

    return false;
  }

  public static List<Usuario> ValidateUser(OnlyPanContext context, string email, string password)
  {
    var userdb =
      context.Usuarios.FromSqlRaw("EXECUTE sp_validate_user {0}, {1}", email, password).ToList();
    if (userdb.Count == 0)
    {
      return null!;
    }

    return userdb;
  }

  public async Task<bool> RegisterUser(OnlyPanContext context, RegisterViewModel model)
  {
    try
    {
      EncryptionService ecr = new EncryptionService();
      var encryptionKey1 = ecr.Encrypt(model.Contrasena);
      var encryptionKey2 = ecr.Encrypt(encryptionKey1);
      var pu = new PhotoUtilities();
      var user = new Usuario()
      {
        FechaInscrito = DateTime.UtcNow,
        Nombre = model.Nombre,
        Correo = model.Correo,
        Contrasena = encryptionKey2,
        Foto = pu.GetPhotoFromFile(Directory.GetCurrentDirectory() + "/Utilities/Images/default.jpeg"),
        Estado = 1
      };
      context.Add(user);
      await context.SaveChangesAsync();
      EmailService em = new EmailService();
      await em.SendVerificationEmail(user.IdUsuario, context);
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

  public async Task<bool> LoginUsuario(OnlyPanContext context, LoginViewModel model, HttpContext hc)
  {
    try
    {
      EncryptionService enc = new EncryptionService();
      var encryptionHash1 = enc.Encrypt(model.Contra);
      var encryptionHash2 = enc.Encrypt(encryptionHash1);
      var usr = ValidateUser(context, model.Correo, encryptionHash2)[0];
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

  public async Task<bool> EditProfile(OnlyPanContext context, ProfileViewModel model, HttpContext hc)
  {
    try
    {
      var user = context.Usuarios.Find(int.Parse(hc.User.Claims.First().Value));
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
      context.Update(user);
      await context.SaveChangesAsync();
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