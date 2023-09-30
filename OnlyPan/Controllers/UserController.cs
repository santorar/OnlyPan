using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlyPan.Models;
using OnlyPan.Models.ViewModels;

namespace OnlyPan.Controllers;

public class UserController : Controller
{
  private readonly OnlyPanContext _context;

  public UserController(OnlyPanContext context)
  {
    _context = context;
  }

  public IActionResult Index()
  {
    return View();
  }

  public IActionResult Register()
  {
    return View();
  }

  [ValidateAntiForgeryToken]
  [HttpPost]
  public async Task<IActionResult> Register(UsuarioViewModel model)
  {
    {
      var user = new Usuario()
      {
        Nombre = model.Nombre,
        Correo = model.Correo,
        Contraseña = model.Contrasena
      };
      _context.Add(user);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Login));
    }
    return View(model);
  }

  public IActionResult Login()
  {
    return View();
  }

  public async Task<IActionResult> List()
  {
    var users = _context.Usuarios.Include(u => u.RolNavigation);
    return View(await users.ToListAsync());
  }
}