using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAuthorizationInterface
    {
        public Guid? GetCurrentUserId();
        public bool isAdmin();
        public Task InitializeAsync(CancellationToken cancellationToken);

    }
}
