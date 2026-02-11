using Application.Abstractions;
using ClosedXML.Excel;
using Core.Models;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.News
{
    public class GenerateExcelCommand : IResultDbCommand<MemoryStream>
    {
        public async Task<MemoryStream> ExecuteAsync(CancellationToken token, AppDbContext context)
        {
            var coulums = new string[7] {
                nameof(NewsEntity.Title),
                nameof(NewsEntity.SubTitle),
                nameof(NewsEntity.CreatedOnDate),
                nameof(NewsEntity.CreatedBy),
                nameof(NewsEntity.Content),
                nameof(NewsEntity.Tags),
                nameof(NewsEntity.Category)
            };

            var dc = coulums.Select(x=> new DataColumn(x)).ToArray();
           
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(dc);

            var news = await context.News
                .Where(x => x.IsDeleted == false)
                 .Select(x=> new
                 {
                     x.Title,
                     x.SubTitle,
                     x.CreatedOnDate,
                     x.CreatedBy.UserName,
                     x.Content,
                     x.Tags,
                     CategoryName = x.Category.Name
                 })
                .ToListAsync(token);

            foreach (var item in news)
            {
                dt.Rows.Add(item.Title,
                    item.SubTitle,
                    item.CreatedOnDate,
                    item.UserName,
                    item.Content,
                    item.Tags,
                    item.CategoryName);
            }

            using (XLWorkbook wb = new XLWorkbook()) 
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream()) 
                {
                    wb.SaveAs(stream);
                    return stream;
                }
            }
        }
    }
}
