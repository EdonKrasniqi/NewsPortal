using Application.Abstractions;
using Core.Constants;
using Infrastructure.Database;
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
    public class ResetPasswordEmailCommand : IParammeterResultDbCommand<(string email, string token), bool>
    {
        private readonly SendEmailCommand _sendEmailCommand;

        public ResetPasswordEmailCommand(SendEmailCommand sendEmailCommand)
        {
            _sendEmailCommand = sendEmailCommand;
        }


        public async Task<bool> ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, (string email, string token) parameter)
        {
            var emailTemplateFile = "C:\\Users\\HP\\Desktop\\NewsWebsite\\Core\\Templates\\SendResetPasswordTemplate.html";
            StreamReader str = new StreamReader(emailTemplateFile);
            string emailTemplate = str.ReadToEnd();
            var builder = new BodyBuilder();
            builder.HtmlBody = emailTemplate;

            string clientBaseUrl = "http://localhost:3000";

            if (emailTemplate != null)
            {
                string resetLink = $"{clientBaseUrl}/reset-password?&token=" + parameter.token;

                builder.HtmlBody = emailTemplate.Replace("{{LINK}}", resetLink);
                var mailRequest = new MailRequestModel()
                {
                    Subject = Email.RestPasswordSubject,
                    EmailTo = parameter.email,
                    Body = builder.HtmlBody
                };

                await _sendEmailCommand.ExecuteAsync(CancellationToken.None, dbContext, saveChanges, mailRequest);

                return true;
            }

            return false;
        }
    }
}
