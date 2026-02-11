using Application.Commands.News;
using Application.Queries.News;
using Autofac;
using Core.Constants;
using Core.Models;
using Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Portal.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Portal.Controllers
{
    public class NewsController : BaseController
    {
        public NewsController(ILifetimeScope scope) : base(scope) { }

        [HttpGet]
        public async Task<ActionResult<List<NewsModel>>> GetAllNewsAsync(CancellationToken token)
        {
            var result = await scope.Resolve<GetAllNewsQuery>()
                .ExecuteAsync(token, scope.Resolve<AppDbContext>());

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NewsModel>> GetNewsByIdAsync(CancellationToken cancellationToken, Guid id)
        {
            var result = await scope.Resolve<GetNewsByIdQuery>()
                .ExecuteAsync(cancellationToken, scope.Resolve<AppDbContext>(), true, id); ;

            return Ok(result);
        }


        [HttpPost]
        [Route("[action]")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult> AddOrEditAsync(CancellationToken cancellationToken, [FromForm] IFormFile file, [FromForm] NewsModel news)
        {
            var result = await scope.Resolve<AddOrEditNewsCommand>()
                   .ExecuteAsync(cancellationToken, scope.Resolve<AppDbContext>(), true, (news, file));

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult> DeleteNewsAsync(CancellationToken cancellationToken, Guid id)
        {
            await scope.Resolve<DeleteNewsCommand>()
                  .ExecuteAsync(cancellationToken, scope.Resolve<AppDbContext>(), true, id);

            return Accepted();
        }
        [HttpGet]
        [Route("[action]/{tag}")]
        public async Task<ActionResult<List<NewsModel>>> GetNewsByTagAsync(CancellationToken cancellationToken, string tag)
        {
            var resault = await scope.Resolve<GetNewsByTagQuery>()
                .ExecuteAsync(cancellationToken, scope.Resolve<AppDbContext>(), true, tag);

            return Ok(resault);
        }
        [HttpGet]
        [Route("[action]/{take}")]
        public async Task<ActionResult<List<NewsModel>>> GetMostViewedAsync(CancellationToken cancellationToken, int take)
        {
            var result = await scope.Resolve<GetMostViewedNewsQuery>()
                .ExecuteAsync(cancellationToken, scope.Resolve<AppDbContext>(), true, take);
            return Ok(result);

        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult> GetExcelAsync(CancellationToken cancellationToken)
        {
            var result = await scope.Resolve<GenerateExcelCommand>()
                .ExecuteAsync(cancellationToken, scope.Resolve<AppDbContext>());

            return File(result.ToArray(), MimeType.Excel, $"News{FileExtensions.Excel}");

        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> UpdateAddNewsAsync(CancellationToken cancellationToken, [FromForm] IFormFile file)
        {
            var result = await scope.Resolve<AddNewsByExcelCommand>()
            .ExecuteAsync(cancellationToken, scope.Resolve<AppDbContext>(), true, file);

            return Ok(result);
        }

    }

}

