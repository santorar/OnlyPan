using Microsoft.AspNetCore.Mvc;
using OnlyPan.Models;

namespace OnlyPan.Controllers;

public class RecipesController : Controller
{
  private readonly OnlyPanContext _context;

  public RecipesController(OnlyPanContext context)
  {
    _context = context;
  }
  // GET
  public IActionResult Index()
  {
    return View();
  }
  public IActionResult Create()
  {
    return View();
  }
}