using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner
{
    class Foodplan
    {
        private List<Meal> meals = new List<Meal>();
        private int week = 0;


        public Foodplan(int Week)
        {
            week = Week;
        }

        public void addMeal(Meal meal)
        {
            meals.Add(meal);
        }
    }
}
