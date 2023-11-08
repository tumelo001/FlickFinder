using FlickFinder.Data;
using FlickFinder.DTOs;
using FlickFinder.Models;
using FlickFinder.Models.ViewModels;
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
        private readonly IConfiguration _config;

        private string apiKey => _config.GetValue<string>("MovieApi:ApiKey");
        private string omdbapiKey => _config.GetValue<string>("MovieApi:omdbapiKey");

        public MoviesController(IWrapperRepository repo, HttpClient httpClient, IConfiguration config)
        {
            _repo = repo;
            _httpClient = httpClient;
            _config = config;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([Required] string search)
        {
            ViewBag.Search = search;
            if (!search.IsNullOrEmpty() && search.Length >= 3)
            {
                TempData["SearchInput"] = search;
                search = search.Trim();
                var response = await _httpClient.GetAsync($"https://api.themoviedb.org/3/search/movie?query={search}&include_adult=false&language=en-US&page=1&api_key={apiKey}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        MovieResultDTO movies = JsonConvert.DeserializeObject<MovieResultDTO>(data);
                        var watchlist = _repo.WatchList.FindAll();

                        if (User.Identity.IsAuthenticated)
                        {
                            for (int i = 0; i < movies.results.Length; i++)
                            {
                                if (watchlist.FirstOrDefault(w => w.MovieId == movies.results[i].MovieId && w.UserName == User.Identity.Name) != null)
                                    movies.results[i].IsInWatchList = true;
                            }
                        }
                        if (movies.results.Length == 0)
                            return View("NotFound");
                        return View(movies.results.ToList());
                    }
                    catch
                    {
                        return View("NotFound");
                    }

                }
                else
                    return View("NotFound");
            }

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var response = await _httpClient.GetAsync($"https://api.themoviedb.org/3/movie/{id}?api_key={apiKey}");
            MovieDetailsViewModel model = new();

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                model.Movie = JsonConvert.DeserializeObject<Movie>(data);
            }

            var response_1 = await _httpClient.GetAsync($"http://www.omdbapi.com/?i={model.Movie.imdbID}&p=full&apikey={omdbapiKey}");
            if (response_1.IsSuccessStatusCode)
            {
                var data = await response_1.Content.ReadAsStringAsync();
                var movie_1 = JsonConvert.DeserializeObject<Movie>(data);

                var movieProperties = typeof(Movie).GetProperties();

                foreach (var property in movieProperties)
                {
                    var moviePropertyValue = property.GetValue(model.Movie);
                    if (moviePropertyValue == null)
                    {
                        var movie_1PropertyValue = property.GetValue(movie_1);
                        property.SetValue(model.Movie, movie_1PropertyValue);
                    }
                }
                if (User.Identity.IsAuthenticated)
                {
                    var watchlist = _repo.WatchList.FindAll().Where(w => w.UserName == User.Identity.Name);
                    if (watchlist.FirstOrDefault(w => w.MovieId == model.Movie.MovieId && w.UserName == User.Identity.Name) != null)
                        model.Movie.IsInWatchList = true;
                }

                string[] categories = { "recommendations", "similar" };

                foreach (var category in categories)
                {
                    var res = await _httpClient.GetAsync($"https://api.themoviedb.org/3/movie/{id}/{category}?api_key={apiKey}");
                    if (res.IsSuccessStatusCode)
                    {
                        var data_ = await res.Content.ReadAsStringAsync();
                        var movieResult = JsonConvert.DeserializeObject<MovieResultDTO>(data_);
                        if (category == "recommendations")
                            model.Recommendations = movieResult.results;
                        else
                            model.Similar = movieResult.results;
                    }
                }
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
