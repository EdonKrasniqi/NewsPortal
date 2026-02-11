using Core.Entities;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace Infrastructure.Database
{
    public class AppDbContext : IdentityDbContext<AppUserEntity, RolesEntity, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<NewsEntity> News { get; set; }
        public DbSet<SiteConfigsEntity> SiteConfigs { get; set; }
        public DbSet<ViewsEntity> Views { get; set; }
        public DbSet<SavedNewsEntity> Saved { get; set; }
        public DbSet<ReactionEntity> Reactions { get; set; }
        public DbSet<AppUserEntity> Users { get; set; }
        public DbSet<RolesEntity> Roles { get; set; }
        public DbSet<UserRoleEntity> UserRoles { get; set; }
        public DbSet<FileEntity> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
