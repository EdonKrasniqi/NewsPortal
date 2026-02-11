using Application.Abstractions;
using Infrastructure.Database;
using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces;
using Portal.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Application.Commands.Account
{
    public class GetCurrentUser : IResultDbCommand<AuthenticationModel>
    {
        private readonly GenerateTokenCommand _generateTokenCommand;
        private readonly IAuthorizationInterface user;

        public GetCurrentUser(GenerateTokenCommand generateTokenCommand,
            IAuthorizationInterface user)
        {
            this.user = user;
            _generateTokenCommand = generateTokenCommand;
        }
        public async Task<AuthenticationModel> ExecuteAsync(CancellationToken cancellationToken, AppDbContext context)
        {
            var id = this.user.GetCurrentUserId();

            if (id == null || id == Guid.Empty)
            {
                return null;
            }
            var user = await context.Users
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                return null;
            }

            var refreshtoken = await _generateTokenCommand.ExecuteAsync(cancellationToken, context, false,user);

            return refreshtoken;
        }

    }
}
