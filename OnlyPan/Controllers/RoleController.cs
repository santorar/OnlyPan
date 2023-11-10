using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlyPan.Models;
using OnlyPan.Models.ViewModels.RolViewModels;
using OnlyPan.Services;

namespace OnlyPan.Controllers;

[Authorize]
public class RoleController : Controller
{
    private readonly OnlyPanDbContext _dbContext;
    private readonly RoleServices _roleServices;

    public RoleController(OnlyPanDbContext dbContext)
    {
        _dbContext = dbContext;
        _roleServices = new RoleServices(dbContext);
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Petition()
    {
        var roles = await _roleServices.GetRoles();
        ViewData["Roles"] = new SelectList(roles, "IdRol", "NombreRol");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Petition(RoleMakePetitionViewModel model)
    {
        var idUser = int.Parse(HttpContext.User.Claims.First().Value);
        if (await _roleServices.CheckPetitions(idUser, model.Role))
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

        return RedirectToAction(nameof(Index), "Recipes");
    }


    //View a list of rol petitions 
    public async Task<IActionResult> ViewRolePetitions()
    {
        List<RolePetitionViewModel> model = await _roleServices.GetPetitions();
        return View(model);
    }

    public async Task<IActionResult> AcceptPetition(int idUser, int idRol)
    {
        int idUserAdmin = int.Parse(HttpContext.User.Claims.First().Value);
        if (await _roleServices.AcceptPetition(idUser, idRol, idUserAdmin))
        {
            ViewData["Success"] = "Solicitud aceptada con exito";
            return RedirectToAction("ViewRolePetitions");
        }

        ViewBag.Error = "Error al aceptar la solicitud";
        return RedirectToAction("ViewRolePetitions");
    }

    public async Task<IActionResult> RejectPetition(int idUser, int idRol)
    {
        int idUserAdmin = int.Parse(HttpContext.User.Claims.First().Value);
        if (await _roleServices.RejectPetition(idUser, idRol, idUserAdmin))
        {
            ViewData["Success"] = "Solicitud rechazada con exito";
            return RedirectToAction("ViewRolePetitions");
        }

        ViewBag.Error = "Error al rechazar la solicitud";
        return RedirectToAction("ViewRolePetitions");
    }
}