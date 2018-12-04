using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeFinder.Models
{
	public class Recipe
	{
		[Key]
		public int Id { get; set; }

		public string label { get; set; }

		public string image { get; set; }

		public string source { get; set; }

		public string url { get; set; }

		public float yield { get; set; }
		public virtual ICollection<Ingredient> ingredients { get; set; }

		public float calories { get; set; }

		public float totalWeight { get; set; }
	}
}
