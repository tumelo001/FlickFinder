using FlickFinder.Models;
using Microsoft.EntityFrameworkCore;

namespace FlickFinder.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions options) : base(options) { }
		

		public DbSet<Movie> Movies { get; set; }
		public DbSet<User> Users { get; set; }	

		/*protected override void OnModelBuilding(ModelBuilder modelBuilder)
		{

		}*/
	}
}
