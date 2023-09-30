using Microsoft.AspNetCore.Mvc;

namespace OnlyPan.Controllers;

public class UserController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Registro()
    {
        return View();
    }

    public IActionResult InicioSesion()
    {
        return View();
    }

    public IActionResult Listado()
    {
        return View();
    }
}