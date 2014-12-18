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
using MvvmFoundation.Wpf;
using System.ComponentModel;

namespace FoodPlanner.ViewModels
{

    public class ShoppingListViewModel : ObservableObject
    {

        #region Fields

        private bool _checkAllChecked;
        private ICommand _addCheckedToInventory;

        #endregion

        public ShoppingListViewModel()
        {
            //initialize shoppinglist
            ShoppingList = new ObservableCollection<ShoppingListIngredient>();
            //run method to generate the shoppinglist
            AssembleShoppingList();

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ShoppingList);

            // Sort Description
            SortDescription SortByName = new SortDescription("InventoryIngredient.Ingredient.Name", ListSortDirection.Ascending);

            //Sort by Descriptions
            view.SortDescriptions.Add(SortByName);
        }

        #region Properties & Commands

        public ObservableCollection<ShoppingListIngredient> ShoppingList { get; set; }

        public bool CheckAllChecked
        {
            get { return _checkAllChecked; }
            set
            {
                _checkAllChecked = value;
                foreach (ShoppingListIngredient shoppingItem in ShoppingList)
                {
                    shoppingItem.Checked = value;
                }
            }
        }



        public ICommand AddCheckedToInventoryCommand
        {
            get
            {
                if (_addCheckedToInventory == null)
                {
                    _addCheckedToInventory = new RelayCommand(() => AddCheckedToInventory());
                }
                return _addCheckedToInventory;
            }
        }

        #endregion

        #region Methods

        private void AddCheckedToInventory()
        {
            //Adding the checked ingredient to the inventory
            foreach (ShoppingListIngredient shoppingItem in ShoppingList.ToList())
            {
                if (shoppingItem.Checked == true)
                {
                    //Add to inventory
                    App.CurrentUser.InventoryIngredients.Add(shoppingItem.InventoryIngredient);
                    //Remove from shoppinglist
                    ShoppingList.Remove(shoppingItem);
                }
            }
            //SaveChanges in the database
            App.db.SaveChanges();
        }

        private void AssembleShoppingList()
        {
            //initialize publicQuerys and number of days to shop ahead.
            PublicQuerys publicQuery = new PublicQuerys();
            DateTime dateToShopAhead = DateTime.Now.AddDays(App.CurrentUser.ShopAhead);

            //Load what the user needs to allways have in the inventory, and amount
            List<ShoppingClass> stockQuantities = (from s in App.db.StockQuantities
                                                   join i in App.db.Ingredients on s.IngredientID equals i.ID
                                                   where s.UserID == App.CurrentUser.ID
                                                   select new ShoppingClass()
                                                   {
                                                       Ingredient = i,
                                                       TotalQuantity = s.Quantity
                                                   }).ToList();

            //initialize the MealRecipeIngredientsTotalQuantity
            List<ShoppingClass> MealRecipeIngredientsTotalQuantity = new List<ShoppingClass>();

            //Traverse through comming meals
            foreach (Meal m in App.db.Meals.Where(m => m.UserID == App.CurrentUser.ID && m.Date <= dateToShopAhead && m.IsActive).ToList())
            {
                //Traverse through the ingredients, in the meal
                foreach (RecipeIngredient i in m.Recipe.RecipeIngredients)
                {
                    //check if the ingredient is already loaded into MealRecipeIngredientsTotalQuantity, if it is add the amount to the 
                    if (MealRecipeIngredientsTotalQuantity.Where(mritq => mritq.Ingredient == i.Ingredient).Count() == 0)
                    {
                        //Add ingredient to MealRecipeIngredientsTotalQuantity
                        MealRecipeIngredientsTotalQuantity.Add(new ShoppingClass() { Ingredient = i.Ingredient, TotalQuantity = Math.Round(i.Quantity * ((decimal)m.Participants / (decimal)i.Recipe.Persons), 2) });
                    }
                    else
                    {
                        //Increment ingredient in MealRecipeIngredientsTotalQuantity
                        MealRecipeIngredientsTotalQuantity.Where(mritq => mritq.Ingredient == i.Ingredient).FirstOrDefault().TotalQuantity += Math.Round(i.Quantity * ((decimal)m.Participants / (decimal)i.Recipe.Persons), 2);
                    }
                }
            }

            //Load the users inventory
            List<ShoppingClass> userInventory = (from il in publicQuery.inventoryList
                                                 select new ShoppingClass()
                                                 {
                                                     Ingredient = il.Ingredient,
                                                     TotalQuantity = il.Quantity
                                                 }).ToList();

            //combine the list from meals and the stock quantities, and group by ingredient and combine the quantity
            List<ShoppingClass> groupedToFindTotalQuantity = stockQuantities.Concat(MealRecipeIngredientsTotalQuantity)
                                                             .GroupBy(sc => sc.Ingredient)
                                                             .Select(sc => new ShoppingClass()
                                                             {
                                                                 Ingredient = sc.FirstOrDefault().Ingredient,
                                                                 TotalQuantity = sc.Count() == 1 ? sc.First().TotalQuantity : ((sc.First().TotalQuantity - sc.Last().TotalQuantity) > 0 ? sc.First().TotalQuantity : (sc.First().TotalQuantity + (sc.Last().TotalQuantity - sc.First().TotalQuantity)))
                                                             }).ToList();


            foreach (ShoppingClass sc in groupedToFindTotalQuantity)
            {
                //initialize the ShoppingListIngredient that is to be added
                ShoppingListIngredient newShoppingListIngredient = null;

                //checks if the users has the ingredient in the inventory, if the user has, subtract the amount the user has from the needed amount.
                if (userInventory.Where(ui => ui.Ingredient == sc.Ingredient).Count() != 0)
                {
                    //subract the amount
                    if (userInventory.Where(t => t.Ingredient == sc.Ingredient).Single().TotalQuantity > sc.TotalQuantity && (sc.TotalQuantity - userInventory.Where(t => t.Ingredient == sc.Ingredient).Single().TotalQuantity) >= 0)
                    {
                        InventoryIngredient newInventoryIngredient = new InventoryIngredient(sc.Ingredient, (sc.TotalQuantity - userInventory.Where(t => t.Ingredient == sc.Ingredient).Single().TotalQuantity));
                        newShoppingListIngredient = new ShoppingListIngredient(newInventoryIngredient);
                    }
                    //if the user has enought of the item dont add to shoppinglist.
                }
                else
                {
                    InventoryIngredient newInventoryIngredient = new InventoryIngredient(sc.Ingredient, sc.TotalQuantity);
                    newShoppingListIngredient = new ShoppingListIngredient(newInventoryIngredient);
                }

                if (newShoppingListIngredient != null)
                {
                    ShoppingList.Add(newShoppingListIngredient);
                }
            }
        }

        #endregion

    }

}