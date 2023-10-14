using System.ComponentModel.DataAnnotations;

namespace OnlyPan.Models.ViewModels.RolViewModels;

public class RoleMakePetitionViewModel
{
  [Display(Name = "Seleccione un rol")]public int Role { get; set; }
}