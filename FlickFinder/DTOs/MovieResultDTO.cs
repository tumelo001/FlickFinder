using FlickFinder.Models;

namespace FlickFinder.DTOs
{
    public class MovieResultDTO
    {
        public Movie[] results { get; set; }
        public int total_pages { get; set; }
        public int page { get; set; }
        public int total_results { get; set; }
    }
}
