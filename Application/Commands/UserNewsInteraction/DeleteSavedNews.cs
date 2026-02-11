using Application.Abstractions;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.UserNewsInteraction
{
    public class DeleteSavedNews : IParameterDbCommand<SavedNewsModel>
    {
        private readonly IAuthorizationInterface _user;

        public DeleteSavedNews(IAuthorizationInterface user)
        {
            _user = user;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, SavedNewsModel parameter)
        {
            parameter.UserId = _user.GetCurrentUserId() ?? Guid.Empty;
            var savedNews = await dbContext.Saved.Where(x => x.UserId == parameter.UserId && x.NewsId == parameter.NewsId).FirstOrDefaultAsync(cancellationToken);

            if(savedNews == null)
            {
                throw new AppBadDataException();
            }

            dbContext.Saved.Remove(savedNews);

            if (saveChanges)
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
