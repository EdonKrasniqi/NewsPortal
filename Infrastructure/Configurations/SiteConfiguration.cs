using Domain.Entities;
using Infrastructure.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class SiteConfiguration : BaseConfiguration<SiteConfigsEntity>
    {
        public override void Configure(EntityTypeBuilder<SiteConfigsEntity> builder)
        {
            base.Configure(builder);

            builder.HasOne(x => x.HeaderLogo)
                .WithOne()
                .HasForeignKey<SiteConfigsEntity>(x => x.HeaderLogoId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.FooterLogo)
                .WithOne()
                .HasForeignKey<SiteConfigsEntity>(x => x.FooterLogoId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
