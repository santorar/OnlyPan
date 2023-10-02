using Microsoft.AspNetCore.Mvc;
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
    if(ModelState.IsValid){
      var user = new Usuario()
      {
        Nombre = model.Nombre,
        Correo = model.Correo,
        Contrasena = model.Contrasena
      };
      _context.Add(user);
      var result = await _context.SaveChangesAsync();
      {
        
      }
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
}