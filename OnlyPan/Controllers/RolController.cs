using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlyPan.Models;
using OnlyPan.Models.ViewModels;
using OnlyPan.Services;

namespace OnlyPan.Controllers;
[Authorize]
public class RolController : Controller
{
  private readonly OnlyPanContext _context;

  public RolController(OnlyPanContext context)
  {
    _context = context;
  }
  public IActionResult Petition()
  {
    return View();
  }
  //TODO send a email for confirm registration
  [HttpPost]
  public async Task<IActionResult> Petition(RolPetitionViewModel model)
  {
    var id_user = HttpContext.User.Claims;
    var peticion = new SolicitudRol()
      {
        
        Comentario = model.Comentario,
        RolSolicitado= model.Rol,
        
      };
      _context.Add(peticion);
      var result = await _context.SaveChangesAsync();
    return View(model);
  }
}