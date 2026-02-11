using Infrastructure.Database;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IParammeterResultDbCommand<TParameter, TResault> : ICommand
    {
        Task<TResault> ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, TParameter parameter);

    }
}
