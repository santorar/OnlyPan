using System.ComponentModel.DataAnnotations;

namespace OnlyPan.Models.ViewModels;

public class ProfileViewModel
{
  [Required,Display(Name="Foto de perfil")]public string Foto { get; set; }
  [Required,Display(Name="Correo Electronico")]public string Correo { get; set; }
  [Required,Display(Name="Nombre de usuario")]public string Name { get; set; }
}