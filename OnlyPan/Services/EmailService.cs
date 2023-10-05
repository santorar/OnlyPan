using System.Net;
using System.Net.Mail;
using OnlyPan.Models;

namespace OnlyPan.Services;

public class EmailService
{
  public static void SendEmail(string email, string subject, string body)
  {
    using (MailMessage mm = new MailMessage("onlipan.notificaciones@gmail.com", email))
    {
      mm.Subject = subject;
      mm.Body = body;
      mm.IsBodyHtml = true;
      SmtpClient smtp = new SmtpClient();
      smtp.Host = "smtp.gmail.com";
      smtp.EnableSsl = true;
      NetworkCredential nc = new NetworkCredential("onlipan.notificaciones@gmail.com", "OnlyPan2023");
      smtp.UseDefaultCredentials = true;
      smtp.Credentials = nc;
      smtp.Port = 587;
      smtp.Send(mm);

    }
  }

  public async Task SendVerificationEmail(int idUser, OnlyPanContext context)
  {
    string activationCode = Guid.NewGuid().ToString();
    Usuario? user = await context.Usuarios.FindAsync(idUser);
    user!.CodigoActivacion = activationCode;
    await context.SaveChangesAsync();
    string body = "Hola " + user.Nombre + "<br>";
    body += "Por favor ingresa al siguiente link para activar tu cuenta<br>";
    body += "<a href='localhost:7077/User/Activate?code=" + activationCode + "'>Activa tu cuenta aqui</a>";
    SendEmail(user.Nombre, "Activa tu cuenta en OnlyPan", body);
  }
}