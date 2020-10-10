using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WpfVerein.Core.Entities;

namespace WpfVerein.Core.Contracts
{
    public interface IPersonRepository
    {
        Task AddMemberAsync(Member person);
        List<Member> GetAllMembers();
        Task<IEnumerable<Member>> GetAllMembersAsync();

        Task<Member> GetMemberByPhoneNumberAsync(string phoneNumber);
        Task<Member> GetMemberByFirstAndLastnameAsync(string firstname, string lastname);
        Task<IEnumerable<Member>> GetPersonsByDateAsync(DateTime date);
        Task<IEnumerable<Member>> GetPersonsForTodayAsync();

        void AttachMembersRange(List<Member> members);
        void RemoveCd(Member member);
    }
}
