using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlyPan.Models;
using OnlyPan.Models.ViewModels;

namespace OnlyPan.Services;

public class UserServices
{
  public bool checkEmail(OnlyPanContext context, string email)
  {
    var userdb =
      context.Usuarios.FromSqlRaw("EXECUTE sp_validate_email {0}", email).ToList();
    if (userdb.Count != 0)
    {
      return true;
    }

    return false;
  }

  public static List<Usuario> validateUser(OnlyPanContext context, string email, string password)
  {
    var userdb =
      context.Usuarios.FromSqlRaw("EXECUTE sp_validate_user {0}, {1}", email, password).ToList();
    if (userdb.Count == 0)
    {
      return null;
    }

    return userdb;
  }

  public async Task<bool> registerUser(OnlyPanContext context, RegisterViewModel model)
   {
    try
    {
      EncryptionService ecr = new EncryptionService();
      var encryptionKey1 = ecr.Encrypt(model.Contrasena);
      var encryptionKey2 = ecr.Encrypt(encryptionKey1);
      var user = new Usuario()
      {
        FechaInscrito = DateTime.UtcNow,
        Nombre = model.Nombre,
        Correo = model.Correo,
        Contrasena = encryptionKey2,
        Foto = GetPhoto(Directory.GetCurrentDirectory()+"/Utilities/Images/default.jpeg"),
        Estado = 1
      };
      context.Add(user);
      await context.SaveChangesAsync();
      return true;
    }
    catch (SystemException e)
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
        new Claim(ClaimTypes.NameIdentifier, user.IdUsuario.ToString()),
        new Claim(ClaimTypes.Email, user.Correo),
        new Claim(ClaimTypes.Name, user.Nombre),
        new Claim(ClaimTypes.Role, user.Rol.ToString())
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
    catch (Exception e)
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
      var usr = validateUser(context, model.Correo, encryptionHash2)[0];
      var result = await CreateCredentials(usr, model.Remember, hc);
      if(result)
        return true;
      return false;
    }
    catch (SystemException e)
    {
      return false;
    }
  }
  public async Task<bool> editProfile(OnlyPanContext context, ProfileViewModel model, HttpContext hc)
   {
    try
    {
      var user = context.Usuarios.Find(int.Parse(hc.User.Claims.First().Value));
      if (model.Email != user?.Correo && model.Email != null)
        user!.Correo = model.Email;
        //TODO send a email for confirm email change
        //TODO Move this method to the Utilities Folder for better abstraction
        if (model.Photo != null && model.Photo.Length > 0)
        {
          using(var memoryStream = new MemoryStream())
          {
            await model.Photo.CopyToAsync(memoryStream);
            user!.Foto = memoryStream.ToArray();
          }
        }
      if(model.Name != user!.Nombre && model.Name != null)
        user.Nombre = model.Name;
      await hc.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
      context.Update(user);
      await context.SaveChangesAsync();
      var result = await CreateCredentials(user, true, hc);
      if (result)
        return true;
      return false;
    }
    catch (SystemException e)
    {
      return false;
    }
  }
  //TODO move this function to Utilities
public static byte[] GetPhoto(string filePath)  
{  
  FileStream stream = new FileStream(  
      filePath, FileMode.Open, FileAccess.Read);  
  BinaryReader reader = new BinaryReader(stream);  
  
  byte[] photo = reader.ReadBytes((int)stream.Length);  
  
  reader.Close();  
  stream.Close();  
  
  return photo;  
}  
}