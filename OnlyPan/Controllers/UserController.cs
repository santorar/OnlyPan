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
  //Register Views
  public IActionResult Register()
  {
    return View();
  }
  //Register a user into the platform
  //TODO make a way to encrypt the password of the user and his personal data directy in the register
  [ValidateAntiForgeryToken]
  [HttpPost]
  public async Task<IActionResult> Register(RegisterViewModel model)
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

  //Login Views and controller
  //TODO make a method to log in and authenticate the user
  [HttpGet]
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