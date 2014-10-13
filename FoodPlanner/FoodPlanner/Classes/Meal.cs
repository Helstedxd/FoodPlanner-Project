using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner
{
    class Meal
    {
        private DateTime date = DateTime.MinValue;
        private Recipe recipe = null;
        private int persons = Properties.Settings.Default.Persons;


        public Meal(DateTime Date, Recipe Recipe, int Persons)
        {
            date = Date;
            recipe = Recipe;
            persons = Persons;
        }
    }
}
