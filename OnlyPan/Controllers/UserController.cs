using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
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
    var us = new UserServices();
    var result = await us.LoginUsuario(_context,user,HttpContext);
    if (result)
    {
      return RedirectToAction("Index", "Home");
    }

    ViewBag.Error = "Datos incorrectos";
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
      var us = new UserServices();
      if (us.checkEmail(_context, model.Correo))
      {
        ViewBag.Error = "Email ya registrado";
        return View(model);
      }

      var result = await us.registerUser(_context, model);
      if (!result)
      {
        ViewBag.Error = "Error intentelo denuevo";
        return View(model);
      }

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
  
  [Authorize]
  public async Task<IActionResult> Profile()
  {
    var user = await _context.Usuarios
      .Include(u => u.RolNavigation)
      .Where(u => u.IdUsuario == int.Parse(HttpContext.User.Claims.First().Value)).ToListAsync();
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
  //TODO fix the image is not uploading correctly
  public async Task<IActionResult> EditProfile(ProfileViewModel model)
  {
    var user = await _context.Usuarios.FindAsync(int.Parse(HttpContext.User.Claims.First().Value));
    var rol = await _context.Rols.FindAsync(user?.Rol);
    ViewData["Name"] = user?.Nombre;
    ViewData["Email"] = user?.Correo;
    ViewData["Rol"] = rol?.NombreRol;
    var us = new UserServices();
    
    var result = await us.editProfile(_context, model, HttpContext);
    if (!result)
    {
      ViewBag.Error = "Error intentelo denuevo";
      return View(model);
    }
    ViewBag.Success = "Datos actualizados";
    return RedirectToAction(nameof(Profile));
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