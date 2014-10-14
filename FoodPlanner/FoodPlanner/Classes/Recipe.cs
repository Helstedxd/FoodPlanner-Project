using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner
{
    class Recipe
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public TimeSpan CookingTime { get; set; }
        public List<string> CookingSteps { get; private set; }
        public List<Food> Ingredients { get; private set; }

        public Recipe(string name, string description, TimeSpan cookingTime, string imagePath)
        {
            this.Name = name;
            this.Description = description;
            this.CookingTime = cookingTime;
            this.ImagePath = imagePath;
            this.CookingSteps = new List<string>();
            this.Ingredients = new List<Food>();
        }
    }
}
