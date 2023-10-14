using System.ComponentModel.DataAnnotations;

namespace OnlyPan.Models.ViewModels.UserViewModels;

public class ForgotPasswordViewModel
{
  [Required,Display(Name = "Correo Electronico")]public string? Email { get; set; }
}