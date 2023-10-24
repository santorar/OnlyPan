using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlyPan.Models;

namespace OnlyPan.Controllers;

public class HomeController : Controller
{
  private readonly ILogger<HomeController> _logger;
  private readonly OnlyPanDbContext _dbContext;

  public HomeController(ILogger<HomeController> logger, OnlyPanDbContext dbContext)
  {
    _logger = logger;
    _dbContext = dbContext;
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
}