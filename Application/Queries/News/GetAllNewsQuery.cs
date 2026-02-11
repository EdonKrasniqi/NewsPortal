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
                .Select(x=> x.MapNewsToModel(_authorizationInterface.GetCurrentUserId() ?? Guid.Empty))
                .ToListAsync(token);

            await news.LoadImages(context, _file, token);

            return news;
        }
    }
}
