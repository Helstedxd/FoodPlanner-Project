using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using FoodPlanner.Models;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace FoodPlanner.ViewModels
{

    public class InventoryViewModel : ObservableObject
    {

        #region Fields

        private CollectionViewSource _inventoryIngredientsCollectionViewSource;
        private int _maximumAutoCompleteItems = 10;
        private List<Ingredient> _queriedIngredients;
        private Ingredient _selectedItem = null;
        private string _searchText;
        private string _lastSearchText; // when we last queried the db

        private ICommand _saveInventoryCommand, _addToInventory;

        #endregion

        public InventoryViewModel()
        {
            InventoryIngredients = App.CurrentUser.InventoryIngredients;
        }

        #region Properties & Commands

        public ObservableCollection<InventoryIngredient> InventoryIngredients { get; set; }

        public IEnumerable<Ingredient> FoundIngredients
        {
            get
            {
                if (_queriedIngredients == null)
                {
                    _queriedIngredients = new List<Ingredient>();
                }

                OnPropertyChanged("disableAddButton");
                return _queriedIngredients.Where(i => i.Name.ToLower().Contains(SearchText.ToLower()));
            }
        }

        public bool disableAddButton
        {
            get
            {
                if (_queriedIngredients != null)
                {
                    if (_queriedIngredients.Count() != 0)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public Ingredient SetSelectedItem
        {
            set
            {
                _selectedItem = (Ingredient)value;
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

        public ICommand AddToInventory
        {
            get
            {
                if (_addToInventory == null)
                {
                    _addToInventory = new RelayCommand(p => AddItemToInventory());
                }

                return _addToInventory;
            }
        }

        #endregion

        #region Methods

        private void SaveInventory()
        {
            // Remove unlinked items from the database
            /*
            foreach (var inventoryItem in App.db.InventoryIngredients.Local.ToList())
            {
                if (inventoryItem.Ingredient == null)
                {
                    App.db.InventoryIngredients.Remove(inventoryItem);
                }
            }
            */
            App.db.SaveChanges();

            Console.WriteLine("Trying to save");
        }

        private void TryToRepopulateTheList()
        {

            // fix this
            if (SearchText == "")
            {
                _lastSearchText = "";
                _queriedIngredients.Clear();
                OnPropertyChanged("FoundIngredients");
                return;
            }

            // Only query the database if the search string has changed
            // and a continues search string could change the previously fetched items.
            if (_lastSearchText != null && _lastSearchText != "" &&
                SearchText.StartsWith(_lastSearchText, StringComparison.OrdinalIgnoreCase) &&
                //  _queriedIngredients.Count != 0 &&
              _queriedIngredients.Count() < _maximumAutoCompleteItems)
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

            var foundIngredientsInDb = App.db.Ingredients
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

        private void AddItemToInventory()
        {
            if (_selectedItem != null)
            {
                App.CurrentUser.InventoryIngredients.Add(new InventoryIngredient(_selectedItem, 50));
            }
        }

        #endregion

    }
}
