using OnlyPan.Models;
using OnlyPan.Services;

namespace OnlyPan.Controllers;

public class AdminController
{
    AdminServices _adminServices;

    public AdminController(OnlyPanContext context)
    {
        _adminServices = new AdminServices(context);
    }
}