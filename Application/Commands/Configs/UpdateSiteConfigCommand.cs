using Application.Abstractions;
using Application.Projections;
using AutoMapper;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Domain.Entities;
using Infrastructure.Database;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Configs
{
    public class UpdateSiteConfigCommand : IParameterDbCommand<SiteConfigurationModel>
    {
        private readonly IAuthorizationInterface _authorization;
        public UpdateSiteConfigCommand(IAuthorizationInterface authorization)
        {
            _authorization = authorization;
        }
        public async Task ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, SiteConfigurationModel parameter)
        {
            var siteConfig = parameter.MapEntityFromModel();

            if (siteConfig == null)
            {
                throw new AppBadDataException();
            }

            siteConfig.UpdatedById = _authorization.GetCurrentUserId();
            siteConfig.LatUpdatedOn = DateTime.Now;

            dbContext.SiteConfigs.Update(siteConfig);

            if (saveChanges)
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
