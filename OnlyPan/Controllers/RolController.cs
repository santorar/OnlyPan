using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlyPan.Models;
using OnlyPan.Models.ViewModels;
using OnlyPan.Services;

namespace OnlyPan.Controllers;

[Authorize]
public class RolController : Controller
{
  private readonly OnlyPanContext _context;

  public IActionResult Index()
  {
    return View();
  }

  public RolController(OnlyPanContext context)
  {
    _context = context;
  }

  public IActionResult Petition()
  {
    var roles = _context.Rols.Where(r => r.IdRol >= 2 && r.IdRol <= 4);
    ViewData["Roles"] = new SelectList(roles, "IdRol", "NombreRol");
    
    return View();
  }

  //TODO send a email for confirm registration
  [HttpPost]
  public async Task<IActionResult> Petition(RolPetitionViewModel model)
  {
    var roles = _context.Rols.Where(r => r.IdRol >= 2 && r.IdRol <= 4);
    ViewData["Roles"] = new SelectList(roles, "IdRol", "NombreRol");
    
    var idUser = int.Parse(HttpContext.User.Claims.First().Value);
    var rs = new RoleServices();
    if (rs.CheckPetitions(_context, idUser))
    {
      ViewBag.Error = "Solicitudes en curso";
      return View(model);
    }
    await rs.makePetition(model, _context, idUser);
    return RedirectToAction(nameof(Index));
  }

  [Authorize(Roles = "2,3")]
  public IActionResult Moderate()
  {
    return View();
  }

  public IActionResult ViewRole()
  {
    return View();
  }
}