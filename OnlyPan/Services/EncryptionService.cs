using System.Security.Cryptography;
using System.Text;

namespace OnlyPan.Services;

public class EncryptionService
{
  public string Encrypt(string data)
  {
    StringBuilder Sb = new StringBuilder();
    using (SHA256 hash = SHA256Managed.Create())
    {
      Encoding enc = Encoding.UTF8;
      byte[] result = hash.ComputeHash(enc.GetBytes(data));
      foreach (byte b in result) Sb.Append(b.ToString("x2"));
    }

    return Sb.ToString();
  }
}