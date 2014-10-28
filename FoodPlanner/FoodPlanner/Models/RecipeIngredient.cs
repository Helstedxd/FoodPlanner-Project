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
            if (this.Quantity == 0)//Så der ikke står " g salt" men bare "salt"
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
