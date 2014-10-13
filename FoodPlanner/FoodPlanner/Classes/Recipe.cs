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
        private int persons = 1;
        private string image = null;
        private List<string> cookingSteps = new List<string>();
        private List<Food> ingredients = new List<Food>();

        public Recipe()
        {

        }
    }
}
