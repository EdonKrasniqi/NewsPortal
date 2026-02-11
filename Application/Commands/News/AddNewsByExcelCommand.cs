using Application.Abstractions;
using ClosedXML.Excel;
using Core.Models;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.News
{
    public class AddNewsByExcelCommand : IParammeterResultDbCommand<IFormFile, List<NewsModel>>
    {
        public async Task<List<NewsModel>> ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, IFormFile parameter)
        {
            var categories = new List<ListItemModel>();
            var newsList = new List<NewsEntity>();

            using (var stream = parameter.OpenReadStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    using (var workbook = new XLWorkbook(stream))
                    {
                        var worksheet = workbook.Worksheet(1);
                        var rows = worksheet.RowsUsed();
                        var categoryNames = rows.Skip(1).Select(x => x.Cell(7).Value.ToString()).ToList();

                        categories = await dbContext.Categories
                            .Where(x => categoryNames.Contains(x.Name))
                            .Select(x => new ListItemModel(x.Id,x.Name))
                            .ToListAsync(cancellationToken);

                        foreach (var row in rows.Skip(1))
                        {

                            var news = new NewsEntity();

                            news.Title = row.Cell(1).Value.ToString();
                            news.SubTitle = row.Cell(2).Value.ToString();
                            news.Content = row.Cell(5).Value.ToString();
                            news.Tags = row.Cell(6).Value.ToString();
                            news.CategoryId = categories
                                .Where(x => x.Name == row.Cell(7).Value.ToString())
                                .Select(x => x.Id)
                                .FirstOrDefault();
                            newsList.Add(news);
                        }
                    }
                }
            }

            dbContext.News.AddRange(newsList);
            await dbContext.SaveChangesAsync(cancellationToken);

            return null;
        }
    }
}

