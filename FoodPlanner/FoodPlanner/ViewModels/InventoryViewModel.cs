using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using FoodPlanner.Models;
using System.Windows.Input;
using FoodPlanner.Helper_Classes;

namespace FoodPlanner.ViewModels
{

    public class InventoryViewModel
    {
        private CollectionViewSource _inventoryIngredientsCollectionViewSource;
        private ICommand _saveInventoryCommand;

        public InventoryViewModel()
        {
            InventoryIngredientsCollectionViewSource.Source = MainWindow.CurrentUser.InventoryIngredients;
        }

        public CollectionViewSource InventoryIngredientsCollectionViewSource
        {
            get
            {
                if (_inventoryIngredientsCollectionViewSource == null)
                {
                    _inventoryIngredientsCollectionViewSource = new CollectionViewSource();
                }
                return _inventoryIngredientsCollectionViewSource;
            }
        }

        public ICommand SaveInventoryCommand
        {
            get
            {
                if (_saveInventoryCommand == null)
                {
                    _saveInventoryCommand = new RelayCommand(p => SaveInventory());
                }

                return _saveInventoryCommand;
            }
        }

        private void SaveInventory()
        {
            // Remove unlinked items from the database
            foreach (var inventoryItem in MainWindow.db.InventoryIngredients.Local.ToList())
            {
                if (inventoryItem.Ingredient == null)
                {
                    MainWindow.db.InventoryIngredients.Remove(inventoryItem);
                }
            }

            MainWindow.db.SaveChanges();

            // Refresh the grids so the database generated values show up. 
            //this.inventoryIngredientsDataGrid.Items.Refresh();
            Console.WriteLine("Inventory saved!");
        }

    }
}
