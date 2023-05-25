using FlickFinder.Data;
using FlickFinder.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;

namespace FlickFinder.Controllers
{
    [Authorize]
    public class WatchListController : Controller
    {
        private readonly IWrapperRepository _repo;
        private readonly UserManager<AppUser> _userManager;

        public WatchListController(IWrapperRepository repo, UserManager<AppUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }

        [TempData]
        public string Message { get; set; }

        public IActionResult Index()
        {
            return View(_repo.WatchList.FindAll());
        }
        public IActionResult Add(string Id)
        {
            var watchlist = new WatchList
            {
                MovieId = Id

                //UserId = (await _userManager.FindByNameAsync(User.Identity.Name)).Id
            };      
            _repo.WatchList.Create(watchlist);
            _repo.SaveChanges();
            Message = "Added to WatchList"; 
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
