using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using FoodPlanner.Models;
using System.Windows.Input;
using FoodPlanner.Helper_Classes;
using System.Collections.ObjectModel;

namespace FoodPlanner.ViewModels
{

    public class InventoryViewModel : ObservableObject
    {
        private CollectionViewSource _inventoryIngredientsCollectionViewSource;
        private ICommand _saveInventoryCommand;

        private int _maximumAutoCompleteItems = 10;
        private List<Ingredient> _queriedIngredients;
        private string _searchText;
        private string _lastSearchText = ""; // when we last queried the db
  

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

        public IEnumerable<Ingredient> FoundIngredients
        {
            get
            {
                if (_queriedIngredients == null)
                {
                    _queriedIngredients = new List<Ingredient>();
                }
                return _queriedIngredients.Where(i => i.Name.ToLower().Contains(SearchText.ToLower()));
            }
        }

      
        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                _searchText = value;
                OnPropertyChanged("SearchText");
                OnPropertyChanged("FoundIngredients");
                TryToRepopulateTheList();
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

        // Methods

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

        private void TryToRepopulateTheList()
        {

            // fix this
            if (SearchText == "") {
                _lastSearchText = "";
                _queriedIngredients.Clear();
                OnPropertyChanged("FoundIngredients");
                return;
            }

            // Only query the database if the search string has changed
            // and a continues search string could change the previously fetched items.
            if (_lastSearchText != "" &&
                SearchText.StartsWith(_lastSearchText, StringComparison.OrdinalIgnoreCase) &&
                //  _queriedIngredients.Count != 0 &&
              _queriedIngredients.OfType<object>().Count() < _maximumAutoCompleteItems)
            {
                Console.WriteLine("Just avoided a unnecessary db lookup ;)");
            }
            else
            {
                _lastSearchText = SearchText;
                Console.WriteLine("Fetching data from db! " + DateTime.Now.ToLongTimeString());
                PopulateListWithIngredientsFromDatabase();
            }
        }

        private void PopulateListWithIngredientsFromDatabase()
        {
            string originalSearchText = SearchText;

            var foundIngredientsInDb = MainWindow.db.Ingredients
                .Where(i => i.Name.ToLower().Contains(originalSearchText.ToLower()))
                .Take(_maximumAutoCompleteItems)
                .OrderBy(i => i.Name.ToLower().IndexOf(originalSearchText));

            // Populate the list if the search text has not changed.
            if (originalSearchText == SearchText)
            {
                _queriedIngredients = foundIngredientsInDb.ToList();
                OnPropertyChanged("FoundIngredients");
            }
            else
            {
                Console.WriteLine("Search text changed before repopulation...");
            }

        }


    }
}
