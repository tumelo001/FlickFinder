using FlickFinder.Models;

namespace FlickFinder.Data
{
    public class WatchListRepository : RepositoryBase<WatchList>, IWatchListRepository
    {
        public WatchListRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
