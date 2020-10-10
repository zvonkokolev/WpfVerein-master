using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WpfVerein.Core.Entities;

namespace WpfVerein.Core.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IPersonRepository PersonRepository { get; }

        Task<int> SaveChangesAsync();
        Task DeleteDatabaseAsync();
        Task MigrateDatabaseAsync();
    }
}
