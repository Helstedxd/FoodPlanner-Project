using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using FoodPlanner.Models;
using System.Windows.Input;
using System.Collections.ObjectModel;
using MvvmFoundation.Wpf;

namespace FoodPlanner.ViewModels
{

    public class InventoryViewModel : ObservableObject
    {

        #region Fields

        private ICommand _saveInventoryCommand;
        private ICommand _addIngredientToInventory;

        #endregion

        public InventoryViewModel()
        {
            InventoryIngredients = App.CurrentUser.InventoryIngredients;
        }

        #region Properties

        public ObservableCollection<InventoryIngredient> InventoryIngredients { get; set; }

        #endregion

        #region Commands

        public ICommand SaveInventoryCommand
        {
            get
            {
                if (_saveInventoryCommand == null)
                {
                    _saveInventoryCommand = new RelayCommand(() => SaveInventory());
                }
                return _saveInventoryCommand;
            }
        }

        public ICommand AddIngredientToInventoryCommand
        {
            get
            {
                if (_addIngredientToInventory == null)
                {
                    _addIngredientToInventory = new RelayCommand<Ingredient>(i => AddIngredientToInventory(i));
                }
                return _addIngredientToInventory;
            }
        }

        #endregion

        #region Methods

        private void AddIngredientToInventory(Ingredient ingredient)
        {
            if (ingredient != null)
            {
                App.CurrentUser.InventoryIngredients.Add(new InventoryIngredient(ingredient, 1));
            }
        }

        private void SaveInventory()
        {
            //TODO: stuff (Inventory is not saved if you leave the page..)
            List<InventoryIngredient> currentUserInventory = App.CurrentUser.InventoryIngredients.ToList();

            foreach (InventoryIngredient ii in App.db.InventoryIngredients.Where(ii => ii.UserID == App.CurrentUser.ID))
            {
                if (currentUserInventory.Where(ci => ci.ID == ii.ID).Count() == 0)
                {
                    App.db.InventoryIngredients.Remove(ii);
                }
            }

            App.db.SaveChanges();
        }

        #endregion

    }
}
