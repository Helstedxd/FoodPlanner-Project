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
        private int _maximumAutoCompleteItems = 10;
        private List<Ingredient> _queriedIngredients;
        private string _searchText;
        private string _lastSearchText; // when we last queried the db

        private ICommand _saveInventoryCommand;
        private ICommand _addIngredientToInventory;

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

                // OnPropertyChanged("disableAddButton"); //TODO: fix
                return _queriedIngredients.Where(i => i.Name.ToLower().Contains(SearchText.ToLower()));
            }
        }

        public Visibility AutoCompleteListVisibility
        {
            get
            {
                if (FoundIngredients.Count() > 0)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }

        public Ingredient SelectedIngredient { get; set; }

        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                _searchText = value;
                RaisePropertyChanged("SearchText");
                RaisePropertyChanged("FoundIngredients");
                RaisePropertyChanged("AutoCompleteListVisibility");
                TryToRepopulateTheList();
            }
        }

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
                    _addIngredientToInventory = new RelayCommand(() => AddIngredientToInventory(SelectedIngredient));
                }
                return _addIngredientToInventory;
            }
        }

        #endregion

        #region Methods

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

        private void TryToRepopulateTheList()
        {
            // fix this
            if (SearchText == "")
            {
                _lastSearchText = "";
                _queriedIngredients.Clear();
                RaisePropertyChanged("FoundIngredients");
                RaisePropertyChanged("AutoCompleteListVisibility");
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
                RaisePropertyChanged("FoundIngredients");
                RaisePropertyChanged("AutoCompleteListVisibility");
            }
            else
            {
                Console.WriteLine("Search text changed before repopulation...");
            }

        }

        private void AddIngredientToInventory(Ingredient ingredient)
        {
            if (ingredient != null)
            {
                App.CurrentUser.InventoryIngredients.Add(new InventoryIngredient(ingredient, 1));
            }
            SearchText = "";
        }

        #endregion

    }
}
