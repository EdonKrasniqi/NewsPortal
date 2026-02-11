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
    public class GetReactionsQuery : IParammeterResultDbCommand<Guid?, List<ReactionModel>>
    {
        public async Task<List<ReactionModel>> ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, Guid? parameter)
        {
            var model = new List<ReactionModel>();

            model = await dbContext.News
                .Where(x=> parameter.HasValue ? x.Id == parameter : true)
                .Select(x => x.MapReactionFromNews())
                .ToListAsync(cancellationToken);

            return model;
        }
    }
}
