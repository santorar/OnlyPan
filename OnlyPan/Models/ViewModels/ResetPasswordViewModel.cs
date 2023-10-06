using System.ComponentModel.DataAnnotations;

namespace OnlyPan.Models.ViewModels;

public class ResetPasswordViewModel
{
  [Required,Display(Name = "Correo Electronico")]public string? Email { get; set; }
}