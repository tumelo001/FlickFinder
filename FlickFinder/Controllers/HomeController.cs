
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
        private readonly HttpClient _httpClient;

        public HomeController(IWrapperRepository repo, HttpClient httpClient)
		{
            _repo = repo;
            _httpClient = httpClient;
        }

		[HttpGet]
		public async Task<IActionResult> Index()
		{

			var response = await _httpClient.GetAsync("https://api.themoviedb.org/3/trending/movie/week?api_key=ddaf2dd3a28f3f67bbfd39b53f1c066f");

			if (response.IsSuccessStatusCode)
			{
				var data = await response.Content.ReadAsStringAsync();

				MovieIndexResult movies = JsonConvert.DeserializeObject<MovieIndexResult>(data);

				return View(movies.results.ToList());	
			}

			return View();
		}

		
	}


	public class MovieIndexResult
	{
		public int page { get; set; }
		public MovieIndex[] results { get; set; }	
	}
}