using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlyPan.Models;
using OnlyPan.Models.Dtos.RecipesDtos;
using OnlyPan.Models.ViewModels.RecipesViewModels;
using OnlyPan.Services;

namespace OnlyPan.Controllers;

public class RecipesController : Controller
{
    private readonly RecipesServices _recipesServices;

    public RecipesController(OnlyPanDbContext dbContext)
    {
        _recipesServices = new RecipesServices(dbContext);
    }

    // GET
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        List<RecipeFeedViewModel> model = await _recipesServices.GetRecipes();
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Search(string searchText)
    {
        List<RecipeFeedViewModel> model = await _recipesServices.SearchRecipes(searchText);
        if (model == null)
        {
            ViewData["Error"] = "Parametro de busqueda no valido";
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    [Authorize(Roles = "2,3,4")]
    public async Task<IActionResult> Create()
    {
        var categories = await _recipesServices.GetCategories();
        var ingredients = await _recipesServices.GetIngredients();
        var tags = await _recipesServices.GetTags();
        var units = await _recipesServices.GetUnits();
        ViewData["Categories"] = new SelectList(categories, "IdCategory", "NameCategory");
        ViewData["Ingredients"] = new SelectList(ingredients, "IdIngredient", "NameIngredient");
        ViewData["Tags"] = new SelectList(tags, "IdTag", "NameTag");
        ViewData["Units"] = new SelectList(units, "IdUnit", "LongName");
        return View();
    }

    [Authorize(Roles = "2,3,4")]
    [HttpPost]
    public async Task<IActionResult> Create(RecipeCreateViewModel model)
    {
        var idUser = int.Parse(HttpContext.User.Claims.First().Value);
        if (!await _recipesServices.CreateRecipe(model, idUser))
        {
            var categories = await _recipesServices.GetCategories();
            var ingredients = await _recipesServices.GetIngredients();
            var tags = await _recipesServices.GetTags();
            var units = await _recipesServices.GetUnits();
            ViewData["Categories"] = new SelectList(categories, "IdCategory", "NameCategory");
            ViewData["Ingredients"] = new SelectList(ingredients, "IdIngredient", "NameIngredient");
            ViewData["Tags"] = new SelectList(tags, "IdTag", "NameTag");
            ViewData["Units"] = new SelectList(units, "IdUnit", "LongName");
            ViewBag.Error = "Error al crear la receta";
            return View(model);
        }

        return RedirectToAction(nameof(Index));
    }
    [Authorize]
    public async Task<IActionResult> View(int idRecipe)
    {
        var userId = int.Parse(HttpContext.User.Claims.First().Value);
        var ratingList = _recipesServices.GetRatingList();
        ViewData["Ratings"] = new SelectList(ratingList);
        RecipeViewModel? model = await _recipesServices.GetRecipe(idRecipe, userId);
        if (model == null) return RedirectToAction(nameof(Index));
        return View(model);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> RateRecipe(int recipeId, int pRating)
    {
        var userId = int.Parse(HttpContext.User.Claims.First().Value);
        var result = await _recipesServices.RateRecipe(recipeId, pRating, userId);
        if (!result)
            ViewBag.Error = "Error al valorar la receta";
        else
            ViewData["Success"] = "Receta valorada correctamente";
        return RedirectToAction(nameof(View), new { idRecipe = recipeId });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateComment(string comment, int recipeId)
    {
        int idUser = int.Parse(HttpContext.User.Claims.First().Value);
        var result = await _recipesServices.CreateComment(comment, recipeId, idUser);
        if (!result)
            ViewBag.Error = "Error al crear el comentario";
        else
            ViewData["Success"] = "Comentario creado correctamente";
        return RedirectToAction(nameof(View), new { idRecipe = recipeId });
    }

    [Authorize]
    public async Task<IActionResult> ReportComent(int commentId, int recipeId)
    {
        var search = await _recipesServices.SearchReportedComment(commentId);
        if (search)
        {
            ViewBag.Error = "El comentario ya ha sido reportado";
            return RedirectToAction(nameof(View), new { idRecipe = recipeId });
        }

        var result = await _recipesServices.ReportComment(commentId);
        if (!result)
            ViewBag.Error = "Error al reportar el comentario";
        else
            ViewData["Success"] = "Comentario reportado correctamente";
        return RedirectToAction(nameof(View), new { idRecipe = recipeId });
    }
    [Authorize]
    public IActionResult Donation()
    {
        return View();
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> MakeDonation(int recipeId, float amount)
    {
        var userId = int.Parse(HttpContext.User.Claims.First().Value);
        bool result = await _recipesServices.MakeDonation(recipeId, amount, userId);
        if (result)
        {
            DonationDto donationDto = await _recipesServices.GetDonation(recipeId, userId);
            return View(nameof(Donation), donationDto);
        }

        ViewBag.Error = "Error al realizar la donación";
        return RedirectToAction(nameof(View), new { idRecipe = recipeId});
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CheckoutDonation(IFormFile comprobante, int recipeId, int donationId)
    {
        bool result = await _recipesServices.CheckoutDonation(donationId, comprobante);
        if(result)
            return RedirectToAction(nameof(View), new {idRecipe = recipeId});
        ViewBag.Error = "Error al subir la imagen intentalo denuevo";
        DonationDto donationDto = await _recipesServices.GetDonation(donationId,recipeId,0);
        return View(nameof(Donation), donationDto);
    }

    [Authorize]
    public async Task<IActionResult> Replicate(int recipeId)
    {
        int idUser = int.Parse(HttpContext.User.Claims.First().Value);
        var result = await _recipesServices.ReplicateRecipe(recipeId, idUser);
        if (result)
            ViewData["Success"] = "Receta replicada con exito";
        else
            ViewData["Error"] = "Error al replicar la receta";
        return RedirectToAction(nameof(View), new { idRecipe = recipeId });
    }
}