using System.ComponentModel.DataAnnotations;

namespace OnlyPan.Models.ViewModels;

public class RolPetitionViewModel
{
  [Display(Name = "Seleccione un rol")]public int Rol { get; set; }
}