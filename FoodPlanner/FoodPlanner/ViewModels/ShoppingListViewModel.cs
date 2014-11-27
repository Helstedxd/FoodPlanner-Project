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
                    //TODO: could we avoid this conversion
                    InventoryIngredient ii = new InventoryIngredient(shoppingItem);
                    App.CurrentUser.InventoryIngredients.Add(ii);
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

            List<ShoppingClass> MealRecipeIngredientsTotalQuantity = (from ri in App.db.RecipeIngredients
                                                                      where App.db.Meals.Any(m => m.UserID == App.CurrentUser.ID && m.RecipeID == ri.RecipeID && m.IsActive && m.Date <= dateToShopAhead)
                                                                      group ri by ri.IngredientID into rig
                                                                      select new ShoppingClass()
                                                                      {
                                                                          Ingredient = rig.FirstOrDefault().Ingredient,
                                                                          TotalQuantity = rig.Sum(i => i.Quantity),
                                                                      }).ToList();

            List<ShoppingClass> test = (from il in publicQuery.inventoryList
                                        select new ShoppingClass()
                                        {
                                            Ingredient = il.Ingredient,
                                            TotalQuantity = il.Quantity
                                        }).ToList();

            List<ShoppingClass> tedst = stockQuantities.Concat(MealRecipeIngredientsTotalQuantity)
                       .GroupBy(sc => sc.Ingredient)
                       .Select(sc => new ShoppingClass()
                       {
                           Ingredient = sc.FirstOrDefault().Ingredient,
                           TotalQuantity = sc.Count() == 1 ? sc.First().TotalQuantity : ((sc.First().TotalQuantity - sc.Last().TotalQuantity) > 0 ? sc.First().TotalQuantity : (sc.First().TotalQuantity + (sc.Last().TotalQuantity - sc.First().TotalQuantity)))
                       }).ToList();

            foreach (ShoppingClass sc in tedst)
            {
                ShoppingListIngredient newShoppingListIngredient = new ShoppingListIngredient(sc.Ingredient, sc.TotalQuantity);
                ShoppingList.Add(newShoppingListIngredient);
            }

            /*
            foreach (ShoppingClass sq in stockQuantities.ToList())
            {
                foreach (ShoppingClass mritq in MealRecipeIngredientsTotalQuantity.ToList())
                {
                    if (sq.Ingredient.ID == mritq.Ingredient.ID)
                    {
                        ShoppingListIngredient newShoppingListIngredient = new ShoppingListIngredient(sq.Ingredient, (sq.TotalQuantity - mritq.TotalQuantity) > 0 ? sq.TotalQuantity : (sq.TotalQuantity + (mritq.TotalQuantity - sq.TotalQuantity)));
                        ShoppingList.Add(newShoppingListIngredient);
                    }
                    else
                    {
                        if (ShoppingList.Where(sl => sl.Ingredient.ID == sq.Ingredient.ID).Count() == 0)
                        {
                            ShoppingListIngredient newShoppingListIngredient = new ShoppingListIngredient(sq.Ingredient, sq.TotalQuantity);
                            ShoppingList.Add(newShoppingListIngredient);
                        }
                        if (ShoppingList.Where(sl => sl.Ingredient.ID == mritq.Ingredient.ID).Count() == 0)
                        {
                            ShoppingListIngredient newShoppingListIngredient = new ShoppingListIngredient(mritq.Ingredient, mritq.TotalQuantity);
                            ShoppingList.Add(newShoppingListIngredient);
                        }
                    }
                }
            }
            */
        }

        #endregion

    }

}