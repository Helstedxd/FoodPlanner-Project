using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner.Models
{
    public class Recipe
    {

        // A property named ID is treated as a PK by default
        public int ID { get; set; }

        public virtual ICollection<Food> Ingredients { get; private set; }

        protected Recipe() { }

        [Index(IsUnique = true)]
        [StringLength(200)]
        public string Title { get; set; }

        public string ImagePath { get; set; }
        public string Description { get; set; }
       // public TimeSpan CookingTime { get; set; }
        public int CookingTime { get; set; }
        //public List<string> CookingSteps { get; private set; } //TODO: not ef ready.

        public Recipe(string title, string description, int cookingTime, string imagePath)
        {
            this.Title = title;
            this.Description = description;
            this.CookingTime = cookingTime; // cookingTime; //TODO: db can not handle all timespan range
            this.ImagePath = imagePath;
            //this.CookingSteps = new List<string>();
            this.Ingredients = new List<Food>();
        }
    }

}
