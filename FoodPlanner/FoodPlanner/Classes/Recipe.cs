using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner
{
    class Recipe
    {
        private string name = null;
        private int persons = Properties.Settings.Default.Persons;
        private string image = null;
        private string description = null;
        private List<string> cookingSteps = new List<string>();
        private List<Food> ingredients = new List<Food>();
        private int cookingTime = 0;

        public Recipe(string Name, int Persons, string Image, string Description, List<string> CookingSteps, List<Food> Ingredients, int CookingTime)
        {
            name = Name;
            persons = Persons;
            image = Image;
            description = Description;
            cookingSteps = CookingSteps;
            ingredients = Ingredients;
            cookingTime = CookingTime;
        }
    }
}
