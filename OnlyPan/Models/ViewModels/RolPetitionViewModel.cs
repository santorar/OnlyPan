using System.ComponentModel.DataAnnotations;

namespace OnlyPan.Models.ViewModels;

public class RolPetitionViewModel
{
  [Display(Name = "Seleccione un rol")]public int Rol { get; set; }
  [DataType(DataType.Text),Display(Name = "Justifique por que quiere el rol")]public string Comentario { get; set; }
}