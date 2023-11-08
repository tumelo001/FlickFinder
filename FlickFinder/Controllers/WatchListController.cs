using FlickFinder.Data;
using FlickFinder.Models;
using FlickFinder.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace FlickFinder.Controllers
{
    [Authorize]
    public class WatchListController : Controller
    {
        private readonly IWrapperRepository _repo;
        private readonly UserManager<AppUser> _userManager;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public WatchListController(IWrapperRepository repo, UserManager<AppUser> userManager,
            HttpClient httpClient, IConfiguration config)
        {
            _repo = repo;
            _userManager = userManager;
            _httpClient = httpClient;
            _config = config;
        }

        private string apiKey => _config.GetValue<string>("MovieApi:ApiKey");

        [TempData]
        public string Message { get; set; }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var watchlist = _repo.WatchList.FindAll().Where(w => w.UserName == user.UserName);

            List<HttpResponseMessage> responses = new List<HttpResponseMessage>();
            foreach (var watch in watchlist)
            {
                responses.Add(await _httpClient.GetAsync($"https://api.themoviedb.org/3/movie/{watch.MovieId}?api_key={apiKey}"));
            }

            List<Movie> movies = new List<Movie>();
            foreach (var response in responses)
            {
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<Movie>(data);
                    movies.Add(JsonConvert.DeserializeObject<Movie>(data));
                    for (int i = 0; i < movies.Count; i++)
                    {
                        if (watchlist.FirstOrDefault(w => w.MovieId == movies[i].MovieId && w.UserName == User.Identity.Name) != null)
                            movies[i].IsInWatchList = true;
                    }
                }
            }

            return View(movies);
        }
        public async Task<IActionResult> Add(string Id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var watchlist = new WatchList
            {
                MovieId = Id,
                UserName = user.UserName
            };
            if (_repo.WatchList.FindAll().FirstOrDefault(w => w.MovieId == watchlist.MovieId && w.UserName == user.UserName) == null)
            {
                _repo.WatchList.Create(watchlist);
                _repo.SaveChanges();

            }
            else
            {
                var response = await _httpClient.GetAsync($"https://api.themoviedb.org/3/movie/{Id}?api_key={apiKey}");
                if (response.IsSuccessStatusCode)
                {
                    Movie movie = new();
                    var data = await response.Content.ReadAsStringAsync();
                    movie = JsonConvert.DeserializeObject<Movie>(data);
                    return RedirectToAction("Remove", new { Id , movie.Title });
                }
                
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Remove(string Id, string title)
        {
            var movieWatchlist = _repo.WatchList.FindAll().FirstOrDefault(m => m.MovieId == Id && m.UserName == User.Identity.Name);

            if (movieWatchlist != null)
            {
                return View(new RemoveViewModel { Id = movieWatchlist.MovieId, Title = title });
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Remove(string id)
        {
            var watchList = await _repo.WatchList.FindAll().FirstOrDefaultAsync(m => m.MovieId == id && m.UserName == User.Identity.Name);
            if (watchList != null)
            {
                _repo.WatchList.Delete(watchList);
                _repo.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
