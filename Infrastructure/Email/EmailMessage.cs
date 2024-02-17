using MimeKit;

namespace Infrastructure.Email;

public class EmailMessage
{
    public List<MailboxAddress> To { get; set; }

    public string Subject { get; set; }

    public string Content { get; set; }

    public EmailMessage(IEnumerable<string> to, string subject, string content)
    {
        To = new List<MailboxAddress>();
        To.AddRange(to.Select(email => new MailboxAddress("email", email)));
        Subject = subject;
        Content = content;
    }
}