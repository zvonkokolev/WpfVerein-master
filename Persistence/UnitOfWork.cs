using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Wpf.Persistence.Validations;
using WpfVerein.Core.Contracts;
using WpfVerein.Core.Entities;
using WpfVerein.Model;

namespace WpfVerein.Persistence
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ClubMemberContext _dbContext;
		private bool _disposed;
		private EmailDuplicateValidation _duplicateValidation;

		public UnitOfWork()
		{
			_dbContext = new ClubMemberContext();
			PersonRepository = new PersonRepository(_dbContext);
		}

		public IPersonRepository PersonRepository { get; }


		/// <summary>
		/// Repository-übergreifendes Speichern der Änderungen
		/// </summary>
		public async Task<int> SaveChangesAsync()
		{
			var entities = _dbContext.ChangeTracker.Entries()
				.Where(entity => entity.State == EntityState.Added
						|| entity.State == EntityState.Modified)
				.Select(e => e.Entity);
			foreach (var entity in entities)
			{
				ValidateEntity(entity);
			}
			return await _dbContext.SaveChangesAsync();
		}

		/// <summary>
		/// Validierungen auf DbContext-Ebene
		/// </summary>
		/// <param name="entity"></param>
		private void ValidateEntity(object entity)
		{
			// Validation of the stored validation attributes
			Validator.ValidateObject(entity, new ValidationContext(entity), true);

			ValidationResult result;
			switch (entity)
			{
				//case Appointment appointment:
				//	result = _overlapValidation.GetValidationResult(appointment, new ValidationContext(appointment));
				//	break;
				case Member user:
					result = _duplicateValidation.GetValidationResult(user, new ValidationContext(user));
					break;
				default:
					return;
			}

			if (result != null && result != ValidationResult.Success && entity is Member)
			{
				throw new ValidationException(result, _duplicateValidation, entity);
			}
			//if (result != null && result != ValidationResult.Success && entity is Appointment)
			//{
			//	throw new ValidationException(result, _overlapValidation, entity);
			//}
		}

		public int SaveChanges()
		{
			//var entities = _dbContext.ChangeTracker.Entries()
			//	.Where(entity => entity.State == EntityState.Added)
			//		|| entity.State == EntityState.Modified)
			//	.Select(e => e.Entity);
			//foreach (var entity in entities)
			//{
			//	ValidateEntity(entity);
			//}
			return _dbContext.SaveChanges();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_dbContext.Dispose();
				}
			}
			_disposed = true;
		}
		public async Task DeleteDatabaseAsync() => await _dbContext.Database.EnsureDeletedAsync();
		public async Task MigrateDatabaseAsync() => await _dbContext.Database.MigrateAsync();
	}
}
