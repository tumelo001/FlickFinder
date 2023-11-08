using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlickFinder.Models
{
    public class WatchList
    {

        public int Id { get; set; }
        public string MovieId { get; set; }

        public string UserName { get; set; }

        public AppUser User { get; set; }
        
    }
}
