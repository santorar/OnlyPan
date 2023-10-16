using OnlyPan.Models;

namespace OnlyPan.Repositories;

public class AdminRepositories
{
    private OnlyPanContext _context;

    public AdminRepositories(OnlyPanContext context)
    {
        _context = context;
    }
}