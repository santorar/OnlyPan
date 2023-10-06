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
        ViewBag.Error = "Email ya registrado";
        return View(model);
      }

      var result = await us.RegisterUser(_context, model);
      if (!result)
      {
        ViewBag.Error = "Error intentelo denuevo";
        return View(model);
      }

      ViewData["Success"] = "Cuenta creada, ingrese a su correo para activarla";
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

    user.Activo = true;
    await _context.SaveChangesAsync();
    ViewData["Success"] = "Cuenta activada";
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
      ViewBag.Error = "Error intentelo denuevo";
      return View(model);
    }

    ViewBag.Success = "Datos actualizados";
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