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
            ShoppingList = new ObservableCollection<ShoppingListIngredient>();
            AssembleShoppingList();

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ShoppingList);

            // Sort Description
            SortDescription SortByName = new SortDescription("InventoryIngredient.Ingredient.Name", ListSortDirection.Ascending);

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
            // ShoppingList.Where(i => i.Checked == true)
            foreach (ShoppingListIngredient shoppingItem in ShoppingList.ToList())
            {
                if (shoppingItem.Checked == true)
                {
                    App.CurrentUser.InventoryIngredients.Add(shoppingItem.InventoryIngredient);
                    ShoppingList.Remove(shoppingItem);
                }
            }

            App.db.SaveChanges();
        }

        private void AssembleShoppingList()
        {
            //TODO: only consider within the shopAhead period (and maybe exclude days in the past)
            //TODO: somehow store these common queries...
            PublicQuerys publicQuery = new PublicQuerys();
            DateTime dateToShopAhead = DateTime.Now.AddDays(App.CurrentUser.ShopAhead);

            List<ShoppingClass> stockQuantities = (from s in App.db.StockQuantities
                                                   join i in App.db.Ingredients on s.IngredientID equals i.ID
                                                   where s.UserID == App.CurrentUser.ID
                                                   select new ShoppingClass()
                                                   {
                                                       Ingredient = i,
                                                       TotalQuantity = s.Quantity
                                                   }).ToList();

            List<ShoppingClass> MealRecipeIngredientsTotalQuantity = new List<ShoppingClass>();

            foreach (Meal m in App.db.Meals.Where(m => m.UserID == App.CurrentUser.ID && m.Date <= dateToShopAhead && m.IsActive).ToList())
            {
                foreach (RecipeIngredient i in m.Recipe.RecipeIngredients)
                {
                    if (MealRecipeIngredientsTotalQuantity.Where(mritq => mritq.Ingredient == i.Ingredient).Count() == 0)
                    {
                        MealRecipeIngredientsTotalQuantity.Add(new ShoppingClass() { Ingredient = i.Ingredient, TotalQuantity = Math.Round(i.Quantity * ((decimal)m.Participants / (decimal)i.Recipe.Persons), 2) });
                    }
                    else
                    {
                        MealRecipeIngredientsTotalQuantity.Where(mritq => mritq.Ingredient == i.Ingredient).FirstOrDefault().TotalQuantity += Math.Round(i.Quantity * ((decimal)m.Participants / (decimal)i.Recipe.Persons), 2);
                    }
                }
            }

            List<ShoppingClass> userInventory = (from il in publicQuery.inventoryList
                                                 select new ShoppingClass()
                                                 {
                                                     Ingredient = il.Ingredient,
                                                     TotalQuantity = il.Quantity
                                                 }).ToList();

            List<ShoppingClass> groupedToFindTotalQuantity = stockQuantities.Concat(MealRecipeIngredientsTotalQuantity)
                                       .GroupBy(sc => sc.Ingredient)
                                       .Select(sc => new ShoppingClass()
                                       {
                                           Ingredient = sc.FirstOrDefault().Ingredient,
                                           TotalQuantity = sc.Count() == 1 ? sc.First().TotalQuantity : ((sc.First().TotalQuantity - sc.Last().TotalQuantity) > 0 ? sc.First().TotalQuantity : (sc.First().TotalQuantity + (sc.Last().TotalQuantity - sc.First().TotalQuantity)))
                                       }).ToList();

            foreach (ShoppingClass sc in groupedToFindTotalQuantity)
            {
                ShoppingListIngredient newShoppingListIngredient = null;

                if (userInventory.Where(ui => ui.Ingredient == sc.Ingredient).Count() != 0)
                {
                    if (userInventory.Where(t => t.Ingredient == sc.Ingredient).Single().TotalQuantity < sc.TotalQuantity)
                    {
                        InventoryIngredient newInventoryIngredient = new InventoryIngredient(sc.Ingredient, (sc.TotalQuantity - userInventory.Where(t => t.Ingredient == sc.Ingredient).Single().TotalQuantity));
                        newShoppingListIngredient = new ShoppingListIngredient(newInventoryIngredient);
                    }
                }
                else
                {
                    Console.WriteLine(sc.Ingredient.Name);
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