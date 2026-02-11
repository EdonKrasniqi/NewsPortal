using Core.Entities.Base;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Base
{
    public  class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class, IBaseEntity, new()
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
           builder.HasKey(x => x.Id);
        }
    }
}
