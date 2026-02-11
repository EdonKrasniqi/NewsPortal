using Application.Abstractions;
using Application.Commands.Files;
using Application.Helpers;
using Application.Projections;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.News
{
    public class GetNewsByIdQuery : IParammeterResultDbCommand<Guid, NewsModel>
    {
        private readonly IAuthorizationInterface _authorizationInterface;
        private readonly GetFileDataCommand _file;
        public GetNewsByIdQuery(IAuthorizationInterface authorizationInterface,GetFileDataCommand filCommand)
        {
            _authorizationInterface = authorizationInterface;
            _file = filCommand;
        }

        public async Task<NewsModel> ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, Guid parameter)
        {
            var news = await dbContext.News
                .Where(x => x.Id == parameter)
                .Select(x=> x.MapNewsToModel(_authorizationInterface.GetCurrentUserId() ?? Guid.Empty))
            .FirstOrDefaultAsync(cancellationToken);

            if (news is null)
            {
                throw new AppBadDataException();
            }

            var data = await _file.ExecuteAsync(cancellationToken, dbContext, false, news.ImageId);
            var img = FileHelper.GetBase64String(data);
            news.Image = img;

            return news;
        }
    }
}
