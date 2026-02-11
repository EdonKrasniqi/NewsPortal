using Application.Abstractions;
using Application.Commands.Files;
using Application.Helpers;
using Application.Projections;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.News
{
    public class GetNewsByTagQuery : IParammeterResultDbCommand<string, List<NewsModel>>
    {
        private readonly IAuthorizationInterface _authorizationInterface;
        private readonly GetFileDataCommand _file;
        public GetNewsByTagQuery(IAuthorizationInterface authorizationInterface, GetFileDataCommand file)
        {
            _authorizationInterface = authorizationInterface;
            _file = file;
        }
        public async Task<List<NewsModel>> ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, string parameter)
        {
            var news = await dbContext.News
                .Where(x => x.Tags.Contains(parameter) && !x.IsDeleted)
                .Select(x=> x.MapNewsToModel(_authorizationInterface.GetCurrentUserId() ?? Guid.Empty))
            .ToListAsync(cancellationToken);

           await news.LoadImages(dbContext, _file, cancellationToken);

            return news;

        }
    }
}
