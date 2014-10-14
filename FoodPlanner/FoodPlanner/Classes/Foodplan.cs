using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner
{
    class Foodplan
    {
        private List<Meal> meals;
        private int week;

        public Foodplan(int week)
        {
            this.week = week;
            this.meals = new List<Meal>();
        }

        public void AddMeal(Meal meal)
        {
            meals.Add(meal);
        }
    }
}
