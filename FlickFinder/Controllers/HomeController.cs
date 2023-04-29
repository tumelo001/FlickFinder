
using FlickFinder.Data;
using FlickFinder.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace FlickFinder.Controllers
{
	public class HomeController : Controller
	{
        private readonly IWrapperRepository _repo;

        public HomeController(IWrapperRepository repo)
		{
            _repo = repo;
        }

		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		
	}
}