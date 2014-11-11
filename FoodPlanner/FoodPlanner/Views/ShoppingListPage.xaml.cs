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

namespace FoodPlanner.Views
{
    /// <summary>
    /// Interaction logic for ShoppingListPage.xaml
    /// </summary>
    public partial class ShoppingListPage : Page
    {

        /*public static FoodContext db;
        public static User CurrentUser { get; set; }
        public ObservableCollection<RecipeIngredient> ShoppingList = new ObservableCollection<RecipeIngredient>();*/

        public ShoppingListPage()
        {

            InitializeComponent();
            //this.DataContext = ShoppingList;

            var MealRecipeIngredientsTotalQuantity =
                from ri in App.db.RecipeIngredients
                where App.db.Meals.Any(m => m.UserID == App.CurrentUser.ID && m.RecipeID == ri.RecipeID)
                group ri by ri.IngredientID into rig
                select new
                {
                    IngredientID = rig.FirstOrDefault().IngredientID,
                    TotalQuantity = rig.Sum(i => i.Quantity)
                };

            var gideonblegmand = MealRecipeIngredientsTotalQuantity.ToList();


            /* db = new FoodContext();
             CurrentUser = db.Users.First();

             CreateShoppingList();*/
        }

        /*private void CreateShoppingList() {
            List<InventoryIngredient> inventoryIngrdients = db.InventoryIngredients.Where(m => m.UserID == CurrentUser.ID).ToList();

            List<RecipeIngredient> recipeIngredients = db.RecipeIngredients.Where(ri => db.Meals.Where(m => m.UserID == CurrentUser.ID).Any(m => m.RecipeID == ri.RecipeID)).ToList();

            foreach (RecipeIngredient ri in recipeIngredients) {
                MessageBox.Show(ri.Quantity.ToString());
            }

            //Tilføj elementer til shopping list med fælgende kriterie
            //RecipeIngrediensen er ikke i Inventory, recipeingrediensen har fratrukket hvad der er i Inventory
            foreach (InventoryIngredient invenIn in inventoryIngrdients) {
            foreach (RecipeIngredient ri in recipeIngredients) {

                    if (invenIn.ID != ri.ID) {
                        ShoppingList.Add(ri);
                    }
                }
            }

            foreach (RecipeIngredient ri in ShoppingList) {
                MessageBox.Show(ri.Ingredient.Name);
            }*/

        //}
    }
}
