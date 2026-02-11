using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUserEntity>
    {
        public void Configure(EntityTypeBuilder<AppUserEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.UserRoles)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Avatar)
                .WithOne()
                .HasForeignKey<AppUserEntity>(x => x.AvatarId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
