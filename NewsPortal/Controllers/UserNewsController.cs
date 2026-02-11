using Application.Commands.UserNewsInteraction;
using Autofac;
using Core.Constants;
using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Portal.Controllers
{
    public class UserNewsController : BaseController
    {
        public UserNewsController(ILifetimeScope scope) : base(scope) { }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> AddViewAsync(CancellationToken canculationToken, ViewModel model)
        {
            await scope.Resolve<AddViewCommand>()
                .ExecuteAsync(canculationToken, scope.Resolve<AppDbContext>(), true, model);

            return Ok();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> AddSavedNewsAsync(CancellationToken cancellationToken, SavedNewsModel sn)
        {
            await scope.Resolve<AddSavedNewsCommand>()
                .ExecuteAsync(cancellationToken, scope.Resolve<AppDbContext>(), true, sn);
            return Ok();
        }

        [HttpGet]
        [Route("[action]/{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<List<ViewModel>>> GetViewForNewsAsync(CancellationToken cancellationToken, Guid id)
        {
            var result = await scope.Resolve<GetViewQuery>()
                  .ExecuteAsync(cancellationToken, scope.Resolve<AppDbContext>(), true, (BaseFilter.News, id, null));

            return Ok(result);
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<List<ViewModel>>> GetViewsAsync(CancellationToken cancellationToken)
        {
            var result = await scope.Resolve<GetViewQuery>()
                 .ExecuteAsync(cancellationToken, scope.Resolve<AppDbContext>(), true, (BaseFilter.All, Guid.Empty, null));

            return Ok(result);
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize]
        public async Task<ActionResult<List<NewsModel>>> GetSavedNewsAsync(CancellationToken cancellationToken)
        {
            var result = await scope.Resolve<GetSavedNewsQuery>()
                 .ExecuteAsync(cancellationToken, scope.Resolve<AppDbContext>(), true,
                 (BaseFilter.Users, null!, scope.Resolve<IAuthorizationInterface>().GetCurrentUserId() ?? Guid.Empty));

            return Ok(result);
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        public async Task<ActionResult> DeleteSavedNewsAsync(CancellationToken token, SavedNewsModel model)
        {
            await scope.Resolve<DeleteSavedNews>()
                .ExecuteAsync(token, scope.Resolve<AppDbContext>(), true, model);

            return Ok();
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        public async Task<ActionResult> AddReactionAsync(CancellationToken token, ReactionModel model)
        {
            await scope.Resolve<AddReaction>()
                .ExecuteAsync(token, scope.Resolve<AppDbContext>(), true, model);

            return Accepted();
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult<List<ReactionModel>>> GetNewsReactionsAsync(CancellationToken token, Guid id)
        {
            var result = await scope.Resolve<GetReactionForNewsQuery>()
                     .ExecuteAsync(token, scope.Resolve<AppDbContext>(), true, id);

            return Ok(result);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<ReactionModel>>> GetReactionsAsync(CancellationToken token)
        {
            var result = await scope.Resolve<GetReactionsQuery>()
                 .ExecuteAsync(token, scope.Resolve<AppDbContext>(), true, null!);

            return Ok(result);
        }
    }
}
