namespace FlickFinder.Models
{
    public class Favourite
    {
        public int Id { get; set; }

        public string MovieId { get; set; }
        public string UserId { get; set; }


        public AppUser User { get; set; }

    }
}
