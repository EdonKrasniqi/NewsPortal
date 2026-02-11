using Application.Abstractions;
using Application.Projections;
using Core.Models;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.UserNewsInteraction
{
    public class GetReactionForNewsQuery : IParammeterResultDbCommand<Guid, List<ReactionModel>>
    {
        public GetReactionForNewsQuery() {}
        public async Task<List<ReactionModel>> ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, Guid parameter)
        {
            var reactions = await dbContext.Reactions
                .Where(x => x.NewsId == parameter)
                .Select(x=> x.MapModelFromEntity())
                .ToListAsync(cancellationToken);

            return reactions;
        }
    }
}
