using Newtonsoft.Json;

namespace FlickFinder.Models
{
    public class Genre
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string GenreName { get; set; }


        public IEnumerable<UserGenre> UserGenres { get; set; }
    }
}
