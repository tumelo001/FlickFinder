namespace FlickFinder.Models.ViewModels
{
    public class MoviesHomePageViewModel
    {
        public IEnumerable<Movie> Trending { get; set; }
        public IEnumerable<Movie> Popular { get; set; }
        public IEnumerable<Movie> TopRated { get; set; }
        public IEnumerable<Movie> Upcoming { get; set; }
        public IEnumerable<Movie> NowPlaying { get; set; }


        public IEnumerable<Movie> WatchList { get; set; }

        public IEnumerable<Movie> Recommended { get; set; }
    }
}
