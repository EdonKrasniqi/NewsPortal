using Application.Abstractions;
using Core.Constants;
using Core.Entities;
using Core.Exceptions;
using Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Portal.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Account
{
    public class LoginCommad : IParammeterResultDbCommand<LoginModel, AuthenticationModel>
    {
        private readonly UserManager<AppUserEntity> _userMenager;
        private readonly GenerateTokenCommand _generateTokenCommand;
        private readonly SignInManager<AppUserEntity> _signInManager;
        public LoginCommad(UserManager<AppUserEntity> userMenager,
            GenerateTokenCommand generateTokenCommand,
            SignInManager<AppUserEntity> signInManager)
        {
            _userMenager = userMenager;
            _signInManager = signInManager;
            _generateTokenCommand = generateTokenCommand;
        }

        public async Task<AuthenticationModel> ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, LoginModel parameter)
        {
            var user = await _userMenager.FindByEmailAsync(parameter.Email);

            if (user == null)
            {
                throw new AppBadDataException();
            }

            var resault = await _signInManager.CheckPasswordSignInAsync(user, parameter.Password, false);

            if (resault.Succeeded)
            {
                var token = await _generateTokenCommand.ExecuteAsync(cancellationToken, dbContext, false,user);

                return token;
            }
            else
            {
                throw new Exception(ErrorMessages.BadPassword);
            }


        }
    }
}