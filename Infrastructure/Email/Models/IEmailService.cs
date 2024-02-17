namespace Infrastructure.Email.Models;

public interface IEmailService
{
    public Task SendEmailAsync(EmailMessage message, List<FileAttachment> attachments);
}