using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IParameterResultCommand<TParameter, TResault> : ICommand
    {
        Task<TResault> ExecuteAsync(CancellationToken cancellationToken, TParameter parameter);
    }
}
