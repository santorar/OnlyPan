using System.ComponentModel.DataAnnotations;

namespace OnlyPan.Models.ViewModels;

public class UsuarioViewModel
{
  [Display(Name = "Nombre de usuario")] public string Nombre { get; set; }

  [Display(Name = "Correo"), DataType(DataType.EmailAddress)] public string Correo { get; set; }

  [Display(Name = "Contraseña"), DataType(DataType.Password)] public string Contrasena { get; set; }
    
  [Display(Name = "Confirmar contraseña"), DataType(DataType.Password), Compare(nameof(Contrasena))] public string ConfirmarContra { get; set; }
}