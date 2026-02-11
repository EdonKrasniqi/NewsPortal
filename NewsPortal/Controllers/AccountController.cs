using Application.Commands.Account;
using Autofac;
using Core.Constants;
using Core.Entities;
using Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Controllers.Base;
using Portal.Models;
using Portal.ViewModels;
using System.Threading;
using System.Threading.Tasks;

namespace Portal.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(ILifetimeScope scope) : base(scope) { }

        [HttpPost]
        public async Task<ActionResult<UserModel>> RegisterAsync(CancellationToken cancellationToken, RegisterModel model)
        {
            model.Role = Roles.SimpleUser;

            var result = await scope.Resolve<RegisterCommad>()
                .ExecuteAsync(cancellationToken, scope.Resolve<AppDbContext>(), true, model);

            return result;
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<AuthenticationModel>> LoginAsync(CancellationToken cancellationToken, LoginModel model)
        {
            var result = await scope.Resolve<LoginCommad>()
                .ExecuteAsync(cancellationToken, scope.Resolve<AppDbContext>(),false, model);

            await scope.Resolve<UpdateViewsUserIdCommand>()
                .ExecuteAsync(cancellationToken, scope.Resolve<AppDbContext>(), true, (model.FingerPrintId, result.Id));

            return result;
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        public async Task<ActionResult<UserModel>> AddAdminAsync(CancellationToken cancellationToken, RegisterModel model)
        {
            model.Role = Roles.Admin;

            var result = await scope.Resolve<RegisterCommad>()
                .ExecuteAsync(cancellationToken, scope.Resolve<AppDbContext>(), true, model);
            return result;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<AppUserEntity>> ForgotPasswordAsync(CancellationToken cancellationToken, ForgetPasswordModel forgetPasswordViewModel)
        {
            var result = await scope.Resolve<ForgotPasswordCommand>()
                .ExecuteAsync(cancellationToken, scope.Resolve<AppDbContext>(), true, forgetPasswordViewModel);

            return Ok(result);
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> ResetPasswordAsync(CancellationToken cancellationToken, ResetPasswordModel resetPasswordViewModel)
        {
            await scope.Resolve<ResetPasswordCommand>()
                .ExecuteAsync(cancellationToken, scope.Resolve<AppDbContext>(), true, resetPasswordViewModel);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
        {
            var result = await scope.Resolve<GetCurrentUser>()
                  .ExecuteAsync(cancellationToken, scope.Resolve<AppDbContext>());

            return Ok(result);
        }
    }
}
