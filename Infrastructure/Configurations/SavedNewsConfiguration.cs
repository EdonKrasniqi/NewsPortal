using Domain.Entities;
using Infrastructure.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class SavedNewsConfiguration : BaseConfiguration<SavedNewsEntity>
    {
        public override void Configure(EntityTypeBuilder<SavedNewsEntity> builder)
        {
            base.Configure(builder);

            builder.HasIndex(x => new { x.UserId, x.NewsId })
                .IsUnique();
            
        }
    }
}
