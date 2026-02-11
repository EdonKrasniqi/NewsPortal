using Application.Abstractions;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.News
{
    public class DeleteNewsCommand : IParameterDbCommand<Guid>
    {
        public async Task ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, Guid parameter)
        {
            var news = await dbContext.News
                .Where(x => x.Id == parameter)
                .FirstOrDefaultAsync(cancellationToken);

            dbContext.News.Remove(news);

            if (saveChanges)
            {
                await dbContext.SaveChangesAsync();
            }

        }
    }
}
