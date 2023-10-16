namespace OnlyPan.Utilities.Classes;

public class PhotoUtilities
{
  public async Task<byte[]> convertToBytes(IFormFile? photo)
  {
    using var ms = new MemoryStream();
    await photo.CopyToAsync(ms);
    return ms.ToArray();
  }
  public byte[] GetPhotoFromFile(string filePath)  
  {  
    FileStream stream = new FileStream(  
        filePath, FileMode.Open, FileAccess.Read);  
    BinaryReader reader = new BinaryReader(stream);  
    
    byte[] photo = reader.ReadBytes((int)stream.Length);  
    
    reader.Close();  
    stream.Close();  
    
    return photo;  
  }  
}