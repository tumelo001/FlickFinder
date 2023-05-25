namespace FlickFinder.Data
{
    public interface IWrapperRepository
    {
        /*IMovieRepository Movie { get; }*/
        IWatchListRepository WatchList { get; }
        void SaveChanges();
    }
}
