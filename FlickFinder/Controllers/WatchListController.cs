using FlickFinder.Data;
using FlickFinder.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
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

        public WatchListController(IWrapperRepository repo, UserManager<AppUser> userManager, HttpClient httpClient)
        {
            _repo = repo;
            _userManager = userManager;
            _httpClient = httpClient;
        }

        [TempData]
        public string Message { get; set; }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var watchlist = _repo.WatchList.FindAll().Where(w => w.UserId == user.Id);

            List<HttpResponseMessage> responses = new List<HttpResponseMessage>();
            foreach (var watch in watchlist)
            {
                 responses.Add(await _httpClient.GetAsync($"https://api.themoviedb.org/3/movie/{watch.MovieId}?api_key=ddaf2dd3a28f3f67bbfd39b53f1c066f"));
            }

            List<Movie> movies = new List<Movie>();
            foreach (var response in responses)
            {
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    movies.Add(JsonConvert.DeserializeObject<Movie>(data));
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
                UserId = user.Id
            };
            Message = "Movie already in Watchlist";
            if (_repo.WatchList.FindAll().FirstOrDefault(w => w.MovieId == watchlist.MovieId && w.UserId == user.Id) != null)
            {
                _repo.WatchList.Create(watchlist);
                _repo.SaveChanges();
                Message = "Added to WatchList";
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Remove()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            if (id != 0)
            {
                var watchList = await _repo.WatchList.GetById(id);
                _repo.WatchList.Delete(watchList);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
