using Application.Abstractions;
using Application.Helpers;
using Core.Entities;
using Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Files
{
    public class UploadFileCommand : IParammeterResultDbCommand<(Guid? id, IFormFile file), FileEntity>
    {
        private readonly IConfiguration _configuration;

        public UploadFileCommand(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<FileEntity> ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, (Guid? id, IFormFile file) parameter)
        {
            var fileEntity = new FileEntity();

            var saveFileModel = await parameter.file.ConvertToSaveModelAsync();
            if (parameter.id.HasValue && parameter.id != Guid.Empty)
            {
                fileEntity = await dbContext.Files.Where(x => x.Id == parameter.id).FirstOrDefaultAsync(cancellationToken);
            }
            dbContext.Files.Add(fileEntity);

            var basePath = _configuration["File:StoragePath"];
            string fileName = fileEntity.Id.ToString() + saveFileModel.FileExtension;
            string storagePath = Path.Combine(basePath, fileName);
            using (var stream = new FileStream(storagePath, FileMode.Create))
            {
                parameter.file.CopyTo(stream);
            }
            fileEntity.Name = saveFileModel.FileName;
            fileEntity.Extension = saveFileModel.FileExtension;

            if(saveChanges)
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            return fileEntity;
        }
    }
}
