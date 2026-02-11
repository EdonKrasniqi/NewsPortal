using Core.Entities;
using Infrastructure.Configurations.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class HomePageConfiguration : BaseConfiguration<HomePageConfigEntity>
    {
        public override void Configure(EntityTypeBuilder<HomePageConfigEntity> builder)
        {
            base.Configure(builder);

        }
    }
}
