using Application.Abstractions;
using AutoMapper;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Domain.Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.UserNewsInteraction
{
    public class AddSavedNewsCommand : IParameterDbCommand<SavedNewsModel>
    {
        private readonly IAuthorizationInterface _authorizationInterface;

        public AddSavedNewsCommand(IAuthorizationInterface authorizationInterface)
        {
            _authorizationInterface = authorizationInterface;
        }
        public async Task ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, SavedNewsModel parameter)
        {
            var savedNews = new SavedNewsEntity();
            savedNews.NewsId = parameter.NewsId;

            savedNews.UserId = _authorizationInterface.GetCurrentUserId() ?? throw new AppBadDataException();
            if (savedNews == null)
            {
                throw new AppBadDataException();
            }
            

             dbContext.Saved.Add(savedNews);

            if (saveChanges)
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }

        }
    }
}
