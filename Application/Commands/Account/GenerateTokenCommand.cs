using Application.Abstractions;
using Core.Entities;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Infrastructure.Database;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Portal.Models;
using Microsoft.AspNetCore.Identity;
using Core.Models;

namespace Application.Commands.Account
{
    public class GenerateTokenCommand : IParammeterResultDbCommand<AppUserEntity, AuthenticationModel>
    {
        private readonly UserManager<AppUserEntity> _userMenager;
        private readonly JwtModel _jwt;
        public GenerateTokenCommand(UserManager<AppUserEntity> userManager,
           JwtModel jwt)
        {
            _userMenager = userManager;
            _jwt = jwt;
        }

        public async Task<AuthenticationModel> ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, AppUserEntity parameter)
        {

            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,parameter.UserName),
                new Claim(ClaimTypes.NameIdentifier,parameter.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,parameter.Email),
                new Claim(JwtRegisteredClaimNames.Sub,parameter.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,parameter.Id.ToString())
            };

            var userRoles = await _userMenager.GetRolesAsync(parameter);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwt.Secret));

            var token = new JwtSecurityToken(
                claims: authClaims,
               expires: DateTime.UtcNow.AddMinutes(1),
               signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
               );
            IdentityModelEventSource.ShowPII = true;
            var jwtTOken = new JwtSecurityTokenHandler().WriteToken(token);

            var response = new AuthenticationModel()
            {
                Id = parameter.Id,
                Token = jwtTOken,
                RefreshToken = null,
                ExpireDate = token.ValidTo,
                UserName = parameter.UserName,
                UserEmail = parameter.Email,
                UserRole = userRoles.FirstOrDefault()
            };

            return response;
        }
    }
}
