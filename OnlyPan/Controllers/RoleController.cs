using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlyPan.Models;
using OnlyPan.Models.ViewModels.RolViewModels;
using OnlyPan.Services;

namespace OnlyPan.Controllers;

[Authorize]
public class RoleController : Controller
{
    private readonly OnlyPanContext _context;
    private readonly RoleServices _roleServices;

    public RoleController(OnlyPanContext context)
    {
        _context = context;
        _roleServices = new RoleServices(context);
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Petition()
    {
        var roles = await  _roleServices.GetRoles();
        ViewData["Roles"] = new SelectList(roles, "IdRol", "NombreRol");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Petition(RoleMakePetitionViewModel model)
    {
        var idUser = int.Parse(HttpContext.User.Claims.First().Value);
        if (await _roleServices.CheckPetitions(idUser,model.Role))
        {
            var roles = await _roleServices.GetRoles();
            ViewData["Roles"] = new SelectList(roles, "IdRol", "NombreRol");
            ViewBag.Error = "Solicitud ya realizada anteriormente";
            return View(model);
        }
        if (!await _roleServices.MakePetition(model, idUser))
        {
            var roles = await _roleServices.GetRoles();
            ViewData["Roles"] = new SelectList(roles, "IdRol", "NombreRol");
            ViewBag.Error = "Error en el servidor, intentelo mas tarde";
            return View(model);
        }

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
        List<RolePetitionViewModel> model = await _roleServices.GetPetitions();
        return View(model);
    }

    public async Task<IActionResult> AcceptPetition(int idUser, int idRol)
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