using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlyPan.Models;
using OnlyPan.Models.ViewModels.AdminViewModels;
using OnlyPan.Services;

namespace OnlyPan.Controllers;

[Authorize(Roles = "2,3")]
public class AdminController : Controller
{
    AdminServices _adminServices;

    public AdminController(OnlyPanDbContext dbContext)
    {
        _adminServices = new AdminServices(dbContext);
    }

    //Moderate View for Rol petition
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
    public async Task<IActionResult> Donations()
    {
        List<DonationsViewModel> model = await _adminServices.GetDonations();
        return View(model);
    }
    public async Task<IActionResult> AcceptDonation(int donationId)
    {
        var result = await _adminServices.AcceptDonation(donationId);
        if (result)
            ViewData["Success"] = "Donacion aceptada con exito";
        else
            ViewData["Error"] = "Error al aceptar la donacion";
        return RedirectToAction(nameof(Donations));
    }

    public async Task<IActionResult> BlockDonation(int donationId)
    {
        var result = await _adminServices.BlockDonation(donationId);
        if(result)
            ViewData["Success"] = "Donacion bloqueada con exito";
        else
            ViewData["Error"] = "Error al bloquear la donacion";
        return RedirectToAction(nameof(Donations));
    }
}