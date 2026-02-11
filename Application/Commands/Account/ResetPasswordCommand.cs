using Application.Abstractions;
using Core.Entities;
using Core.Exceptions;
using Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Portal.Models;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Account
{
    public class ResetPasswordCommand : IParameterDbCommand<ResetPasswordModel>
    {
        private UserManager<AppUserEntity> _userManager;

        public ResetPasswordCommand(UserManager<AppUserEntity> userManager)
        {
            _userManager = userManager;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, ResetPasswordModel parameter)
        {
            var user = await _userManager.FindByEmailAsync(parameter.Email);
            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, parameter.Token, parameter.Password);

                if (result.Succeeded)
                {
                    return;
                }

            }

            throw new AppBadDataException();
        }
    }
}
