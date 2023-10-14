using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlyPan.Models;
using OnlyPan.Models.ViewModels;
using OnlyPan.Models.ViewModels.RolViewModels;
using OnlyPan.Services;

namespace OnlyPan.Controllers;

[Authorize]
public class RoleController : Controller
{
  private readonly OnlyPanContext _context;

  public IActionResult Index()
  {
    return View();
  }

  public RoleController(OnlyPanContext context)
  {
    _context = context;
  }

  public IActionResult Petition()
  {
    var roles = _context.Rols.Where(r => r.IdRol >= 2 && r.IdRol <= 4);
    ViewData["Roles"] = new SelectList(roles, "IdRol", "NombreRol");

    return View();
  }

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

    await rs.MakePetition(model, _context, idUser);
    return RedirectToAction(nameof(Index));
  }

  //Moderate View for Rol petition
  [Authorize(Roles = "2,3")]
  public IActionResult Moderate()
  {
    return View();
  }

  //View a list of rol petitions 
  public async Task<IActionResult> ViewRole()
  {
    var petitions = _context.SolicitudRols
      .Include(u => u.IdUsuarioSolicitudNavigation)
      .Include(r => r.IdRolSolicitudNavigation)
      .Where(r => r.IdEstado == 4);
    return View(await petitions.ToListAsync());
  }

  public async  Task<IActionResult> AcceptPetition(int idUser, int idRol)
  {
    // take the petition from the database
    SolicitudRol? petition = await _context.SolicitudRols.FindAsync(idUser, idRol);
    if (petition != null)
    {
      petition.IdUsuarioAprovador = int.Parse(HttpContext.User.Claims.First().Value);
      petition.FechaAprovacion = DateTime.UtcNow;
      petition.IdEstado = 5;
      // Add the rol to the user
      Usuario? user = await _context.Usuarios.FindAsync(idUser);
      if (user != null)
      {
        user.Rol = petition.IdRolSolicitud;
        // Save the changes into the database
        await _context.SaveChangesAsync();
        return RedirectToAction("ViewRole");
      }
  }
    ViewBag.Error = "Error al aceptar la solicitud";
    return RedirectToAction("ViewRole");
  }

  public async Task<IActionResult> RejectPetition(int idUser, int idRol)
  {
    SolicitudRol? petition = await _context.SolicitudRols.FindAsync(idUser, idRol);
    if (petition != null)
    {
      petition.IdUsuarioAprovador = int.Parse(HttpContext.User.Claims.First().Value);
      petition.FechaAprovacion = DateTime.UtcNow;
      petition.IdEstado = 6;
      // Add the rol to the user
        // Save the changes into the database
        await _context.SaveChangesAsync();
      return RedirectToAction("ViewRole");
    }
    ViewBag.Error = "Error al rechazar la solicitud";
    return RedirectToAction("ViewRole");
  }
}