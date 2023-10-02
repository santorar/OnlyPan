using System.ComponentModel.DataAnnotations;

namespace OnlyPan.Models.ViewModels;

public class RolPetitionViewModel
{
  [DataType(DataType.Text),Display(Name = "Seleccione un rol")]public string Rol { get; set; }
  [DataType(DataType.Text),Display(Name = "Justifique por que quiere el rol")]public string Comentario { get; set; }
}