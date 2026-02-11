using Core.Entities;
using Domain.Entities;
using Infrastructure.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class NewsConfiguration : BaseConfiguration<NewsEntity>
    {
        public override void Configure(EntityTypeBuilder<NewsEntity> builder)
        {
           base.Configure(builder);

            builder.HasOne(x => x.CreatedBy)
                .WithMany()
                .HasForeignKey(x => x.CreatedById);

            builder.HasOne(x => x.UpdatedBy)
                .WithMany()
                .HasForeignKey(x => x.UpdatedById);

            builder.HasOne(x => x.Category)
                .WithMany(x => x.News)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Image)
               .WithMany()
               .HasForeignKey(x => x.ImageId)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
