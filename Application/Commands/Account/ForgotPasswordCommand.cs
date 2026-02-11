using Application.Abstractions;
using Application.Commands.Emails;
using Core.Entities;
using Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Portal.Models;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Application.Commands.Account
{
    public class ForgotPasswordCommand : IParammeterResultDbCommand<ForgetPasswordModel, AppUserEntity>
    {
        private readonly UserManager<AppUserEntity> _userManager;
        private readonly ResetPasswordEmailCommand _resetPasswordEmailCommand;
        public ForgotPasswordCommand(UserManager<AppUserEntity> userManager,
            ResetPasswordEmailCommand resetPasswordEmailCommand)
        {
            _userManager = userManager;
            _resetPasswordEmailCommand = resetPasswordEmailCommand;
        }

        public async Task<AppUserEntity> ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, ForgetPasswordModel parameter)
        {
            var user = await _userManager.FindByEmailAsync(parameter.Email);

            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                token = HttpUtility.UrlEncode(token);

                await _resetPasswordEmailCommand.ExecuteAsync(cancellationToken, dbContext, true, (parameter.Email, token));
            }

            return user;
        }
    }
}

