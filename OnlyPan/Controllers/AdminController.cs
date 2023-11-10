using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlyPan.Models;
using OnlyPan.Models.ViewModels.AdminViewModels;
using OnlyPan.Models.ViewModels.RecipesViewModels;
using OnlyPan.Services;

namespace OnlyPan.Controllers;

[Authorize]
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

    [Authorize(Roles = "3")]
    public async Task<IActionResult> Comments()
    {
        List<ReportedCommentViewModel> model = await _adminServices.GetReportedComments();
        return View(model);
    }

    [Authorize(Roles = "3")]
    public async Task<IActionResult> BlockComment(int idComment)
    {
        var result = await _adminServices.BlockComment(idComment);
        if (result)
            TempData["Success"] = "Comentario bloqueado con exito";
        else
            TempData["Error"] = "Error al bloquear el comentario";
        return RedirectToAction(nameof(Comments));
    }

    [Authorize(Roles = "3")]
    public async Task<IActionResult> AcceptComment(int idComment)
    {
        var result = await _adminServices.AcceptComment(idComment);
        if (result)
            TempData["Success"] = "Comentario aceptado con exito";
        else
            TempData["Error"] = "Error al aceptar el comentario";
        return RedirectToAction(nameof(Comments));
    }
    [Authorize(Roles = "3")]
    public async Task<IActionResult> Donations()
    {
        List<DonationsViewModel> model = await _adminServices.GetDonations();
        return View(model);
    }

    public IActionResult ViewDonationPayment(string photo)
    {
        if (string.IsNullOrEmpty(photo))
            return RedirectToAction(nameof(Donations));
        var model = new PaymentViewModel()
        {
            imagen = Convert.FromBase64String(photo)
        };
        return View(model);
    }
    [Authorize(Roles = "3")]
    public async Task<IActionResult> AcceptDonation(int donationId)
    {
        var result = await _adminServices.AcceptDonation(donationId);
        if (result)
            TempData["Success"] = "Donacion aceptada con exito";
        else
            TempData["Error"] = "Error al aceptar la donacion";
        return RedirectToAction(nameof(Donations));
    }

    public async Task<IActionResult> BlockDonation(int donationId)
    {
        var result = await _adminServices.BlockDonation(donationId);
        if(result)
            TempData["Success"] = "Donacion bloqueada con exito";
        else
            TempData["Error"] = "Error al bloquear la donacion";
        return RedirectToAction(nameof(Donations));
    }

    public async Task<IActionResult> Recipes()
    {
        List<RecipeModerateViewModel> model = await _adminServices.GetRecipes(4);
        var states = await _adminServices.GetStates(1);
        ViewData["States"] = new SelectList(states, "IdState", "State");
        if (model == null)
        {
            ViewData["Error"] = "Error al cargar las recetas";
            return RedirectToAction(nameof(Moderate));
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Recipes(int idState)
    {
        List<RecipeModerateViewModel> model = await _adminServices.GetRecipes(idState);
        var states = await _adminServices.GetStates(1);
        ViewData["States"] = new SelectList(states, "IdState", "State");
        if (model == null)
        {
            ViewData["Error"] = "Error al cargar las recetas";
            return RedirectToAction(nameof(Moderate));
        }
        return View(model);
    }
    
    public async Task<IActionResult> ViewRecipe(int idRecipe)
    {
        RecipeViewModel model = await _adminServices.GetRecipe(idRecipe);
        if(model == null) return RedirectToAction(nameof(Recipes));
        return View(model);
    }

    public async Task<IActionResult> AcceptRecipe(int idRecipe)
    {
        var result = await _adminServices.AcceptRecipe(idRecipe);
        if (result)
            ViewData["Success"] = "Receta aceptada con exito";
        else
            ViewData["Error"] = "Error al aceptar la receta";
        return RedirectToAction(nameof(Recipes));
    }
    public async Task<IActionResult> BlockRecipe(int idRecipe)
    {
        var result = await _adminServices.BlockRecipe(idRecipe);
        if (result)
            ViewData["Success"] = "Receta bloqueada con exito";
        else
            ViewData["Error"] = "Error al bloquear la receta";
        return RedirectToAction(nameof(Recipes));
    }
    
}