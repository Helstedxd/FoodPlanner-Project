using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner
{
    public partial class RecipeIngredient
    {
        public override string ToString()
        {
            if (!Convert.ToBoolean(this.Quantity))
            {
                return Convert.ToString(this.Ingredient.Name);
            }
            else
            {
                return Convert.ToString(this.Quantity + this.Ingredient.Unit + " " + this.Ingredient.Name);
            }
        }
    }
}
