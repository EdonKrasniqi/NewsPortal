using Application.Commands.Reports;
using Autofac;
using Core.Enums;
using Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Portal.Controllers.Base;
using Portal.Models;
using Portal.ViewModels;
using System.Threading;
using System.Threading.Tasks;

namespace Portal.Controllers
{
    public class ReportController : BaseController
    {
        public ReportController(ILifetimeScope scope) : base(scope) { }

        [HttpGet("[action]/{filter}")]
        public async Task<ActionResult<DashboardViewModel>> GetDashboarViewModelAsync(CancellationToken token, DashbordMainFilter filter)
        {
            var result = await scope.Resolve<GetDashbordViewModelQuery>()
                .ExecuteAsync(token, scope.Resolve<AppDbContext>(), true, filter);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ReportModel>> GetReportAsync(CancellationToken token, ReportModel model)
        {
            var result = await scope.Resolve<GetReportCommand>()
                 .ExecuteAsync(token, scope.Resolve<AppDbContext>(), true, model);

            return Ok(result);
        }
    }
}
