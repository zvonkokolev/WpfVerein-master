using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfVerein.Core.Contracts;
using WpfVerein.Core.Entities;
using WpfVerein.Persistence;
using WpfVerein.Utils;

namespace WpfVerein.Model
{
	/// <summary>
	/// Repository als Singleton, damit die Daten aus dem CSV-File nur einmal gelesen werden!
	/// </summary>
	public class PersonRepository : IPersonRepository
	{
		private const string fileName = "Verein.csv";
		private readonly ClubMemberContext _dbContext;
		private List<Member> dbMembers = new List<Member>();

		public PersonRepository(ClubMemberContext dbContext)
		{
			_dbContext = dbContext;
		}

		/// <summary>
		/// Lädt die Daten vom csv-File in die Collection
		/// </summary>
		public static void LoadCdsFromCsv()
		{
			List<Member> members = new List<Member>();
			string[][] cdCsv = fileName.ReadStringMatrixFromCsv(true);
			members = cdCsv.Select(x =>
				  new Member
				  {
					  Id = int.Parse(x.ElementAt(0)),
					  RowVersion = null,
					  Firstname = x.ElementAt(2),
					  Lastname = x.ElementAt(3),
					  Email = x.ElementAt(4),
					  Phone = x.ElementAt(5),
					  BirthDay = Convert.ToDateTime(x.ElementAt(6)),
					  ActualDateTime = Convert.ToDateTime(x.ElementAt(7))
				  }).ToList();
		}

		/// <summary>
		/// Löscht einen Mitglied
		/// </summary>
		/// <param name="member"></param>
		public void RemoveCd(Member member)
		{
			Member cdToDelete = _dbContext.Members
				 .Where(cd => cd.Firstname.Equals(member.Firstname) && cd.Lastname.Equals(member.Lastname))
				 .FirstOrDefault();
			_dbContext.Members.Remove(cdToDelete);
		}

		/// <summary>
		/// Bearbeitet einen Mitglied
		/// </summary>
		/// <param name="newMember"></param>
		public void UpdateCd(Member oldMemberChanged)
		{
			_dbContext.Update(oldMemberChanged);
		}

		/// <summary>
		/// Neuer Mitglied in der Verein einfügen
		/// </summary>
		/// <param name="member"></param>
		public void AddMember(Member member)
		{
			dbMembers.Add(member);
		}

		public async Task AddMemberAsync(Member person) => await _dbContext.Members.AddAsync(person);

		/// <summary>
		/// Liefert eine (neue!) Liste aller Mitglieder
		/// Entkoppelt die zurückgelieferte Collektion von der Collection im Repository
		/// Beachte! Die Objekte selbst sind jedoch noch dieselben (clonen wäre erforderlich)!
		/// </summary>
		/// <returns></returns>
		public List<Member> GetAllMembers() => _dbContext.Members.OrderBy(x => x.Firstname).ToList();

		public async Task<IEnumerable<Member>> GetAllMembersAsync() => await _dbContext.Members.OrderBy(x => x.Firstname).ToListAsync();

		public async Task<Member> GetMemberByPhoneNumberAsync(string phoneNumber) =>
			 await _dbContext.Members
				  .Where(cd => cd.Phone.Equals(phoneNumber))
				  .FirstOrDefaultAsync();

		public async Task<Member> GetMemberByFirstAndLastnameAsync(string firstname, string lastname) =>
			 await _dbContext.Members
				  .Where(cd => cd.Firstname.Equals(firstname) && cd.Lastname.Equals(lastname))
				  .FirstOrDefaultAsync();

		public async Task<IEnumerable<Member>> GetPersonsByDateAsync(DateTime date) =>
			 await _dbContext.Members
				  .Where(cd => cd.BirthDay == date)
				  .ToListAsync();

		public async Task<IEnumerable<Member>> GetPersonsForTodayAsync() =>
			 await _dbContext.Members
				  .Where(cd => cd.ActualDateTime.Date == DateTime.Now.Date)
				  .ToListAsync();

		public void AttachMembersRange(List<Member> members) => _dbContext.AttachRange(members);

		public async Task<bool> CheckIfUserEmailExists(string email, int userId)
		{
			var user = await _dbContext.Members.Where(u => u.Email == email && u.Id != userId).FirstOrDefaultAsync();

			if (user != null)
			{
				return true;
			}

			return false;
		}
	}
}
