using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlyPan.Models;

namespace OnlyPan.Controllers;

public class HomeController : Controller
{
  private readonly ILogger<HomeController> _logger;
  private readonly OnlyPanContext _context;

  public HomeController(ILogger<HomeController> logger, OnlyPanContext context)
  {
    _logger = logger;
    _context = context;
  }

  public IActionResult Index()
  {
    return View();
  }

  public IActionResult Bio()
  {
    return View();
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }

  public async Task<IActionResult> Feed()
  {
    List<Recetum> recipes = await _context.Receta.ToListAsync();
    return View(recipes);
  }
}