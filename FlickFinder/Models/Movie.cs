using FlickFinder.Controllers;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace FlickFinder.Models
{
    public class Movie
    {

        [Key]
        [JsonProperty("id")]
        public string MovieId { get; set; }

        [JsonProperty("imdb_id")]
        public string imdbID { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("tagline")]
        public string Tagline { get; set; }

        [JsonProperty("release_date")]
        public string Year { get; set; }

        [JsonProperty("Rated")]
        public string Rated { get; set; }

        [JsonProperty("status")]
        public string Released { get; set; }

        [JsonProperty("runtime")]
        public string Runtime { get; set; }

        [JsonProperty("Genre")]
        public string Genre { get; set; }

        [JsonProperty("Director")]
        public string Director { get; set; }

        [JsonProperty("Writer")]
        public string Writer { get; set; }

        [JsonProperty("Actors")]
        public string Actors { get; set; }

        [JsonProperty("overview")]
        public string Plot { get; set; }

        [JsonProperty("Language")]
        public string Language { get; set; }

        [JsonProperty("Country")]
        public string Country { get; set; }

        [JsonProperty("Awards")]
        public string Awards { get; set; }

        private string _poster;

        [JsonProperty("poster_path")]
        public string Poster
        {
            get
            {
                return _poster;
            }
            set
            {
                _poster = "https://image.tmdb.org/t/p/w500/" + value;
                if (value == null)
                    _poster = "/images/default.jpg";
            }
        }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("imdbRating")]
        public string Rating { get; set; }


        public string Slug => Title?.Replace(" ", "+").ToLower() + "+" + Year.ToLower();

        public bool IsInWatchList { get; set; } = false;
        public bool IsInFavourite { get; set; } = false;

    }
}
