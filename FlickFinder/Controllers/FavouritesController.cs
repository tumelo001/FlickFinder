using Microsoft.AspNetCore.Mvc;

namespace FlickFinder.Controllers
{
    public class FavouritesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
