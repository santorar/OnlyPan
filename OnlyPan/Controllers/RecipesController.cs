using System.Net;
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
  public IActionResult Index()
  {
    return View();
  }
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

  [HttpPost]
  public async Task<IActionResult> Create(RecipeCreateViewModel model)
  {
    var idUser = int.Parse(HttpContext.User.Claims.First().Value);
    if(!await _recipesServices.CreateRecipe(model, idUser))
    {
      ViewBag.Error = "Error al crear la receta";
      return View(model);
    }

    return RedirectToAction(nameof(Index));
  }
}