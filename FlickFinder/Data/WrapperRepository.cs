namespace FlickFinder.Data
{
    public class WrapperRepository : IWrapperRepository
    {
        private readonly AppDbContext _appDbContext;
        //private IMovieRepository _movie;
        private IWatchListRepository _watchList;

       /* public IMovieRepository Movie
        {
            get
            {
                if (_movie == null)
                    _movie = new MovieRepository(_appDbContext);
                return _movie;
            }
        }*/

        public IWatchListRepository WatchList 
        {
            get
            {
                if (_watchList == null)
                    _watchList = new WatchListRepository(_appDbContext);
                return _watchList;
            } 
        }

        public WrapperRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public void SaveChanges()
        {
            _appDbContext.SaveChanges();    
        }
    }
}
