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

namespace FoodPlanner.ViewModels {
    class SettingsViewModel : ObservableObject {
        #region Fields

        private int _maximumAutoCompleteItems = 10; //TODO: this might not be the right place.

        public User CurrentUser { get; set; }
        private List<Ingredient> _queriedIngredients;
        private string _searchText;
        private string _lastSearchText;
        
        #endregion
        
        public SettingsViewModel() {
            CurrentUser = App.CurrentUser;
        }

        #region Properties

        public IEnumerable<Ingredient> FoundIngredients {
            get {
                if (_queriedIngredients == null) {
                    _queriedIngredients = new List<Ingredient>();
                }

                // OnPropertyChanged("disableAddButton"); //TODO: fix
                return _queriedIngredients.Where(i => i.Name.ToLower().Contains(SearchText.ToLower()));
            }
        }

        public Visibility AutoCompleteListVisibility {
            get {
                if (FoundIngredients.Count() > 0) {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }

        public Ingredient SelectedIngredient { get; set; }

        public string SearchText {
            get {
                return _searchText;
            }
            set {
                _searchText = value;
                RaisePropertyChanged("SearchText");
                RaisePropertyChanged("FoundIngredients");
                RaisePropertyChanged("AutoCompleteListVisibility");
                TryToRepopulateTheList();
            }
        }

        #endregion

        #region Methods

        private void TryToRepopulateTheList() {
            // Only query the database if the search string has changed
            // and a continues search string could change the previously fetched items.
            if (string.IsNullOrEmpty(_lastSearchText) ||
                !SearchText.StartsWith(_lastSearchText, StringComparison.OrdinalIgnoreCase) ||
                _queriedIngredients.Count() >= _maximumAutoCompleteItems) {
                _lastSearchText = SearchText;
                if (SearchText == "") {
                    _queriedIngredients.Clear();
                    RaisePropertyChanged("FoundIngredients");
                    RaisePropertyChanged("AutoCompleteListVisibility");
                } else {
                    Console.WriteLine("Fetching data from db! " + DateTime.Now.ToLongTimeString());
                    PopulateListWithIngredientsFromDatabase();
                }
            } else {
                Console.WriteLine("Avoided an unnecessary db lookup");
            }
        }

        private void PopulateListWithIngredientsFromDatabase() {
            //TODO: this function should run asynchronously - and not block user interaction.
            string originalSearchText = SearchText;

            var foundIngredientsInDb = App.db.Ingredients
                .Where(i => i.Name.ToLower().Contains(originalSearchText.ToLower()))
                .Take(_maximumAutoCompleteItems)
                .OrderBy(i => i.Name.ToLower().IndexOf(originalSearchText));

            // Populate the list if the search text has not changed.
            if (originalSearchText == SearchText) {
                _queriedIngredients = foundIngredientsInDb.ToList();
                RaisePropertyChanged("FoundIngredients");
                RaisePropertyChanged("AutoCompleteListVisibility");
            } else {
                Console.WriteLine("Search text changed before repopulation...");
            }
        }

        #endregion
    }
}
