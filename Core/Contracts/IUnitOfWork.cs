using System;
using System.Threading.Tasks;

namespace WpfVerein.Core.Contracts
{
	public interface IUnitOfWork : IDisposable
	{
		IPersonRepository PersonRepository { get; }

		Task<int> SaveChangesAsync();
		Task DeleteDatabaseAsync();
		Task MigrateDatabaseAsync();

		int SaveChanges();
	}
}
