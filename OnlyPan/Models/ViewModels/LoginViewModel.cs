using System.ComponentModel.DataAnnotations;

namespace OnlyPan.Models.ViewModels;

public class LoginViewModel
{
  [DataType(DataType.EmailAddress),Required,Display(Name = "Correo electronico")]public string Correo;
  [DataType(DataType.Password),Required,Display(Name = "Contraseña")]public string Contra;
}