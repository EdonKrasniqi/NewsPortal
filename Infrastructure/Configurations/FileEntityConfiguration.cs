using Core.Entities;
using Core.Entities.Base;
using Infrastructure.Configurations.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class FileEntityConfiguration : BaseConfiguration<FileEntity>
    {
        public override void Configure(EntityTypeBuilder<FileEntity> builder)
        {
            base.Configure(builder);
        }
    }
}
