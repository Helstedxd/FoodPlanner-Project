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
        public GraylistIngredient SelectedGreyListIngredient { get; set; }
        private ICommand _saveListItemCommand;
        private ICommand _addIngredientToUnwantedIngredientsCommand;
        private ICommand _removeingredientFromUnwantedIngredientsCommand;
        private ICommand _incrementShopAheadCommand;
        private ICommand _decrementShopAhead;
        private ICommand _incrementPersonsInHouseholdCommand;
        private ICommand _decrementPersonsInHouseholdCommand;
        private User _currentUser;
        public User CurrentUser {
            get { return _currentUser; }
            set {
                _currentUser = value;
                RaisePropertyChanged("CurrentUser");
            }
        }
        public int PersonsInHouseHold {
            get {
                return App.CurrentUser.PersonsInHouseHold;
            }
        }
        public int ShopAhead {
            get {
                return App.CurrentUser.ShopAhead;
            }
        }
        private InventoryIngredient _inventoryIngredient;
        public InventoryIngredient StockIngredient {
            get { return _inventoryIngredient; }
            set {
                _inventoryIngredient = value;
                RaisePropertyChanged("InventoryIngredient");
            }
        }
        private GraylistIngredient _greyListInventoryIngredient;
        public GraylistIngredient GreyListInventoryIngredient {
            get { return _greyListInventoryIngredient; }
            set {
                _greyListInventoryIngredient = value;
                RaisePropertyChanged("GreyListIngredient");
            }
        }

        #endregion
        
        public SettingsViewModel() {
            StockIngredient = new InventoryIngredient();
            CurrentUser = App.CurrentUser;
            SelectedBlackListIngredient = new BlacklistIngredient();
            SelectedGreyListIngredient = new GraylistIngredient();
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
            StockIngredient.ID = i.ID;

        }

        public ICommand IncrementShopAheadCommand {
            get {
                if (_incrementShopAheadCommand == null) {
                    _incrementShopAheadCommand = new RelayCommand(() => IncrementShopAhead());
                }

                return _incrementShopAheadCommand;
            }
        }

        public ICommand DecrementShopAheadCommmand {
            get {
                if (_decrementShopAhead == null) {
                    _decrementShopAhead = new RelayCommand(() => DecrementShopAhead());
                }

                return _decrementShopAhead;
            }
        }

        public ICommand IncrementPersonsInHouseholdCommand {
            get {
                if (_incrementPersonsInHouseholdCommand == null) {
                    _incrementPersonsInHouseholdCommand = new RelayCommand(() => IncrementPersonsInHousehold());
                }

                return _incrementPersonsInHouseholdCommand;
            }
        }

        public ICommand DecrementPersonsInHouseholdCommand {
            get {
                if (_decrementPersonsInHouseholdCommand == null) {
                    _decrementPersonsInHouseholdCommand = new RelayCommand(() => DecrementPersonsInHousehold());
                }

                return _decrementPersonsInHouseholdCommand;
            }
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
            StockIngredient.Ingredient.ID = ingredient.ID;
        }

        //For alle in/decrement gølder det at de ikke må være 0> og >(eks)1000
        private void IncrementShopAhead() {
            App.CurrentUser.ShopAhead++;
            App.db.SaveChanges();
            RaisePropertyChanged("ShopAhead");
        }

        private void DecrementShopAhead() {
            App.CurrentUser.ShopAhead--;
            App.db.SaveChanges();
            RaisePropertyChanged("ShopAhead");
        }

        private void IncrementPersonsInHousehold() {
            App.CurrentUser.PersonsInHouseHold++;
            App.db.SaveChanges();
            RaisePropertyChanged("PersonsInHouseHold");
        }

        private void DecrementPersonsInHousehold() {
            App.CurrentUser.PersonsInHouseHold--;
            App.db.SaveChanges();
            RaisePropertyChanged("PersonsInHouseHold");
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
