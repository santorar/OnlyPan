using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlyPan.Models;
using OnlyPan.Models.ViewModels.RecipesViewModels;
using OnlyPan.Services;

namespace OnlyPan.Controllers;

public class RecipesController : Controller
{
    private readonly RecipesServices _recipesServices;

    public RecipesController(OnlyPanContext context)
    {
        _recipesServices = new RecipesServices(context);
    }

    // GET
    public async Task<IActionResult> Index()
    {
        List<RecipeFeedViewModel> model = await _recipesServices.GetRecipes();
        return View(model);
    }

    [Authorize(Roles = "2,3,4")]
    public async Task<IActionResult> Create()
    {
        var categories = await _recipesServices.GetCategories();
        var ingredients = await _recipesServices.GetIngredients();
        var tags = await _recipesServices.GetTags();
        var units = _recipesServices.GetUnits();
        ViewData["Categories"] = new SelectList(categories, "IdCategory", "NameCategory");
        ViewData["Ingredients"] = new SelectList(ingredients, "IdIngredient", "NameIngredient");
        ViewData["Tags"] = new SelectList(tags, "IdTag", "NameTag");
        ViewData["Units"] = new SelectList(units, "ShortName", "LongName");
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
            var units = _recipesServices.GetUnits();
            ViewData["Categories"] = new SelectList(categories, "IdCategory", "NameCategory");
            ViewData["Ingredients"] = new SelectList(ingredients, "IdIngredient", "NameIngredient");
            ViewData["Tags"] = new SelectList(tags, "IdTag", "NameTag");
            ViewData["Units"] = new SelectList(units, "ShortName", "LongName");
            ViewBag.Error = "Error al crear la receta";
            return View(model);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> View(int idRecipe)
    {
        RecipeViewModel model = await _recipesServices.GetRecipe(idRecipe);
        return View(model);
    }

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

    public async Task<IActionResult> ReportComent(int commentId, int recipeId)
    {
        var result = await _recipesServices.ReportComment(commentId);
        if (!result)
            ViewBag.Error = "Error al reportar el comentario";
        else
            ViewData["Success"] = "Comentario reportado correctamente";
        return RedirectToAction(nameof(View), new { idRecipe = recipeId });
    }
}