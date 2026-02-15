using Application.Abstractions;
using Application.Commands.Files;
using Application.Helpers;
using Application.Projections;
using AutoMapper;
using Core.Interfaces;
using Core.Models;
using DocumentFormat.OpenXml.InkML;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.News
{
    public class GetAllNewsQuery : IResultDbCommand<List<NewsModel>>
    {
        private readonly IAuthorizationInterface _authorizationInterface;
        private readonly GetFileDataCommand _file;

        public GetAllNewsQuery(IAuthorizationInterface authorizationInterface, GetFileDataCommand file)
        {
            _authorizationInterface = authorizationInterface;
            _file = file;
        }
        public async Task<List<NewsModel>> ExecuteAsync(CancellationToken token, AppDbContext context)
        {

                var news = await context.News
                .Select(x => new NewsModel
                {
                    CategoryId = x.CategoryId,
                    Id = x.Id,
                    SubTitle = x.SubTitle,
                    Title = x.Title,
                    isSaved = _authorizationInterface.GetCurrentUserId().HasValue ? x.SavedNews.Where(x => x.UserId == _authorizationInterface.GetCurrentUserId()).Any() : false,
                    IsFeatured = x.IsFeatured,
                    Content = x.Content,
                    CreatedById = x.CreatedById ?? null,
                    CreatedOnDate = x.CreatedOnDate,
                    IsDeleted = x.IsDeleted,
                    Tags = x.Tags,
                    UpdatedById = x.UpdatedById ?? null,
                    UpdatedOnDate = x.UpdatedOnDate,
                    ImageId = x.ImageId.Value,
                    Video = x.Video
                })
                .ToListAsync(token);

                await news.LoadImages(context, _file, token);

                return news;
        }
    }
}
