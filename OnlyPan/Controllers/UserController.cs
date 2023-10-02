using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
  //Login Views and controller
  // TODO move the operation to the utilities folder
  public IActionResult Login()
  {
    ClaimsPrincipal c = HttpContext.User;
    if (c.Identity != null)
    {
      if (c.Identity.IsAuthenticated)
      {
        return RedirectToAction("Index","Home");
      }
    }
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> Login(LoginViewModel user)
  {
    try
    {
      using (SqlConnection con = new(_context.Database.GetConnectionString()))
      {
        using (SqlCommand cmd = new("sp_validate_user",con))
        {
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = user.Correo;
          cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = user.Contra;
          con.Open();
          var dr = cmd.ExecuteReader();
          while (dr.Read())
          {
              List<Claim> c = new List<Claim>()
              {
                new Claim(ClaimTypes.Email, user.Correo),
                new Claim(ClaimTypes.Name, dr[4].ToString() ?? throw new InvalidOperationException()),
                new Claim(ClaimTypes.Role, dr[1].ToString() ?? throw new InvalidOperationException())
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
          ViewBag.Error = "Datos incorrectos o cuenta no registrada.";
          con.Close();
        }
      }
    }
    catch (SystemException e)
    {
      ViewBag.Error = "Conexion Fallida";
    }
    return View();
  }
    
  //Register Views
  public IActionResult Register()
  {
    return View();
  }
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

  public async Task<IActionResult> Logout()
  {
    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return RedirectToAction("Index", "Home");
  }
}