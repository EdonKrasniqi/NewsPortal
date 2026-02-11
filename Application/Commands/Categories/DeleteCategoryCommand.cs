using Application.Abstractions;
using Core.Exceptions;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Categories
{
    public class DeleteCategoryCommand : IParameterDbCommand<Guid>
    {
        public async Task ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, Guid parameter)
        {
            var categ = await dbContext.Categories
                .Where(x => x.Id == parameter)
                .FirstOrDefaultAsync(cancellationToken);

            if(categ == null)
            {
                throw new AppBadDataException();
            }

            dbContext.Categories.Remove(categ);

            if (saveChanges)
            {
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
