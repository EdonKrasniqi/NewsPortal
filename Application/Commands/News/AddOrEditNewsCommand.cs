using Application.Abstractions;
using Application.Commands.Files;
using Application.Projections;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.News
{
    public class AddOrEditNewsCommand : IParammeterResultDbCommand<(NewsModel model, IFormFile file),Guid>
    {
        private readonly IAuthorizationInterface _user;
        private readonly UploadFileCommand _file;
        public AddOrEditNewsCommand(IAuthorizationInterface user,UploadFileCommand file)
        {
            _user = user;
            _file = file;

        }
        public async Task<Guid> ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, (NewsModel model,IFormFile file) parameter)
        {
            if (parameter.model is null)
            {
                throw new AppBadDataException();
            }
            var news = parameter.model.MapNewsEntityFromModel();

            news.UpdatedById = _user.GetCurrentUserId();
            news.UpdatedOnDate = DateTime.Now;

            if(parameter.file != null)
            {
                news.Image = await _file.ExecuteAsync(cancellationToken, dbContext,false,(news.ImageId, parameter.file));
            }

            if (news.Id == Guid.Empty)
            {
                news.CreatedOnDate = DateTime.Now;
                news.CreatedById = _user.GetCurrentUserId();
                await dbContext.News.AddAsync(news, cancellationToken);
            }
            else
            {
                dbContext.News.Update(news);
            }

            if (saveChanges)
            {
                await dbContext.SaveChangesAsync(CancellationToken.None);
            }

            return news.Id;
        }
    }
}
