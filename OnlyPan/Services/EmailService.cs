using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using OnlyPan.Models;

namespace OnlyPan.Services;

public class EmailService
{
  public static async Task SendEmail(string email, string subject, string body)
  {
    var e = new MimeMessage();
    e.From.Add(MailboxAddress.Parse("OnlyPan.Notify@gmail.com"));
    e.To.Add(MailboxAddress.Parse(email));
    e.Subject = subject;
    e.Body = new TextPart(TextFormat.Html) { Text = body };
    using var smtp = new SmtpClient();
    await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
    await smtp.AuthenticateAsync("OnlyPan.Notify@gmail.com", "vogmdjftudbqcrov");
    await smtp.SendAsync(e);
    await smtp.DisconnectAsync(true);
  }

  public async Task SendVerificationEmail(int idUser, OnlyPanContext context)
  {
    string activationCode = Guid.NewGuid().ToString();
    Usuario? user = await context.Usuarios.FindAsync(idUser);
    user!.CodigoActivacion = activationCode;
    await context.SaveChangesAsync();
    string body = "Hola " + user.Nombre + "<br> <br>";
    body += "Por favor ingresa al siguiente link para activar tu cuenta<br>";
    body += "<a href=\"https://localhost:7077/User/Activate?code=" + activationCode + "\">Activa tu cuenta aqui</a>";
    await SendEmail(user.Correo, "Activa tu cuenta en OnlyPan", body);
  }
}