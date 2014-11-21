using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.ComponentModel;
using FoodPlanner.Models;
using System.Windows.Input;
using System.Collections.ObjectModel;
using MvvmFoundation.Wpf;

namespace FoodPlanner.ViewModels {
    class SettingsViewModel : ObservableObject {
        #region Fields

        public BlacklistIngredient SelectedBlackListIngredient { get; set; }
        private ICommand _saveListItemCommand;
        private ICommand _addIngredientToUnwantedIngredientsCommand;
        private ICommand _removeingredientFromUnwantedIngredientsCommand;       
        private User _currentUser;
        public User CurrentUser {
            get { return _currentUser; }
            set {
                _currentUser = value;
                RaisePropertyChanged("CurrentUser");
            }
        }
        private InventoryIngredient _inventoryIngredient;
        public InventoryIngredient InventoryIngredient {
            get { return _inventoryIngredient; }
            set {
                _inventoryIngredient = value;
                RaisePropertyChanged("InventoryIngredient");
            }
        }

        #endregion
        
        public SettingsViewModel() {
            InventoryIngredient = new InventoryIngredient();
            CurrentUser = App.CurrentUser;
            SelectedBlackListIngredient = new BlacklistIngredient();
        }

        #region Properties

        #endregion

        #region ICommands
        public ICommand SaveListItemCommand {
            get {
                if (_saveListItemCommand == null) {
                    _saveListItemCommand = new RelayCommand<Ingredient>(i => SaveNewStockIngredientName(i));
                }

                return _saveListItemCommand;
            }
        }

        private void SaveNewStockIngredientName(Ingredient i) {
            InventoryIngredient.ID = i.ID;

        }

        public ICommand AddIngredientToUnwantedIngredientsCommand {
            get {
                if (_addIngredientToUnwantedIngredientsCommand == null) {
                    _addIngredientToUnwantedIngredientsCommand = new RelayCommand<Ingredient>(i => AddIngredientToUnwantedIngredients(i));
                }

                return _addIngredientToUnwantedIngredientsCommand;

            }
        }

        public ICommand RemoveIngredientFromUnwantedIngredientsCommand {
            get {
                if (_removeingredientFromUnwantedIngredientsCommand == null) {
                    _removeingredientFromUnwantedIngredientsCommand = new RelayCommand(() => RemoveIngredientFromUnwantedIngredients());
                }
                return _removeingredientFromUnwantedIngredientsCommand;
            }
        }

        #endregion

        #region Methods

        private void SaveChosenListItemFromAutoCompleteList(Ingredient ingredient) {
            InventoryIngredient.Ingredient.ID = ingredient.ID;
        }

        //Fix: Det skal sikres at man ikke kan tilføje den samme ingrediens flere gange.
        private void AddIngredientToUnwantedIngredients(Ingredient ingredient) {
            BlacklistIngredient IngredientToBeAdded = new BlacklistIngredient() { IngredientID = ingredient.ID, UserID = App.CurrentUser.ID};
            App.db.BlacklistIngredients.Add(IngredientToBeAdded);
            App.db.SaveChanges();
        }

        //Fix: Metoden skal også sikre mod at der er valgt et element i listboxen.
        private void RemoveIngredientFromUnwantedIngredients() {
            App.db.BlacklistIngredients.RemoveRange(App.db.BlacklistIngredients.Where(bli => bli.Id == SelectedBlackListIngredient.Id && bli.UserID == App.CurrentUser.ID));
            App.db.SaveChanges();
        }
            
        #endregion
    }
}
