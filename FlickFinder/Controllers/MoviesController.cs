using FlickFinder.Data;
using FlickFinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace FlickFinder.Controllers
{
    public class MoviesController : Controller
    {

        private readonly IWrapperRepository _repo;
        private readonly HttpClient _httpClient;

        public MoviesController(IWrapperRepository repo, HttpClient httpClient)
        {
            _repo = repo;
            _httpClient = httpClient;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([Required] string searchInput)
        {
            if (!searchInput.IsNullOrEmpty() && searchInput.Length >= 3)
            {
                TempData["SearchInput"] = searchInput;
                searchInput = searchInput.Trim();
                var response = await _httpClient.GetAsync($"https://api.themoviedb.org/3/search/movie?query={searchInput}&include_adult=false&language=en-US&page=1&api_key=ddaf2dd3a28f3f67bbfd39b53f1c066f");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        SearchResult movies = JsonConvert.DeserializeObject<SearchResult>(data);
                        return View(movies.results.ToList());
                    }
                    catch
                    {
                        return View("NotFound");
                    }

                }
            }
            
            //ViewBag.ErrorSearchInput = "Your input should be at least 3 characters";
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var response = await _httpClient.GetAsync($"https://api.themoviedb.org/3/movie/{id}?api_key=ddaf2dd3a28f3f67bbfd39b53f1c066f");
            Movie movie = new();

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                movie = JsonConvert.DeserializeObject<Movie>(data);
            }

            

            var response_1 = await _httpClient.GetAsync($"http://www.omdbapi.com/?i={movie.imdbID}&p=full&apikey=6cb64284");
            if (response_1.IsSuccessStatusCode)
            {
                var data = await response_1.Content.ReadAsStringAsync();
                var movie_1 = JsonConvert.DeserializeObject<Movie>(data);

                var movieProperties = typeof(Movie).GetProperties();

                foreach (var property in movieProperties)
                {
                    var moviePropertyValue = property.GetValue(movie);
                    if(moviePropertyValue == null)
                    {
                        var movie_1PropertyValue = property.GetValue(movie_1);
                        property.SetValue(movie, movie_1PropertyValue);
                    }
                }

                return View(movie);
            }
            return View();
        }
    }

    public class SearchResult
    {
        public Movie[] results { get; set; }
        public int total_pages { get; set; }
        public int page { get; set; }
        public int total_results { get; set; }
    }
    /*public class Ratings 
    {
        //public Movie movie { get; set; }
        public object[] Rating { get; set; }
    }
*/


}
