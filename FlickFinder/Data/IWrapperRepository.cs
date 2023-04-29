namespace FlickFinder.Data
{
    public interface IWrapperRepository
    {
        IMovieRepository Movie { get; }
        void SaveChanges();
    }
}
