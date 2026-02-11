using Domain.Entities;
using Infrastructure.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ReactionConfiguration : BaseConfiguration<ReactionEntity>
    {
        public override void Configure(EntityTypeBuilder<ReactionEntity> builder)
        {
            base.Configure(builder);

            builder.HasOne(x => x.News)
                .WithMany()
                .HasForeignKey(x => x.NewsId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
