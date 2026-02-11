using Application.Abstractions;
using Application.Projections;
using AutoMapper;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Database;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.UserNewsInteraction
{
    public class AddReaction : IParameterDbCommand<ReactionModel>
    {
        private readonly IAuthorizationInterface _authorizationInterface;
        public AddReaction(IAuthorizationInterface authorizationInterface)
        {
            _authorizationInterface = authorizationInterface;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, ReactionModel parameter)
        {
            var entity = parameter.MapEntityFromModel();

            entity.UserId = _authorizationInterface.GetCurrentUserId() ?? Guid.NewGuid();

            dbContext.Reactions.Add(entity);

            if (saveChanges)
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
