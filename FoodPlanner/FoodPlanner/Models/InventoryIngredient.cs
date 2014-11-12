using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner.Models
{
    public partial class InventoryIngredient
    {

        public InventoryIngredient() { }

        public InventoryIngredient(Ingredient ingredient, decimal quantity)
        {
            this.Ingredient = ingredient;
            this.Quantity = quantity;
            // TODO: These dates should probably be chosen in a more clever way.
            this.PurchaseDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
        }

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
