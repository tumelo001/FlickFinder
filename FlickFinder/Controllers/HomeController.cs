using FlickFinder.Data;
using FlickFinder.DTOs;
using FlickFinder.Models;
using FlickFinder.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;

namespace FlickFinder.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IWrapperRepository _repo;

        protected class CategoryProp
        {
            public string ApiName { get; set; }
            public string PropName { get; set; }
        }

        private static List<CategoryProp> _movieCategery => new List<CategoryProp>()
        {
            new CategoryProp{ ApiName = @"trending/movie/week", PropName = "Trending"},
            new CategoryProp { ApiName = "now_playing", PropName = "NowPlaying" },
            new CategoryProp { ApiName = "top_rated", PropName = "TopRated" }
        };

        public HomeController(HttpClient httpClient, IConfiguration config, IWrapperRepository repo)
        {
            _httpClient = httpClient;
            _config = config;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            MoviesHomePageViewModel model = new MoviesHomePageViewModel();
            //var ls = model.GetType().GetRuntimeFields().Select(p => p.Name.ToString());
            var watchlist = _repo.WatchList.FindAll();

            foreach (var category in _movieCategery)
            {
                var response = await _httpClient.GetAsync(ApiRequestURL(category.ApiName));
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    MovieResultDTO movie = JsonConvert.DeserializeObject<MovieResultDTO>(data);
                    var m = model.GetType().GetProperty(category.PropName);

                    if (User.Identity.IsAuthenticated)
                    {
                        for (int i = 0; i < movie.results.Length; i++)
                        {
                            if (watchlist.FirstOrDefault(w => w.MovieId == movie.results[i].MovieId && w.UserName == User.Identity.Name) != null)
                                movie.results[i].IsInWatchList = true;
                        }
                    }
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
}