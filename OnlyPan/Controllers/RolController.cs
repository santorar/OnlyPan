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
    var request = _context.SolicitudRols.FromSqlRaw("EXECUTE sp_check_petitions {0}", idUser).ToList();
    if (request.Count > 0)
    {
      ViewBag.Error = "Solicitudes en curso";
      return View(model);
    }
  var peticion = new SolicitudRol()
    {
      UsuarioSolicitud = idUser,
      Comentario = model.Comentario,
      RolSolicitado = model.Rol,
      EstadoSolicitud = 4,
      FechaSolicitud = DateTime.UtcNow,
    };
    _context.Add(peticion);
    var result = await _context.SaveChangesAsync();
    return RedirectToAction(nameof(Index));
  }

  [Authorize(Roles = "2,3")]
  public IActionResult Moderate()
  {
    return View();
  }
}