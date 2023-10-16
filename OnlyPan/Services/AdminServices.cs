using OnlyPan.Models;
using OnlyPan.Repositories;

namespace OnlyPan.Services;

public class AdminServices
{
    private AdminRepositories _adminRepositories;

    public AdminServices(OnlyPanContext context)
    {
        _adminRepositories = new AdminRepositories(context);
    }
}