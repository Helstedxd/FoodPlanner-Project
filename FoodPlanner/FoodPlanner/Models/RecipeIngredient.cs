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
            string returnString = null;

            if (this.Quantity != 0)//Eliminate if there is no quantity
            {
                returnString += this.Quantity + " ";
            }

            if (!string.IsNullOrWhiteSpace(this.Ingredient.Unit)) //Eliminate the double space that can occour when there is no unit 
            {
                returnString += this.Ingredient.Unit + " ";
            }

            returnString += this.Ingredient.Name;

            return returnString;
        }
    }
}
