namespace Infrastructure.Pdf;

public interface IPdfService
{
    public Task<byte[]> CreatePdfFromHtmlAsync(string htmlContent);
}