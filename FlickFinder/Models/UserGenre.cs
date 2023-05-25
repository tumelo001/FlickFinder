namespace FlickFinder.Models
{
    public class UserGenre
    {
        public int Id { get; set; }

        public int GenreId { get; set; }
        public string UserId { get; set; }

        public AppUser User { get; set; }
        public Genre Genre { get; set; }
    }
}
