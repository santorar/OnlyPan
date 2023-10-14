using System.ComponentModel.DataAnnotations;

namespace OnlyPan.Models.ViewModels.UserViewModels;

public class RegisterViewModel
{
  [Display(Name = "Nombre de usuario")] public string? Name { get; set; }

  [Display(Name = "Correo"), DataType(DataType.EmailAddress)] public string? Email { get; set; }

  [Display(Name = "Contraseña"), DataType(DataType.Password)] public string? Password { get; set; }
    
  [Display(Name = "Confirmar contraseña"), DataType(DataType.Password), Compare(nameof(Password))]
  public string? ConfirmPassword { get; set; }
}