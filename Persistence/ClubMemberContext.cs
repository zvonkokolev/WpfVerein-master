using Microsoft.EntityFrameworkCore;
using WpfVerein.Core.Entities;

namespace WpfVerein.Model
{
	public class ClubMemberContext : DbContext
	{
		public DbSet<Member> Members { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder options)
			=> options.UseSqlite("Data Source=administration.db");
	}
}
