using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlyPan.Models;

namespace OnlyPan.Controllers;

public class UserController : Controller
{
    private readonly OnlyPanContext _context;
    // GET
    public UserController(OnlyPanContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(int a)
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    public async Task<IActionResult> List()
    {
        var users = _context.Usuarios.Include(u => u.RolNavigation);
        return View(await users.ToListAsync());
    }
}