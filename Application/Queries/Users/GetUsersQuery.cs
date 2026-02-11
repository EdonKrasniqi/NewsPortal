using Application.Abstractions;
using Application.Projections;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Portal.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.Users
{
    public class GetUsersQuery : IParammeterResultDbCommand<bool?, List<UserModel>>
    {
        private readonly UserManager<AppUserEntity> _userManager;
        private readonly IAuthorizationInterface authorizationInterface;

        public GetUsersQuery(UserManager<AppUserEntity> userManager, IAuthorizationInterface auth)
        {
            _userManager = userManager;
            authorizationInterface = auth;
        }

        public async Task<List<UserModel>> ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, bool? parameter)
        {
            var userModel = new List<UserModel>();
            if (parameter != true)
            {
                userModel = await dbContext.Users
                    .Select(x=> x.MapUserModelFromEntity())
                    .ToListAsync(cancellationToken);
            }
            else
            {
                var currentUserId = authorizationInterface.GetCurrentUserId();

                 userModel = await dbContext.Users
                    .Where(x => x.Id == currentUserId)
                    .Select(x=> x.MapUserModelFromEntity())
                    .ToListAsync(cancellationToken);
            }

            return userModel;
        }
    }
}
