using Application.Commands.Files;
using Bogus;
using Bogus.Extensions;
using Core.Constants;
using Core.Entities;
using CsvHelper;
using CsvHelper.Configuration;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TaskRunner.Tasks
{
    public class DbInitialization
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUserEntity> _userManager;
        private readonly UploadFileCommand _fileUpload;
        public DbInitialization(AppDbContext appDbContext, UserManager<AppUserEntity> userManager, UploadFileCommand fileUpload)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _fileUpload = fileUpload;
        }
        public async Task Run(CancellationToken cancellationToken)
        {
            await CreateAndMigrateDatabse(cancellationToken);
            
            var roles = SeedRoles();

            await SeedUsersAsync(cancellationToken, roles);

            var categories = SeedCategories(cancellationToken);

            var file = CreateFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Documents", "Images", "RTK_logo.png"), "image/png");

            var fileEntity = await _fileUpload.ExecuteAsync(cancellationToken, _appDbContext, false, (null, file));

            var user = await _appDbContext.Users.Where(x => x.Email == "admin@gmail.com").FirstOrDefaultAsync(cancellationToken);

            SeedNews(categories, fileEntity, user);

            await SeedConfigurationsAsync(cancellationToken);

            await _appDbContext.SaveChangesAsync(CancellationToken.None);
        }

        private void SeedNews(List<CategoryEntity> categoryEntities, FileEntity file, AppUserEntity user)
        {
            var faker = new Faker<NewsEntity>()
                .RuleFor(x => x.Category, f => f.PickRandom(categoryEntities))
                .RuleFor(x => x.IsFeatured, f => f.Random.Bool())
                .RuleFor(x => x.IsDeleted, false)
                .RuleFor(x => x.SubTitle, f => f.Lorem.Text().ClampLength(20))
                .RuleFor(x => x.Title, f => f.Lorem.Text().ClampLength(10))
                .RuleFor(x => x.Content, f => f.Lorem.Text().ClampLength(100))
                .RuleFor(x => x.ExpireDate, DateTime.Now.AddYears(1))
                .RuleFor(x => x.CreatedById, user.Id)
                .RuleFor(x => x.UpdatedById, user.Id)
                .RuleFor(x => x.CreatedOnDate, DateTime.UtcNow)
                .RuleFor(x => x.UpdatedOnDate, DateTime.UtcNow)
                .RuleFor(x => x.Tags, f => "Test,Keyword");

            Random random = new Random();

            int randomNumber = random.Next(2, 11);
            for (int i = 0; i < randomNumber; i++)
            {
                var news = faker.Generate();

                news.ImageId = file.Id;
                
                _appDbContext.Add(news);
            }

        }

        private async Task SeedConfigurationsAsync(CancellationToken cancellationToken)
        {
            SiteConfigsEntity config = new SiteConfigsEntity();
            var file = CreateFormFile(Path.Combine(Directory.GetCurrentDirectory(), "Documents", "Images", "RTK_logo.png"), "image/png");
            var fileEntity = await _fileUpload.ExecuteAsync(cancellationToken, _appDbContext, false, (null, file));
            config.HeaderLogoId = fileEntity.Id;
            config.FooterLogoId = fileEntity.Id;
            config.FooterColor = "#fff";
            config.HeaderColor = "#fff";

            _appDbContext.SiteConfigs.Add(config);
        }

        private async Task CreateAndMigrateDatabse(CancellationToken cancellationToken)
        {
            await _appDbContext.Database.EnsureDeletedAsync(cancellationToken);
            await _appDbContext.Database.MigrateAsync(cancellationToken);
            await _appDbContext.Database.EnsureCreatedAsync(cancellationToken);
        }

        private async Task<List<AppUserEntity>> SeedUsersAsync(CancellationToken cancellationToken,List<RolesEntity> roles)
        {
            var adminUser = new AppUserEntity()
            {
                Email = "admin@gmail.com",
                UserName = "Admin",
                EmailConfirmed = true,
                UserRoles = new List<UserRoleEntity>()
                {
                    new UserRoleEntity()
                    {
                        Role = roles.Where(x=> x.Name == Roles.Admin).FirstOrDefault()
                    }
                }
            };

            var simpleUser = new AppUserEntity()
            {
                Email = "edon.krasniqi@gmail.com",
                UserName = "EdonKrasniqi",
                EmailConfirmed = true,
                UserRoles = new List<UserRoleEntity>()
                {
                    new UserRoleEntity()
                    {
                        Role = roles.Where(x=> x.Name == Roles.SimpleUser).FirstOrDefault()
                    }
                }
            };

            await _userManager.CreateAsync(adminUser, "Pa$$w0rd");
            await _userManager.CreateAsync(simpleUser, "Pa$$w0rd");

            return new List<AppUserEntity> { adminUser,simpleUser };
        }

        private List<CategoryEntity> SeedCategories(CancellationToken cancellationToken)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(),"Documents","CSV","Seed.csv");

            List<CategoryModel> newsCategories = new List<CategoryModel>();

            // Read the CSV file
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HeaderValidated = null,
                MissingFieldFound = null
            };
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader,config,false))
            {
                newsCategories = csv.GetRecords<CategoryModel>().ToList();
            }
            var entities = new List<CategoryEntity>();
            // Output the read data
            foreach (var category in newsCategories)
            {
               var entity = new CategoryEntity()
               {
                   Name = category.Name,    
                   Description = category.Description,
                   ShowOnline = category.ShowOnline,
               };

                entities.Add(entity);
            }

            _appDbContext.Categories.AddRange(entities);

            return entities;
        }

        private List<RolesEntity> SeedRoles()
        {
            var roles = new List<RolesEntity>()
            {
              new RolesEntity()
              {
                  Name = Roles.SimpleUser,
                  NormalizedName = Roles.SimpleUser,
              },
               new RolesEntity()
              {
                  Name = Roles.Admin,
                  NormalizedName = Roles.Admin,
              }
            };

            _appDbContext.Roles.AddRange(roles);

            return roles;
        }

        public static IFormFile CreateFormFile(string filePath,string type)
        {
            var fileName = Path.GetFileName(filePath);
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var formFile = new FormFile(fileStream, 0, fileStream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = type
            };

            return formFile;
        }
    }
}
