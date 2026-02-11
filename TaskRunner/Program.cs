using Application.Commands.Files;
using AutoMapper.Configuration;
using Core.Entities;
using Core.Models;
using Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Portal.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using TaskRunner.Tasks;

namespace TaskRunner
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
          .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            config.SetBasePath(Directory.GetCurrentDirectory()!)
                 .AddEnvironmentVariables()
                 .AddUserSecrets<Program>();

        }).ConfigureServices((context, services) =>
        {
            var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));
            services.AddScoped<UserInput>();
            services.AddScoped<UploadFileCommand>();
            services.AddScoped<DbInitialization>();

            services.AddIdentity<AppUserEntity, RolesEntity>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            var jwtSettings = context.Configuration.GetSection("JWT").Get<JwtModel>();
            services.Configure<JwtModel>(context.Configuration.GetSection("JWT"));

            services.AddIdentityCore<AppUserEntity>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequiredLength = 8;
                opt.Password.RequiredUniqueChars =0;
                opt.Password.RequireLowercase = false;
            })
            .AddSignInManager<SignInManager<AppUserEntity>>();

            services.Configure<MailSettings>(context.Configuration.GetSection(nameof(MailSettings)));
        })
        .Build();

            using var services = builder.Services.CreateScope();
            var userInput = services.ServiceProvider.GetRequiredService<UserInput>();
            await userInput.GetAsync();
        }
    }
}
