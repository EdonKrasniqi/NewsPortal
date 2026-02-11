using Application.Abstractions;
using Core.Entities;
using Core.Models;
using Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Portal.ViewModels;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Commands.Emails;
using Portal.Models;
using Core.Constants;

namespace Application.Commands.Account
{
    public class RegisterCommad : IParammeterResultDbCommand<RegisterModel, UserModel>
    {
        private readonly UserManager<AppUserEntity> _userMenager;
        private readonly SendWelcomeEmail _sendWelcomeEmail;
        public RegisterCommad(UserManager<AppUserEntity> userMenager,
           SendWelcomeEmail sendWelcomeEmail)
        {
            _userMenager = userMenager;
            _sendWelcomeEmail = sendWelcomeEmail;
        }

        public async Task<UserModel> ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, RegisterModel parameter)
        {
            if (_userMenager.Users.Any(x => x.Email == parameter.Email))
            {
                throw new Exception(ErrorMessages.EmailIsTaken);
            }
            var user = new AppUserEntity()
            {
                Id = Guid.NewGuid(),
                Email = parameter.Email,
                UserName = parameter.Email.Split(Characters.At)[0]
            };

            var result = await _userMenager.CreateAsync(user, parameter.Password);
            if (result.Succeeded)
            {
                var WelcomeRequest = new WelcomeRequest() { EmailTo = parameter.Email, UserName = user.UserName };
                var mailRequest = new MailRequestModel()
                {
                    EmailTo = user.Email,
                    Subject = Email.WelcomSubject,
                    Body = Email.Body,
                };

                var currentregister = await _userMenager.Users
                    .FirstOrDefaultAsync(x => x.Email == parameter.Email);

                await _userMenager.AddToRoleAsync(currentregister, parameter.Role);

                var Usertoreturn = new UserModel()
                {
                    UserId = currentregister.Id,
                    UserName = currentregister.UserName,
                    Email = currentregister.Email,
                    Role = Roles.SimpleUser
                };

               // await _sendWelcomeEmail.ExecuteAsync(cancellationToken, dbContext, false, WelcomeRequest);

                return Usertoreturn;
            }
            throw new Exception(ErrorMessages.UserCanNotBeCreated);

        }

    }
}
