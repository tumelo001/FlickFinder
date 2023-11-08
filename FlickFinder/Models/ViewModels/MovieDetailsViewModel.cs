namespace FlickFinder.Models.ViewModels
{
    public class MovieDetailsViewModel
    {
        public Movie Movie { get; set; }
        public IEnumerable<Movie> Recommendations { get; set; }
        public IEnumerable<Movie> Similar { get; set; }
    }
}
