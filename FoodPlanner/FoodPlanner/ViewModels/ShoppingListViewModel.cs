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

        public FoodContext db;
        public static User CurrentUser { get; set; }
        public ObservableCollection<RecipeIngredient> ShoppingListCollection { get; set; }

        public ShoppingListViewModel() {

            db = new FoodContext();
            CurrentUser = db.Users.First();
            CreateShoppingList();
            
        }

        private void CreateShoppingList() {
            List<InventoryIngredient> inventoryIngrdients = db.InventoryIngredients.Where(m => m.UserID == CurrentUser.ID).ToList();

            List<RecipeIngredient> recipeIngredients = db.RecipeIngredients.Where(ri => db.Meals.Where(m => m.UserID == CurrentUser.ID).Any(m => m.RecipeID == ri.RecipeID)).ToList();

            ShoppingListCollection = new ObservableCollection<RecipeIngredient>();

            //Tilføj elementer til shopping list med fælgende kriterie
            //RecipeIngrediensen er ikke i Inventory, recipeingrediensen har fratrukket hvad der er i Inventory
            foreach (InventoryIngredient invenIn in inventoryIngrdients) {
            foreach (RecipeIngredient ri in recipeIngredients) {

                    if (invenIn.ID != ri.ID) {
                        ShoppingListCollection.Add(ri);
                    }
                }
            }

        }
    }
}
