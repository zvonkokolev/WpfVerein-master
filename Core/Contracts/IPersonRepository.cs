using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WpfVerein.Core.Entities;

namespace WpfVerein.Core.Contracts
{
	public interface IPersonRepository
	{
		Task AddMemberAsync(Member person);		
		Task<IEnumerable<Member>> GetAllMembersAsync();

		Task<Member> GetMemberByPhoneNumberAsync(string phoneNumber);
		Task<Member> GetMemberByFirstAndLastnameAsync(string firstname, string lastname);
		Task<IEnumerable<Member>> GetPersonsByDateAsync(DateTime date);
		Task<IEnumerable<Member>> GetPersonsForTodayAsync();

		List<Member> GetAllMembers();
		void AddMember(Member member);
		void AttachMembersRange(List<Member> members);
		void RemoveCd(Member member);
		void UpdateCd(Member oldMember);
		Task<bool> CheckIfUserEmailExists(string email, int userId);
	}
}
