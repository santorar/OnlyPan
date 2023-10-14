using System.ComponentModel.DataAnnotations;

namespace OnlyPan.Models.ViewModels.UserViewModels;

public class ResetPasswordViewModel
{
  [Required, Display(Name="Contraseña nueva"), DataType(DataType.Password)]
  public string? Password { get; set; }
  [Required, Display(Name="Confirmar contraseña"), DataType(DataType.Password),Compare(nameof(Password))]
  public string? ConfirmPassword { get; set; }
  public string? Token { get; set; }
}