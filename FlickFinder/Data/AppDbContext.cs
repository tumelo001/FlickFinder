using FlickFinder.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlickFinder.Data
{
	public class AppDbContext : IdentityDbContext<AppUser>
    {
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
		
		public DbSet<WatchList> WatchList { get; set; }
		public DbSet<Favourite> Favourites { get; set; }
		public DbSet<Genre> Genres { get; set; }
		public DbSet<UserGenre> UserGenres { get; set; }

	}
}
