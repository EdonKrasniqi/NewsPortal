using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class RolesConfiguration : IEntityTypeConfiguration<RolesEntity>
    {
        public void Configure(EntityTypeBuilder<RolesEntity> builder)
        {
            builder.HasKey(x=> x.Id);
        }
    }
}
