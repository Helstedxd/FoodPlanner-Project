using MvvmFoundation.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoodPlanner.Models
{
    public partial class InventoryIngredient : ObservableObject
    {

        public InventoryIngredient() { }

        public InventoryIngredient(Ingredient ingredient, decimal quantity)
        {
            this.Ingredient = ingredient;
            this.Quantity = quantity;
            // TODO: These dates should probably be chosen in a more clever way.
            this.PurchaseDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now.AddDays(7);
        }

        public InventoryIngredient(InventoryIngredient ii)
        {
            this.ID = ii.ID;
            this.UserID = ii.UserID;
            this.IngredientID = ii.IngredientID;
            this.User = ii.User;
            this.ExpirationDate = ii.ExpirationDate;
            this.Ingredient = ii.Ingredient;
            this.PurchaseDate = ii.PurchaseDate;
            this.Quantity = ii.Quantity;
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
