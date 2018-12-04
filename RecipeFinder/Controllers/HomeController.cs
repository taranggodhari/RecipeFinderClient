using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RecipeFinder.Models;

namespace RecipeFinder.Controllers
{
	public class HomeController : Controller
	{
		public async Task<ActionResult> Index()
		{
			List<Recipe> recipes = new List<Recipe>();

			using (var client = new HttpClient())
			{

				client.BaseAddress = new Uri("https://localhost:44333/");
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				HttpResponseMessage Res = await client.GetAsync("api/recipe");

				if (Res.IsSuccessStatusCode)
				{


					var name = Res.Content.ReadAsStringAsync().Result;
					recipes = JsonConvert.DeserializeObject<List<Recipe>>(name);


				}
			}
			return View(recipes.ToList());
		}

		public IActionResult About()
		{
			ViewData["Message"] = "Your application description page.";

			return View();
		}

		public IActionResult Contact()
		{
			ViewData["Message"] = "Your contact page.";

			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
