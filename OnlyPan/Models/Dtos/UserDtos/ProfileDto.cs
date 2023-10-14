using OnlyPan.Models.Dtos.UserDtos;

namespace OnlyPan.Models.Dtos;

public class ProfileDto : UserDto
{
    
    public string? RoleName { get; set; }

    public int Followers { get; set; }
    
    public int Followed { get; set; }
}