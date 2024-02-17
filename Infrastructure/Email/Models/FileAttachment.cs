namespace Infrastructure.Email.Models;

public class FileAttachment
{
    public string FileName { get; set; }
    public byte[] Data { get; set; }
    public string ContentType { get; set; }

    public FileAttachment(string fileName, byte[] data, string contentType)
    {
        FileName = fileName;
        Data = data;
        ContentType = contentType;
    }
}