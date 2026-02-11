using Application.Abstractions;
using Core.Models;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Files
{
    public class GetFileDataCommand : IParammeterResultDbCommand<Guid, FileModel>
    {
        private readonly IConfiguration _configuration;
        public GetFileDataCommand(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<FileModel> ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, Guid parameter)
        {
            var model = new FileModel();

            var fileEntity = await dbContext.Files
                .Where(x => x.Id == parameter)
                .FirstOrDefaultAsync(cancellationToken);

            var basePath = _configuration["File:StoragePath"];
            string fileName = fileEntity.Id.ToString() + fileEntity.Extension;
            string storagePath = Path.Combine(basePath, fileName);
            byte[] fileBytes = await File.ReadAllBytesAsync(storagePath);

            model.Data = fileBytes;
            model.FileName = fileEntity.Name;
            model.Type = fileEntity.Extension;
            return model;
        }
    }
}
