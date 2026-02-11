using Application.Abstractions;
using Core.Entities;
using Core.Exceptions;
using Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Users
{
    public class DeleteUserCommand : IParameterDbCommand<Guid>
    {
        public UserManager<AppUserEntity> _userMenager;
        public DeleteUserCommand(UserManager<AppUserEntity> userMenager)
        {
            _userMenager = userMenager;

        }
        public async Task ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, Guid parameter)
        {
            var user = await dbContext.Users.Where(x => x.Id == parameter).FirstOrDefaultAsync();

            if(user == null)
            {
                throw new AppBadDataException();
            }

            await _userMenager.DeleteAsync(user);

            if (saveChanges)
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }

        }
    }
}
