using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlyPan.Models;
using OnlyPan.Models.ViewModels.AdminViewModels;
using OnlyPan.Services;

namespace OnlyPan.Controllers;

public class AdminController : Controller
{
    AdminServices _adminServices;

    public AdminController(OnlyPanContext context)
    {
        _adminServices = new AdminServices(context);
    }

    //Moderate View for Rol petition
    [Authorize(Roles = "2,3")]
    public IActionResult Moderate()
    {
        return View();
    }

    public async Task<IActionResult> Comments()
    {
        List<ReportedCommentViewModel> model = await _adminServices.GetReportedComments();
        return View(model);
    }

    public async Task<IActionResult> BlockComment(int idComment)
    {
        var result = await _adminServices.BlockComment(idComment);
        if (result)
            ViewData["Success"] = "Comentario bloqueado con exito";
        else
            ViewData["Error"] = "Error al bloquear el comentario";
        return RedirectToAction(nameof(Comments));
    }

    public async Task<IActionResult> AcceptComment(int idComment)
    {
        var result = await _adminServices.AcceptComment(idComment);
        if (result)
            ViewData["Success"] = "Comentario aceptado con exito";
        else
            ViewData["Error"] = "Error al aceptar el comentario";
        return RedirectToAction(nameof(Comments));
    }
}