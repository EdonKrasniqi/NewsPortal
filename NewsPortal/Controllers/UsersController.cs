using Application.Commands.Files;
using Application.Commands.Users;
using Application.Queries.Users;
using Autofac;
using Core.Constants;
using Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Portal.Controllers.Base;
using Portal.Models;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Portal.Controllers
{
    public class UsersController : BaseController
    {
        public UsersController(ILifetimeScope scope) : base(scope) { }

        [HttpGet]
        [Route("[action]")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<List<UserModel>>> GetUsersAsync(CancellationToken cancullationToken)
        {
            var result = await scope.Resolve<GetUsersQuery>()
                .ExecuteAsync(cancullationToken, scope.Resolve<AppDbContext>(), true, null);

            return Ok(result);
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize]
        public async Task<ActionResult<UserModel>> GetCurrentUserAsync(CancellationToken cancullationToken)
        {
            var result = await scope.Resolve<GetUsersQuery>()
               .ExecuteAsync(cancullationToken, scope.Resolve<AppDbContext>(), true, true);

            return Ok(result);
        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult> DeleteUserAsync(CancellationToken cancellationToken, Guid Id)
        {
            await scope.Resolve<DeleteUserCommand>()
                .ExecuteAsync(cancellationToken, scope.Resolve<AppDbContext>(), true, Id);

            return Ok();
        }
        
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> UpdateUserAsync(CancellationToken cancellationToken, [FromForm] IFormFile file, [FromForm] EditUserModel model)
        {
            var result = await scope.Resolve<UpdateUserCommand>()
            .ExecuteAsync(cancellationToken, scope.Resolve<AppDbContext>(), true, (model, file));

            return Ok(result);
        }

    }
}