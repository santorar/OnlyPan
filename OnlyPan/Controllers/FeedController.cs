using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlyPan.Controllers;

[Authorize]
public class FeedController : Controller
{
  // GET
  public IActionResult Index()
  {
    return View();
  }
}