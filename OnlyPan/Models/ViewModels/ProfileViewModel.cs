using System.ComponentModel.DataAnnotations;

namespace OnlyPan.Models.ViewModels;

public class ProfileViewModel
{
  [Display(Name="Foto de perfil")]public IFormFile? Photo { get; set; }
  [Required,Display(Name="Correo Electronico")]public string? Email { get; set; }
  [Required,Display(Name="Nombre de usuario")]public string? Name { get; set; }
}