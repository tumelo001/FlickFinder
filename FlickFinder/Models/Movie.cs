using FlickFinder.Controllers;
using Newtonsoft.Json;


namespace FlickFinder.Models
{
    public class MovieIndex
    {
        [JsonProperty("id")]
        public string MovieId { get; set; }

        [JsonProperty("title")]
        public string Title  { get; set; }

        [JsonProperty("poster_path")]
        public string Poster { get; set; }


    }


	public class Movie
	{
        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Year")]
        public string Year { get; set; }

        [JsonProperty("Rated")]
        public string Rated { get; set; }

        [JsonProperty("Released")]
        public string Released { get; set; }

        [JsonProperty("Runtime")]
        public string Runtime { get; set; }

        [JsonProperty("Genre")]
        public string Genre { get; set; }

        [JsonProperty("Director")]
        public string Director { get; set; }

        [JsonProperty("Writer")]
        public string Writer { get; set; }

        [JsonProperty("Actors")]
        public string Actors { get; set; }

        [JsonProperty("Plot")]
        public string Plot { get; set; }

        [JsonProperty("Language")]
        public string Language { get; set; }

        [JsonProperty("Country")]
        public string Country { get; set; }

        [JsonProperty("Awards")]
        public string Awards { get; set; }

        [JsonProperty("Poster")]
        public string Poster { get; set; }

        
        [JsonProperty("imdbID")]
        public string imdbID { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("imdbRating")]
        public string imdbRating { get; set; }

    }
}
