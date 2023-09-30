using System.ComponentModel.DataAnnotations;

namespace OnlyPan.Models.ViewModels;

public class UserViewModel
{
  [Display(Name = "Nombre")] public string nombre { get; set; }

  [Display(Name = "Correo")] public string correo { get; set; }

  [Display(Name = "Contraseña")] public string contraseña { get; set; }
}