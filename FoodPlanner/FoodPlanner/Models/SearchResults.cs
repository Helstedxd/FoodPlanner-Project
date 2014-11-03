using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner
{
    class SearchResults
    {
        public int RecipeID { get; set; }
        public Recipe Recipe { get; set; }
        public decimal RecipeQuantity { get; set; }
        public decimal InventoryQuantity { get; set; }
        public int IngredientCount { get; set; }

        public SearchResults() { }
    }
}
