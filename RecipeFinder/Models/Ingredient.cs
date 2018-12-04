using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeFinder.Models
{
	public class Ingredient
	{
		[Key]
		public int Id { get; set; }

		public int RecipeId { get; set; }

		public string text { get; set; }

		public float weight { get; set; }
		[ForeignKey("RecipeId")]
		public virtual Recipe Recipe { get; set; }
	}
}
