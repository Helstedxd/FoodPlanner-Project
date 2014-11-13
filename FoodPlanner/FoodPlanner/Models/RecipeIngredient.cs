using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner.Models
{
    public partial class RecipeIngredient
    {
        public override string ToString()
        {
            if (this.Quantity == 0)//Eliminate if there is no quantity
            {
                return Convert.ToString(this.Ingredient.Name);
            }
            else if (string.IsNullOrWhiteSpace(this.Ingredient.Unit)) //Eliminate the double space that can occour when there is no unit 
            {
                return Convert.ToString(this.Quantity + " " + this.Ingredient.Name);
            }
            else
            {
                return Convert.ToString(this.Quantity + " " + this.Ingredient.Unit + " " + this.Ingredient.Name);
            }
        }
    }
}
