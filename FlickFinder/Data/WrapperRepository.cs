namespace FlickFinder.Data
{
    public class WrapperRepository : IWrapperRepository
    {
        private readonly AppDbContext _appDbContext;
        private IMovieRepository _movie;

        public IMovieRepository Movie
        {
            get
            {
                if (_movie == null)
                    _movie = new MovieRepository(_appDbContext);
                return _movie;
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
