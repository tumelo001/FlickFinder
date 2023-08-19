
using FlickFinder.Data;
using FlickFinder.Models;
using FlickFinder.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;

namespace FlickFinder.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        protected class CategoryProp
        {
            public string ApiName { get; set; }
            public string PropName { get; set; }
        }

        private static List<CategoryProp> _movieCategery => new List<CategoryProp>()
        {
            new CategoryProp{ ApiName = @"trending/movie/week", PropName = "Trending"},
            new CategoryProp { ApiName = "now_playing", PropName = "NowPlaying" },
            new CategoryProp { ApiName = "popular", PropName = "Popular" },
            new CategoryProp { ApiName = "top_rated", PropName = "TopRated" },
            new CategoryProp { ApiName = "upcoming", PropName = "Upcoming" }
        };

        public HomeController(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            MoviesHomePageViewModel model = new MoviesHomePageViewModel();
            var ls = model.GetType().GetRuntimeFields().Select(p => p.Name.ToString());

            foreach (var category in _movieCategery)
            {
                var response = await _httpClient.GetAsync(ApiRequestURL(category.ApiName));
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    MovieResult movie = JsonConvert.DeserializeObject<MovieResult>(data);
                    var m = model.GetType().GetProperty(category.PropName);
                    m.SetValue(model, movie.results);
                }
            }
            return View(model);
        }

        public string ApiRequestURL(string category, int page = 1)
        {
            string apikey = _config.GetValue<string>("MovieApi:ApiKey");
            category = category.Contains("trending") ? category : "movie/" + category;
            return $"https://api.themoviedb.org/3/{category}?api_key={apikey}";
        }

    }

    public class MovieResult
    {
        public int page { get; set; }
        public Movie[] results { get; set; }
    }
}