using Infrastructure.Email.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Infrastructure.Email;

public class EmailService : IEmailService
{
    private string From;
    private string SmtpServer;
    private int Port;
    private string UserName;
    private string Password;
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
        From = _configuration["EmailConfiguration:From"]!;
        SmtpServer = _configuration["EmailConfiguration:SmtpServer"]!;
        UserName = _configuration["EmailConfiguration:UserName"]!;
        Port = int.Parse(_configuration["EmailConfiguration:Port"]!);
        Password = _configuration["EmailConfiguration:Password"]!;
    }
    
    public async Task SendEmailAsync(EmailMessage message, List<FileAttachment> attachments)
    {
        var emailMessage = CreateEmailMessage(message, attachments);
        await Send(emailMessage);
    }

    private MimeMessage CreateEmailMessage(EmailMessage message, List<FileAttachment> attachments)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("no-reply", From));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;

        var bodyBuilder = new BodyBuilder
        {
            TextBody = message.Content
        };

        var multipart = new Multipart("mixed");
        emailMessage.Body = multipart;
        
        multipart.Add(bodyBuilder.ToMessageBody());
        foreach (var attachment in attachments)
        {
            var attachmentPart = new MimePart(attachment.ContentType)
            {
                Content = new MimeContent(new MemoryStream(attachment.Data), ContentEncoding.Default),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = attachment.FileName
            };

            multipart.Add(attachmentPart);
        }

        return emailMessage;
    }

    private async Task Send(MimeMessage mailMessage)
    {
        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(SmtpServer, Port, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(UserName, Password);

            await client.SendAsync(mailMessage);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            await client.DisconnectAsync(true);
            client.Dispose();
        }
    }
}