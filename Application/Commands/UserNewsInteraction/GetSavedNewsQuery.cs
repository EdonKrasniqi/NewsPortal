using Application.Abstractions;
using AutoMapper;
using Core.Enums;
using Core.Models;
using Domain.Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Core.Interfaces;
using Application.Projections;
using Application.Helpers;
using Application.Commands.Files;

namespace Application.Commands.UserNewsInteraction
{
    public class GetSavedNewsQuery : IParammeterResultDbCommand<(BaseFilter filter, Guid? newsId, Guid userId), List<NewsModel>>
    {
        private readonly IAuthorizationInterface _authManager;
        private readonly GetFileDataCommand _file;

        public GetSavedNewsQuery(IAuthorizationInterface authManager, GetFileDataCommand file)
        {
            _authManager = authManager;
            _file = file;
        }
        public async Task<List<NewsModel>> ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, (BaseFilter filter, Guid? newsId, Guid userId) parameter)
        {
            var news = new List<NewsModel>();

            if (parameter.filter == BaseFilter.All)
            {
                news = await dbContext.Saved
                    .Select(x=> x.News.MapNewsToModel(_authManager.GetCurrentUserId() ?? Guid.NewGuid()))
                    .ToListAsync(cancellationToken);
            }
            else if (parameter.filter == BaseFilter.Users)
            {
                news = await dbContext.Saved
                    .Where(x => x.UserId == parameter.userId)
                    .Select(x => x.News.MapNewsToModel(_authManager.GetCurrentUserId() ?? Guid.NewGuid()))
                    .ToListAsync(cancellationToken);
            }
            else
            {
                news = await dbContext.Saved
                    .Where(x => x.NewsId == parameter.newsId)
                    .Select(x => x.News.MapNewsToModel(_authManager.GetCurrentUserId() ?? Guid.NewGuid()))
                    .ToListAsync(cancellationToken);
            }

            await news.LoadImages(dbContext, _file, cancellationToken);

            return news;
        }
    }
}
