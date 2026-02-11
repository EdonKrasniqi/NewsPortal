using Application.Abstractions;
using Application.Commands.Files;
using Application.Helpers;
using Application.Projections;
using Core.Models;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Configs
{
    public class GetSiteConfig : IResultDbCommand<SiteConfigurationModel>
    {
        private readonly GetFileDataCommand _file;
        public GetSiteConfig(GetFileDataCommand file)
        {
            _file = file;
        }
        public async Task<SiteConfigurationModel> ExecuteAsync(CancellationToken token, AppDbContext context)
        {
            var model = await context.SiteConfigs
                .Select(x => x.MapModelFromEntity())
                .FirstOrDefaultAsync(token);

            model.HeaderLogo = await GetImage(model?.HeaderLogoId, context, token);
            model.FooterLogo = await GetImage(model?.FooterLogoId, context, token);

            return model;
        }
        private async Task<string> GetImage(Guid? id, AppDbContext context, CancellationToken cancellationToken)
        {
            if (id.HasValue)
            {
                var data = await _file.ExecuteAsync(cancellationToken, context, false, id! ?? Guid.Empty);
                var img = FileHelper.GetBase64String(data);
                return img;
            }
            return "";
        }
    }
}
