using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IParameterDbCommand<TParameter> : ICommand
    {
        Task ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, TParameter parameter);
    }
}
