using System.Security.Cryptography;
using System.Text;

namespace OnlyPan.Services;

public class EncryptionService
{
  public string Encrypt(string data)
  {
    StringBuilder sb = new StringBuilder();
    using (SHA512 hash = SHA512.Create())
    {
      Encoding enc = Encoding.UTF8;
      byte[] result = hash.ComputeHash(enc.GetBytes(data));
      foreach (byte b in result) sb.Append(b.ToString("x2"));
    }
    return sb.ToString();
  }
}