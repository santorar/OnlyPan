using System.ComponentModel.DataAnnotations;

namespace OnlyPan.Models.ViewModels.UserViewModels;

public class ProfileEditViewModel
{
  [Display(Name="Foto de perfil")]public IFormFile? Photo { get; set; }
  [Required,Display(Name="Correo Electronico")]public string? Email { get; set; }
  [Required,Display(Name="Nombre de usuario")]public string? Name { get; set; }
  [Required,Display(Name="Biografia")]public string? Biography { get; set; }
}