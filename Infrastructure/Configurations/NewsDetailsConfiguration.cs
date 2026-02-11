using Core.Entities;
using Infrastructure.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class NewsDetailsConfiguration : BaseConfiguration<NewsDetailsConfigEntity>
    {
        public override void Configure(EntityTypeBuilder<NewsDetailsConfigEntity> builder)
        {
            base.Configure(builder);

            builder.HasOne(x => x.BackgroundImg)
                .WithOne()
                .HasForeignKey<NewsDetailsConfigEntity>(x => x.BackgroundImgId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
