using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlyPan.Models;
using OnlyPan.Models.ViewModels;
using OnlyPan.Services;

namespace OnlyPan.Controllers;

public class UserController : Controller
{
  private readonly OnlyPanContext _context;

  public UserController(OnlyPanContext context)
  {
    _context = context;
  }

  //Login Views and controller
  // TODO move the operation to the utilities folder
  public IActionResult Login()
  {
    ClaimsPrincipal c = HttpContext.User;
    if (c.Identity != null)
    {
      if (c.Identity.IsAuthenticated)
      {
        return RedirectToAction("Index", "Home");
      }
    }

    return View();
  }

  [HttpPost]
  public async Task<IActionResult> Login(LoginViewModel user)
  {
    //TODO move the logic to the UserServices
    EncryptionService enc = new EncryptionService();
    var encryptionHash1 = enc.Encrypt(user.Contra);
    var encryptionHash2 = enc.Encrypt(encryptionHash1);
    try
    {
      var userdb =
        _context.Usuarios.FromSqlRaw("EXECUTE sp_validate_user {0}, {1}", user.Correo, encryptionHash2).ToList()[0] ??
        throw new SystemException();
      List<Claim> c = new List<Claim>()
      {
        new Claim(ClaimTypes.Sid, userdb.IdUsuario.ToString()),
        new Claim(ClaimTypes.Email, user.Correo),
        new Claim(ClaimTypes.Name, userdb.Nombre),
        new Claim(ClaimTypes.Role, userdb.Rol.ToString())
      };
      ClaimsIdentity ci = new(c, CookieAuthenticationDefaults.AuthenticationScheme);
      AuthenticationProperties p = new AuthenticationProperties();
      p.AllowRefresh = true;
      p.IsPersistent = user.Remember;

      if (user.Remember)
        p.ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1);
      else
        p.ExpiresUtc = DateTimeOffset.MaxValue;
      await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci), p);
      return RedirectToAction("Index", "Home");
    }
    catch (SystemException e)
    {
      ViewBag.Error = "Datos incorrectos o Usuario no registrado";
    }

    return View();
  }

//Register Views
  public IActionResult Register()
  {
    return View();
  }

  //TODO send a email for confirm registration
  [ValidateAntiForgeryToken]
  [HttpPost]
  public async Task<IActionResult> Register(RegisterViewModel model)
  {
    if (ModelState.IsValid)
    {
      EncryptionService ecr = new EncryptionService();
      var encryptionKey1 = ecr.Encrypt(model.Contrasena);
      var encryptionKey2 = ecr.Encrypt(encryptionKey1);
        var userdb =
          _context.Usuarios.FromSqlRaw("EXECUTE sp_validate_email {0}", model.Correo).ToList();
        if (userdb.Count != 0)
        {
          ViewBag.Error = "Email ya registrado";
          return View(model);
        }
      var user = new Usuario()
      {
        FechaInscrito = DateTime.UtcNow,
        Nombre = model.Nombre,
        Correo = model.Correo,
        Contrasena = encryptionKey2,
        Estado = 1
      };
      _context.Add(user);
      var result = await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Login));
    }

    return View(model);
  }

  public async Task<IActionResult> Logout()
  {
    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return RedirectToAction("Index", "Home");
  }
  //TODO Create a method to restore the password
}