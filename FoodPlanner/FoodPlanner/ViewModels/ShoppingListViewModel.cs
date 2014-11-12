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

namespace FoodPlanner.ViewModels {

    public class ShoppingListViewModel {

        public List<InventoryIngredient> ShoppingList { get; set; }

        public ShoppingListViewModel() {

            AssembleShoppingList();
            
        }

        private void AssembleShoppingList() {
            var InventoryIngredientsTotalQuantity =
                from ii in App.db.InventoryIngredients
                join i in App.db.Ingredients on ii.IngredientID equals i.ID
                where ii.UserID == App.CurrentUser.ID
                group ii by ii.IngredientID into iig
                select new {
                    IngredientID = iig.FirstOrDefault().IngredientID,
                    Ingredient = iig.FirstOrDefault().Ingredient,
                    Unit = iig.FirstOrDefault().Ingredient.Unit,
                    TotalQuantity = iig.Sum(i => i.Quantity)
                };

    
            var MealRecipeIngredientsTotalQuantity =
                from ri in App.db.RecipeIngredients
                where App.db.Meals.Any(m => m.UserID == App.CurrentUser.ID && m.RecipeID == ri.RecipeID)
                group ri by ri.IngredientID into rig
                select new {
                    IngredientID = rig.FirstOrDefault().IngredientID,
                    Unit = rig.FirstOrDefault().Ingredient.Unit,
                    TotalQuantity = rig.Sum(i => i.Quantity),
                };

        /*    var gideonblegmand =
                from iitq in InventoryIngredientsTotalQuantity
                join mritq in MealRecipeIngredientsTotalQuantity on iitq.IngredientID equals mritq.IngredientID
                select new { idd = iitq.IngredientID, iiq = iitq.TotalQuantity, mriq = mritq.TotalQuantity };

            var gideonb = gideonblegmand.ToList();*/

            var InventoryIngredientsTotalQuantityList = InventoryIngredientsTotalQuantity.ToList();

            var MealRecipeIngredientsTotalQuantityList = MealRecipeIngredientsTotalQuantity.ToList();

            List<ShoppingList> ShoppingList = new List<ShoppingList>();

            MessageBox.Show(MealRecipeIngredientsTotalQuantityList.Count().ToString());
            MessageBox.Show(InventoryIngredientsTotalQuantityList.Count().ToString());

            foreach (var ii in InventoryIngredientsTotalQuantityList) {
                foreach(var mri in MealRecipeIngredientsTotalQuantityList) {
                    if (mri.IngredientID == ii.IngredientID /*&& mri.TotalQuantity - ii.TotalQuantity > 0*/) {

                        //Ingredient newIngredient = new Ingredient();

                        ShoppingList newInventoryIngredient = new ShoppingList() { ingredient = ii.Ingredient, quantity = (mri.TotalQuantity - ii.TotalQuantity) };

                        ShoppingList.Add(newInventoryIngredient);
                    }
                }
            }

        }
    }

    class ShoppingList {
        public Ingredient ingredient {
            get;
            set;
        }

        public decimal quantity {
            get;
            set;
        }

        public ShoppingList() { }
    }
}
