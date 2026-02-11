using Application.Abstractions;
using Core.Models;
using Infrastructure.Database;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Portal.Models;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Emails
{
    public class SendWelcomeEmail : IParameterDbCommand<WelcomeRequest>
    {
        private readonly MailSettings _mailSettings;
        public SendWelcomeEmail(MailSettings mailSettings)
        {
            _mailSettings = mailSettings;
        }

        public Task ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, WelcomeRequest parameter)
        {
            string FilePath = "C:\\Users\\HP\\Desktop\\NewsWebsite\\Core\\Templates\\WelcomeTemplate.html";
            // FilePath =Path.Combine(Core.Templates,)
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("[username]", parameter.UserName).Replace("[email]", parameter.EmailTo);
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(parameter.EmailTo));
            email.Subject = $"Welcome {parameter.UserName}";
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            smtp.Send(email);
            smtp.Disconnect(true);

            return Task.CompletedTask;
        }
    }
}
