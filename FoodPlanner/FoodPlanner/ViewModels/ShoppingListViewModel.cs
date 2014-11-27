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

            var MealRecipeIngredientsTotalQuantity = from ri in App.db.RecipeIngredients
                                                     where App.db.Meals.Any(m => m.UserID == App.CurrentUser.ID && m.RecipeID == ri.RecipeID)
                                                     group ri by ri.IngredientID into rig
                                                     select new
                                                     {
                                                         IngredientID = rig.FirstOrDefault().IngredientID,
                                                         Ingredient = rig.FirstOrDefault().Ingredient,
                                                         Unit = rig.FirstOrDefault().Ingredient.Unit,
                                                         TotalQuantity = rig.Sum(i => i.Quantity),
                                                     };

            var InventoryMealIngredientQuantityDifferences = from mritq in MealRecipeIngredientsTotalQuantity
                                                             join iitq in publicQuery.inventoryIQueryable on mritq.IngredientID equals iitq.IngredientID into j
                                                             from iitqOrNull in j.DefaultIfEmpty()
                                                             select new
                                                             {
                                                                 Ingredient = mritq.Ingredient,
                                                                 InventoryQuantityDifference = iitqOrNull != null ? mritq.TotalQuantity - iitqOrNull.Quantity : mritq.TotalQuantity,
                                                                 Unit = mritq.Ingredient.Unit
                                                             };

            foreach (var IngredientDifference in InventoryMealIngredientQuantityDifferences)
            {
                if (IngredientDifference.InventoryQuantityDifference > 0)
                {
                    // TODO: the purchase- and expiration date will have to be updated if we add this item to the inventory
                    ShoppingListIngredient newShoppingListIngredient = new ShoppingListIngredient(IngredientDifference.Ingredient, IngredientDifference.InventoryQuantityDifference);
                    ShoppingList.Add(newShoppingListIngredient);
                }
            }
        }

        #endregion

    }

}