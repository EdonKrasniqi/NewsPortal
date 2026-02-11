using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IResultDbCommand<TResault> : ICommand
    {
        Task<TResault> ExecuteAsync(CancellationToken token, AppDbContext context);
    }
}
