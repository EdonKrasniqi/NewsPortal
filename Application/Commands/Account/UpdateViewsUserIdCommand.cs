using Application.Abstractions;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Account
{
    public class UpdateViewsUserIdCommand : IParameterDbCommand<(string fingerPrintId, Guid userId)>
    {

        public async Task ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, (string fingerPrintId, Guid userId) parameter)
        {
            var news = await dbContext.Views
                .Where(x => x.FingerPrintId == parameter.fingerPrintId && x.UserId == null)
                .ToListAsync(cancellationToken);

            news.ForEach(x => x.UserId = parameter.userId);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
