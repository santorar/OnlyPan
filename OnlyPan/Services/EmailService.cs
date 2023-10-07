using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

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

  public async Task SendVerificationEmail(string email, string name, string activationToken)
  {
    string body = "Hola " + name + "<br> <br>";
    body += "Por favor ingresa al siguiente link para activar tu cuenta: <br>";
    body += "<a href=\"https://localhost:7077/User/Activate?code=" + activationToken + "\">Activa tu cuenta aqui</a>";
    await SendEmail(email, "Activa tu cuenta en OnlyPan", body);
  }

  public async Task SendForgotPasswordEmail(string email, string name, string recoveryToken)
  {
    string body = "Hola " + name + "<br> <br>";
    body += "Para recuperar tu contraseña ingresa al siguiente link: <br>";
    body += "<a href=\"https://localhost:7077/User/ResetPassword?token=" + recoveryToken + "\">Recupera tu contraseña aqui</a>";
    await SendEmail(email, "Recupera tu contraseña en OnlyPan", body);
  }
}