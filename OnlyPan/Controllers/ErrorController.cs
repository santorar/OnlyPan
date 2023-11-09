using Microsoft.AspNetCore.Mvc;

namespace OnlyPan.Controllers;

public class ErrorController : Controller
{
    // GET
    [Route("404")]
    public IActionResult PageNotFound()
    {
        return View();
    }
}