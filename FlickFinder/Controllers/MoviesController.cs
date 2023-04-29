using FlickFinder.Data;
using FlickFinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

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


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([Required] string searchInput)
        {
            searchInput = searchInput.Trim();
            var response = await _httpClient.GetAsync($"http://www.omdbapi.com/?s={searchInput}&apikey=6cb64284");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();


                SearchResult movies = JsonConvert.DeserializeObject<SearchResult>(data);


                return View(movies.Search.ToList());
            }
            else
            {

            }

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var response = await _httpClient.GetAsync($"http://www.omdbapi.com/?i={id}&apikey=6cb64284");
            if (response.IsSuccessStatusCode) 
            {
                var data = await response.Content.ReadAsStringAsync();
                Movie movie = JsonConvert.DeserializeObject<Movie>(data);
                return View(movie);
            }
            return View();
        }
    }

    public class SearchResult  
    {
        public Movie[] Search; 
        public int totalResults { get; set; }
        public bool Response { get; set; }
    }
    /*public class Ratings 
    {
        //public Movie movie { get; set; }
        public object[] Rating { get; set; }
    }
*/


}
