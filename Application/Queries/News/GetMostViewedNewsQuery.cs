using Application.Abstractions;
using Application.Projections;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.News
{
    public class GetMostViewedNewsQuery : IParammeterResultDbCommand<int, List<NewsModel>>
    {
        public readonly IAuthorizationInterface _authorizationInterface;
        public GetMostViewedNewsQuery(IAuthorizationInterface authorizationInterface)
        {
            _authorizationInterface = authorizationInterface;

        }
        public async Task<List<NewsModel>> ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, int parameter)
        {
            var news = await dbContext.News
                 .OrderBy(x => x.Views.Count())
                 .Take(parameter)
                 .Select(x=> x.MapNewsToModel(_authorizationInterface.GetCurrentUserId() ?? Guid.Empty))
                 .ToListAsync(cancellationToken);

            return news;
        }
    }
}
