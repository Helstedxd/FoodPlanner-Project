using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodPlanner.Models;

namespace FoodPlanner.ViewModels
{
    public class RecipeViewModel
    {

        public Recipe Recipe { get; set; }

        public RecipeViewModel(Recipe recipe) {
            this.Recipe = recipe;
        }

    }
}
