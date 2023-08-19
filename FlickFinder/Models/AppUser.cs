using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlickFinder.Models
{
    public class AppUser : IdentityUser
    {
        public string AvatarImageURL { get; set; }  

        public IEnumerable<WatchList> WatchList { get; set; }
        public IEnumerable<UserGenre> UserGenres { get; set; }
        public IEnumerable<Favourite> Favourites { get; set; }
    }
}

