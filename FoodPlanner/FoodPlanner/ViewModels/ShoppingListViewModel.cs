using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using FoodPlanner.Models;

namespace FoodPlanner.ViewModels
{

    public class ShoppingListViewModel
    {

        public List<InventoryIngredient> ShoppingList { get; set; }

        public ShoppingListViewModel()
        {

            ShoppingList = new List<InventoryIngredient>();

            AssembleShoppingList();

        }

        private void AssembleShoppingList()
        {
            //TODO: somehow store these common queries...
            var InventoryIngredientsTotalQuantity =
                from ii in App.db.InventoryIngredients
                join i in App.db.Ingredients on ii.IngredientID equals i.ID
                where ii.UserID == App.CurrentUser.ID
                group ii by ii.IngredientID into iig
                select new
                {
                    IngredientID = iig.FirstOrDefault().IngredientID,
                    Unit = iig.FirstOrDefault().Ingredient.Unit,
                    TotalQuantity = iig.Sum(i => i.Quantity)
                };

            var MealRecipeIngredientsTotalQuantity =
                from ri in App.db.RecipeIngredients
                where App.db.Meals.Any(m => m.UserID == App.CurrentUser.ID && m.RecipeID == ri.RecipeID)
                group ri by ri.IngredientID into rig
                select new
                {
                    IngredientID = rig.FirstOrDefault().IngredientID,
                    Ingredient = rig.FirstOrDefault().Ingredient,
                    Unit = rig.FirstOrDefault().Ingredient.Unit,
                    TotalQuantity = rig.Sum(i => i.Quantity),
                };

            var InventoryMealIngredientQuantityDifferences =
                from mritq in MealRecipeIngredientsTotalQuantity
                join iitq in InventoryIngredientsTotalQuantity on mritq.IngredientID equals iitq.IngredientID into j
                from iitqOrNull in j.DefaultIfEmpty()
                select new
                {
                    Ingredient = mritq.Ingredient,
                    InventoryQuantityDifference = iitqOrNull != null ? mritq.TotalQuantity - iitqOrNull.TotalQuantity : mritq.TotalQuantity,
                    Unit = mritq.Ingredient.Unit
                };

            foreach (var IngredientDifference in InventoryMealIngredientQuantityDifferences)
            {
                if (IngredientDifference.InventoryQuantityDifference > 0)
                {
                    // TODO: the purchase- and expiration date will have to be updated if we add this item to the inventory
                    InventoryIngredient newShoppingListIngredient = new InventoryIngredient(IngredientDifference.Ingredient, IngredientDifference.InventoryQuantityDifference);
                    ShoppingList.Add(newShoppingListIngredient);
                }
            }


        }
    }

}
