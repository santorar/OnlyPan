using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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


//Register Views and controller
  public IActionResult Register()
  {
    return View();
  }

  [ValidateAntiForgeryToken]
  [HttpPost]
  public async Task<IActionResult> Register(RegisterViewModel model)
  {
    if (ModelState.IsValid)
    {
      var us = new UserServices();
      if (await us.CheckEmail(_context, model.Email))
      {
        ViewBag.Error = "Este Email Ya se Encuentra Registrado. Inicia Sesión.";
        return View(model);
      }

      var result = await us.RegisterUser(_context, model);
      if (!result)
      {
        ViewBag.Error = "Error intantelo de nuevo";
        return View(model);
      }

      ViewData["Success"] = "Cuenta Creada, Ingresa a Tu Correo Para Verificarla y Activarla En El Sistema";
      return View(nameof(Login));
    }

    return View(model);
  }

  public async Task<IActionResult> Activate()
  {
    var activationCode = Request.Query["code"].ToString();
    var us = new UserServices();
    var result = await us.ActivateAccount(_context, activationCode);
    if (!result)
    {
      ViewBag.Error = "Codigo de activacion invalido";
      return RedirectToAction(nameof(Login));
    }

    ViewData["Success"] = "Cuenta Activada Exitosamente";
    return View(nameof(Login));
  }

  //Login Views and controller
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
    var us = new UserServices();
    var result = await us.LoginUsuario(_context, user, HttpContext);
    if (result)
    {
      return RedirectToAction("Index", "Home");
    }

    ViewBag.Error = "Los Datos Son Incorrectos";
    return View();
  }

  public IActionResult ForgotPassword()
  {
    return View();
  }
  [HttpPost]
  public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
  {
    UserServices sc = new UserServices();
    var result = await sc.ForgotPassword(_context, model.Email);
    if (!result)
    {
      ViewBag.Error = "Error, Intentalo De Nuevo";
      return View(model);
    }
    ViewData["Success"] = "Verifique su correo electronico, para seguir con el proceso de recuperacion";
    return View(nameof(Login));
  }
  public async Task<IActionResult> ResetPassword()
  {
    string recoveryToken = Request.Query["token"].ToString();
    UserServices us = new UserServices();
    var result = await us.RecoveryValidation(_context, recoveryToken);
    if (!result)
    {
      ViewBag.Error = "Token invalido";
      return View(nameof(Login));
    }

    ViewData["Token"] = recoveryToken;
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
  {
    UserServices sc = new UserServices();
    var result = await sc.ResetPassword(_context, model);
    if (!result)
    {
      ViewBag.Error = "Error intentalo de Nuevo";
      return View(model);
    }
    ViewData["Success"] = "Contraseña actualizada";
    return View(nameof(Login));
  }

  public async Task<IActionResult> Logout()
  {
    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return RedirectToAction("Index", "Home");
  }
  //TODO Create a method to restore the password

  [Authorize]
  public async Task<IActionResult> Profile()
  {
    var users = await _context.Usuarios
      .Include(u => u.RolNavigation)
      .Where(u => u.IdUsuario == int.Parse(HttpContext.User.Claims.First().Value)).ToListAsync();
    Usuario user = users.First();
    return View(user);
  }

  [Authorize]
  public async Task<IActionResult> EditProfile()
  {
    var user = await _context.Usuarios.FindAsync(int.Parse(HttpContext.User.Claims.First().Value));
    var rol = await _context.Rols.FindAsync(user?.Rol);
    ViewData["Name"] = user?.Nombre;
    ViewData["Email"] = user?.Correo;
    ViewData["Rol"] = rol?.NombreRol;
    return View();
  }

  [Authorize]
  [HttpPost]
  public async Task<IActionResult> EditProfile(ProfileViewModel model)
  {
    var user = await _context.Usuarios.FindAsync(int.Parse(HttpContext.User.Claims.First().Value));
    var rol = await _context.Rols.FindAsync(user?.Rol);
    ViewData["Name"] = user?.Nombre;
    ViewData["Email"] = user?.Correo;
    ViewData["Rol"] = rol?.NombreRol;
    var us = new UserServices();

    var result = await us.EditProfile(_context, model, HttpContext);
    if (!result)
    {
      ViewBag.Error = "Error, intentalo De Nuevo";
      return View(model);
    }

        ViewData["Success"] = "Datos actualizados";
    return View(nameof(Profile), user);
  }

  [Authorize(Roles = "2,3")]
  public async Task<IActionResult> ViewProfileRol(int idPetition)
  {
    var solicitud = await _context.SolicitudRols
      .Include(s => s.UsuarioSolicitudNavigation)
      .Include(s => s.RolSolicitadoNavigation)
      .Where(s => s.IdSolicitud == idPetition).ToListAsync();
    return View(solicitud);
  }
}