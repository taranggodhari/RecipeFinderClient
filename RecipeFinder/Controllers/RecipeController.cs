using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RecipeFinder.Data;
using RecipeFinder.Models;

namespace RecipeFinder.Controllers
{
	public class RecipeController : Controller
	{
		private readonly ApplicationDbContext context;
		public RecipeController(ApplicationDbContext _context)
		{
			context = _context;
		}
		public async Task<ActionResult> Index()
		{

			List<Recipe> recipes = new List<Recipe>();

			using (var client = new HttpClient())
			{

				client.BaseAddress = new Uri("https://localhost:44333/");
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				//Add app key generated on apigee here
				//client.DefaultRequestHeaders.Add("appkey", "531909d58814fa2f57ebd012084dc0af");
				HttpResponseMessage Res = await client.GetAsync("api/recipe");

				if (Res.IsSuccessStatusCode)
				{
					var name = Res.Content.ReadAsStringAsync().Result;
					recipes = JsonConvert.DeserializeObject<List<Recipe>>(name);
				}
				//returning the employee list to view  
				return View(recipes.ToList());
			}
		}
		[HttpGet]
		public async Task<ActionResult> Search(string search)
		{

			List<Recipe> recipes = new List<Recipe>();

			using (var client = new HttpClient())
			{

				client.BaseAddress = new Uri("https://localhost:44333/");
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				HttpResponseMessage Res = await client.GetAsync("api/recipe/query/" + search);

				if (Res.IsSuccessStatusCode)
				{
					var name = Res.Content.ReadAsStringAsync().Result;
					recipes = JsonConvert.DeserializeObject<List<Recipe>>(name);
				}
				//returning the employee list to view  
				return View(recipes.ToList());
			}
		}
		[HttpPost]
		//[Authorize(Roles = "Student")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Save(Recipe recipe)
		{
			if (recipe != null)
			{
				var newrecipe = new Recipe
				{
					calories = recipe.calories,
					image = recipe.image,
					label = recipe.label,
					source = recipe.source,
					totalWeight = recipe.totalWeight,
					url = recipe.url,
					yield = recipe.yield,


				};
			}
			return RedirectToAction(nameof(Index));
		}
	}

}