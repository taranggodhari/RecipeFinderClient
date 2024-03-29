﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
		private readonly UserManager<ApplicationUser> userManager;
		public RecipeController(ApplicationDbContext _context, UserManager<ApplicationUser> _userManager)
		{
			userManager = _userManager;
			context = _context;
		}
		public async Task<ActionResult> Index()
		{

			List<Recipe> recipes = new List<Recipe>();

			using (var client = new HttpClient())
			{

				client.BaseAddress = new Uri("http://taranggodhari-eval-test.apigee.net/");
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				//Add app key generated on apigee here
				client.DefaultRequestHeaders.Add("apikey", "ojqE3hNqm5MyUTRQQlKxwDFLZZkkw2n5");
				HttpResponseMessage Res = await client.GetAsync("recipeapiproxy/api/recipe");

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

				client.BaseAddress = new Uri("http://taranggodhari-eval-test.apigee.net/");
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				client.DefaultRequestHeaders.Add("apikey", "ojqE3hNqm5MyUTRQQlKxwDFLZZkkw2n5");
				HttpResponseMessage Res = await client.GetAsync("recipeapiproxy/api/recipe/query/" + search);

				if (Res.IsSuccessStatusCode)
				{
					var name = Res.Content.ReadAsStringAsync().Result;
					recipes = JsonConvert.DeserializeObject<List<Recipe>>(name);
				}
				ViewBag.SearchTerm = search;
				//returning the employee list to view  
				return View(recipes.ToList());
			}
		}
		[Authorize]
		[HttpGet]
		public async Task<ActionResult> GetSavedRecipe()
		{

			var user = await userManager.GetUserAsync(User);
			List<Recipe> recipes = new List<Recipe>();
			using (var client = new HttpClient())
			{

				client.BaseAddress = new Uri("http://taranggodhari-eval-test.apigee.net/");
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				client.DefaultRequestHeaders.Add("apikey", "ojqE3hNqm5MyUTRQQlKxwDFLZZkkw2n5");
				HttpResponseMessage Res = await client.GetAsync("recipeapiproxy/api/recipe/userrecipe/" + user.Email);

				if (Res.IsSuccessStatusCode)
				{
					var name = Res.Content.ReadAsStringAsync().Result;
					recipes = JsonConvert.DeserializeObject<List<Recipe>>(name);
				}
				//returning the employee list to view  
				return View(recipes.ToList());
			}
		}
		[Authorize]
		[HttpGet]
		public async Task<ActionResult> GetSavedRecipeId()
		{

			var user = await userManager.GetUserAsync(User);
			List<Recipe> recipes = new List<Recipe>();
			List<RecipeId> recipeids = new List<RecipeId>();
			using (var client = new HttpClient())
			{

				client.BaseAddress = new Uri("http://taranggodhari-eval-test.apigee.net/");
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				client.DefaultRequestHeaders.Add("apikey", "ojqE3hNqm5MyUTRQQlKxwDFLZZkkw2n5");
				HttpResponseMessage Res = await client.GetAsync("recipeapiproxy/api/recipe/userrecipe/" + user.Email);

				if (Res.IsSuccessStatusCode)
				{
					var name = Res.Content.ReadAsStringAsync().Result;
					recipes = JsonConvert.DeserializeObject<List<Recipe>>(name);

				}
				foreach (var recipe in recipes)
				{
					RecipeId recipeId = new RecipeId()
					{
						id = recipe.Id
					};
					recipeids.Add(recipeId);
				}
				//returning the employee list to view  
				return Ok(recipeids.ToList());
			}
		}
		[HttpGet]
		public async Task<ActionResult> RecipeDetails(string rid)
		{
			var id = Convert.ToInt32(rid);
			Recipe recipe = new Recipe();

			using (var client = new HttpClient())
			{

				client.BaseAddress = new Uri("http://taranggodhari-eval-test.apigee.net/");
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				client.DefaultRequestHeaders.Add("apikey", "ojqE3hNqm5MyUTRQQlKxwDFLZZkkw2n5");
				HttpResponseMessage Res = await client.GetAsync("recipeapiproxy/api/recipe/id/" + id);

				if (Res.IsSuccessStatusCode)
				{
					var name = Res.Content.ReadAsStringAsync().Result;
					recipe = JsonConvert.DeserializeObject<Recipe>(name);
				}
				//returning the employee list to view  
				return View(recipe);
			}
		}
		[Authorize]
		[HttpPost]
		//[Authorize(Roles = "Student")]
		public async Task<IActionResult> SaveRecipe(string rid, string searchterm)
		{
			List<Recipe> recipes = new List<Recipe>();
			using (var client = new HttpClient())
			{

				client.BaseAddress = new Uri("http://taranggodhari-eval-test.apigee.net/");
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				client.DefaultRequestHeaders.Add("apikey", "ojqE3hNqm5MyUTRQQlKxwDFLZZkkw2n5");
				var user = await userManager.GetUserAsync(User);
				var email = user.Email;
				PostRecipe postrecipe = new PostRecipe()
				{
					RecipeId = rid,
					Email = email
				};
				HttpResponseMessage response = await client.PostAsJsonAsync("recipeapiproxy/api/recipe", postrecipe);
				if (searchterm == "" || searchterm == null)
				{
					return RedirectToAction("GetSavedRecipe", "Recipe");
				}
				else
				{
					return RedirectToAction("Search", "Recipe", new { search = searchterm.ToString() });
				}
			}
		}
		[Authorize]
		[HttpGet]
		//[Authorize(Roles = "Student")]
		public async Task<IActionResult> UpdateRecipe(string rid, string searchterm)
		{
			var id = Convert.ToInt32(rid);
			Recipe recipe = new Recipe();

			using (var client = new HttpClient())
			{

				client.BaseAddress = new Uri("http://taranggodhari-eval-test.apigee.net/");
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				client.DefaultRequestHeaders.Add("apikey", "ojqE3hNqm5MyUTRQQlKxwDFLZZkkw2n5");

				HttpResponseMessage Res = await client.GetAsync("recipeapiproxy/api/recipe/id/" + id);

				if (Res.IsSuccessStatusCode)
				{
					var name = Res.Content.ReadAsStringAsync().Result;
					recipe = JsonConvert.DeserializeObject<Recipe>(name);
				}
				//returning the employee list to view  
				return View(recipe);
			}
		}
		[Authorize]
		[HttpPost]
		//[Authorize(Roles = "Student")]
		public async Task<IActionResult> UpdateThisRecipe(Recipe recipe)
		{
			var user = await userManager.GetUserAsync(User);

			using (var client = new HttpClient())
			{

				client.BaseAddress = new Uri("http://taranggodhari-eval-test.apigee.net/");
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				client.DefaultRequestHeaders.Add("apikey", "ojqE3hNqm5MyUTRQQlKxwDFLZZkkw2n5");
				HttpResponseMessage GetRes = await client.GetAsync("recipeapiproxy/api/recipe/id/" + recipe.Id);

				if (GetRes.IsSuccessStatusCode)
				{
					var name = GetRes.Content.ReadAsStringAsync().Result;
					var newrecipe = JsonConvert.DeserializeObject<Recipe>(name);

					recipe.ingredients = newrecipe.ingredients;
					HttpResponseMessage Res = await client.PutAsJsonAsync("recipeapiproxy/api/recipe/" + recipe.Id + "/" + user.Email, recipe);
					if (Res.IsSuccessStatusCode)
					{
						return RedirectToAction("RecipeDetails", "Recipe", new { rid = recipe.Id });
					}
					else
					{
						return RedirectToAction("UpdateRecipe", "Recipe", new { rid = recipe.Id });
					}
				}
				else
				{
					return RedirectToAction("UpdateRecipe", "Recipe", new { rid = recipe.Id });
				}
			}
		}
		[Authorize]
		[HttpPost]
		//[Authorize(Roles = "Student")]
		public async Task<IActionResult> DeleteRecipe(string rid, string searchterm)
		{
			List<Recipe> recipes = new List<Recipe>();
			using (var client = new HttpClient())
			{

				client.BaseAddress = new Uri("http://taranggodhari-eval-test.apigee.net/");
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				client.DefaultRequestHeaders.Add("apikey", "ojqE3hNqm5MyUTRQQlKxwDFLZZkkw2n5");
				var user = await userManager.GetUserAsync(User);

				HttpResponseMessage response = await client.DeleteAsync("recipeapiproxy/api/recipe/" + rid + "/" + user.Email);
				if (searchterm == "" || searchterm == null)
				{
					return RedirectToAction("GetSavedRecipe", "Recipe");
				}
				else
				{
					return RedirectToAction("Search", "Recipe", new { search = searchterm.ToString() });
				}
			}
		}

	}
}