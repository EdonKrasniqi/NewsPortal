using Application.Commands.Files;
using Core.Models;
using DocumentFormat.OpenXml.InkML;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public static class NewsHelper
    {
        public async static Task LoadImages( this List<NewsModel> news,AppDbContext dbContext,GetFileDataCommand fileDataCommand,CancellationToken cancellationToken)
        {
            foreach(var item in news)
            {
                var data = await fileDataCommand.ExecuteAsync(cancellationToken, dbContext, false, item.ImageId);
                var img = FileHelper.GetBase64String(data);
                item.Image = img;
            }
        }
    }
}
