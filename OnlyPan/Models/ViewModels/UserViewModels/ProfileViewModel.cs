namespace OnlyPan.Models.ViewModels.UserViewModels;

public class ProfileViewModel
{
    public int UserId { get; set; }
    public string? Rol { get; set; }

    public byte[]? Photo { get; set; }

    public string? Biography { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public int Followers { get; set; }
    public int Followed { get; set; }
    public bool isFollowed { get; set; }
    public List<ReplicsViewModel> Replics { get; set; }
}