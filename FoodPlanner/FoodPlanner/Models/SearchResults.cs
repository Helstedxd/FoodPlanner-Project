using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner
{
    class SearchResults
    {
        public Recipe recipe { get; set; }
        public double match { get; set; }
        public int partialMatch { get; set; }
        public int fullMatch { get; set; }

        public SearchResults(Recipe recipe, bool isMatchIngredient, bool isMatchName)
        {
            int matchValue = 0;
            
            partialMatch = 0;
            fullMatch = 0;
            this.recipe = recipe;
            if (isMatchIngredient)
            {
                matchValue++;
            }

            if (isMatchName)
            {
                matchValue++;
            }

            this.match = matchValue;
        }
    }
}
