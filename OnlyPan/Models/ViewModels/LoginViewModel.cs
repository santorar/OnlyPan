using System.ComponentModel.DataAnnotations;

namespace OnlyPan.Models.ViewModels;

public class LoginViewModel
{
  [DataType(DataType.EmailAddress),Required,Display(Name = "Correo electronico")] public string? Email { get; set; }
  [DataType(DataType.Password),Required,Display(Name = "Contraseña")] public string? Password { get; set; }
  [Display(Name = "Recordar Sesion")] public bool Remember { get; set; }
}