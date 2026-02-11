using Application.Abstractions;
using AutoMapper;
using MimeKit;
using System.IO;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Database;
using Portal.Models;

namespace Application.Commands.Emails
{
    public class SendEmailCommand : IParameterDbCommand<MailRequestModel>
    {
        private readonly MailSettings _mailSettings;
        public SendEmailCommand(MailSettings mailSettings)
        {
            _mailSettings = mailSettings;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, MailRequestModel parameter)
        {
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(_mailSettings.Mail));
            email.To.Add(MailboxAddress.Parse(parameter.EmailTo));
            email.Subject = parameter.Subject;
            var builder = new BodyBuilder();
            if (parameter.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in parameter.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = parameter.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
