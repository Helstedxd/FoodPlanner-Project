using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner.Models
{
    class Meal
    {
        public Recipe Recipe { get; set; }
        public DateTime Date { get; set; }
        public int Persons { get; set; }

        public Meal(Recipe recipe, DateTime date, int persons)
        {
            this.Recipe = recipe;
            this.Date = date;
            this.Persons = persons;
        }

        public Meal(Recipe recipe)
            : this(recipe, DateTime.Now, Properties.Settings.Default.Persons)
        {
        }

    }
}
