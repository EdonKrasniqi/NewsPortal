
using Application.Commands.Configs;
using Autofac;
using Core.Models;
using Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Portal.Controllers.Base;
using System.Threading;
using System.Threading.Tasks;

namespace Portal.Controllers
{
    public class ConfigController : BaseController
    {
        public ConfigController(ILifetimeScope scope) : base(scope) { }

        [HttpGet]
        public async Task<ActionResult<SiteConfigurationModel>> GetNewsConfig(CancellationToken cancullationToken)
        {
            var result = await scope.Resolve<GetSiteConfig>()
                .ExecuteAsync(cancullationToken, scope.Resolve<AppDbContext>());

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> EditNewsConfigRow(CancellationToken cancellationToken, SiteConfigurationModel siteConfig)
        {
            await scope.Resolve<UpdateSiteConfigCommand>()
                  .ExecuteAsync(cancellationToken, scope.Resolve<AppDbContext>(), true, siteConfig);

            return Ok();
        }
    }
}
