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

namespace FoodPlanner.ViewModels
{
    class SettingsViewModel : ObservableObject
    {
        #region Fields

        public BlacklistIngredient SelectedBlackListIngredient { get; set; }
        public GraylistIngredient SelectedGreyListIngredient { get; set; }
        private ICommand _saveNewStockIngredientNameCommand,
            _addIngredientToUnwantedIngredientsCommand,
            _removeingredientFromUnwantedIngredientsCommand,
            _incrementShopAheadCommand,
            _decrementShopAhead,
            _incrementPersonsInHouseholdCommand,
            _decrementPersonsInHouseholdCommand,
            _addNewStockIngredientCommand,
            _addNewFavoriteIngredientCommand;
        private User _currentUser;
        private Uri _selectedPage = new Uri(Properties.Settings.Default.StartPage, UriKind.Relative);
        public User CurrentUser
        {
            get { return _currentUser; }
            set
            {
                _currentUser = value;
                RaisePropertyChanged("CurrentUser");
            }
        }

        public int PersonsInHouseHold
        {
            get
            {
                return App.CurrentUser.PersonsInHouseHold;
            }
        }
        public int ShopAhead
        {
            get
            {
                return App.CurrentUser.ShopAhead;
            }
        }
        private StockQuantity _inventoryIngredient;
        public StockQuantity StockIngredient
        {
            get { return _inventoryIngredient; }
            set
            {
                _inventoryIngredient = value;
                RaisePropertyChanged("InventoryIngredient");
            }
        }
        private GraylistIngredient _greyListInventoryIngredient;
        public GraylistIngredient GreyListInventoryIngredient
        {
            get { return _greyListInventoryIngredient; }
            set
            {
                _greyListInventoryIngredient = value;
                RaisePropertyChanged("GreyListIngredient");
            }
        }

        #endregion

        public SettingsViewModel()
        {
            StockIngredient = new StockQuantity();
            CurrentUser = App.CurrentUser;
            SelectedBlackListIngredient = new BlacklistIngredient();
            SelectedGreyListIngredient = new GraylistIngredient();
        }

        #region Properties

        public List<Uri> UriList
        {
            get
            {
                return CreateUri();
            }
        }

        public Uri SelectedPage
        {
            get
            {
                return _selectedPage;
            }
            set
            {
                _selectedPage = value;
                Properties.Settings.Default.StartPage = value.ToString();
                Properties.Settings.Default.Save();
            }
        }

        #endregion

        #region ICommands
        public ICommand SaveNewStockIngredientNameCommand
        {
            get
            {
                if (_saveNewStockIngredientNameCommand == null)
                {
                    _saveNewStockIngredientNameCommand = new RelayCommand<Ingredient>(i => SaveNewStockIngredientName(i));
                }

                return _saveNewStockIngredientNameCommand;
            }
        }

        public ICommand IncrementShopAheadCommand
        {
            get
            {
                if (_incrementShopAheadCommand == null)
                {
                    _incrementShopAheadCommand = new RelayCommand(() => IncrementShopAhead());
                }

                return _incrementShopAheadCommand;
            }
        }

        public ICommand DecrementShopAheadCommmand
        {
            get
            {
                if (_decrementShopAhead == null)
                {
                    _decrementShopAhead = new RelayCommand(() => DecrementShopAhead());
                }

                return _decrementShopAhead;
            }
        }

        public ICommand IncrementPersonsInHouseholdCommand
        {
            get
            {
                if (_incrementPersonsInHouseholdCommand == null)
                {
                    _incrementPersonsInHouseholdCommand = new RelayCommand(() => IncrementPersonsInHousehold());
                }

                return _incrementPersonsInHouseholdCommand;
            }
        }

        public ICommand DecrementPersonsInHouseholdCommand
        {
            get
            {
                if (_decrementPersonsInHouseholdCommand == null)
                {
                    _decrementPersonsInHouseholdCommand = new RelayCommand(() => DecrementPersonsInHousehold());
                }

                return _decrementPersonsInHouseholdCommand;
            }
        }

        public ICommand AddIngredientToUnwantedIngredientsCommand
        {
            get
            {
                if (_addIngredientToUnwantedIngredientsCommand == null)
                {
                    _addIngredientToUnwantedIngredientsCommand = new RelayCommand<Ingredient>(i => AddIngredientToUnwantedIngredients(i));
                }

                return _addIngredientToUnwantedIngredientsCommand;

            }
        }

        public ICommand RemoveIngredientFromUnwantedIngredientsCommand
        {
            get
            {
                if (_removeingredientFromUnwantedIngredientsCommand == null)
                {
                    _removeingredientFromUnwantedIngredientsCommand = new RelayCommand(() => RemoveIngredientFromUnwantedIngredients());
                }
                return _removeingredientFromUnwantedIngredientsCommand;
            }
        }

        public ICommand AddNewStockIngredientCommand
        {
            get
            {
                if (_addNewStockIngredientCommand == null)
                {
                    _addNewStockIngredientCommand = new RelayCommand(() => AddNewStockIngredient());
                }

                return _addNewStockIngredientCommand;
            }
        }

        public ICommand AddNewFavoriteIngredientCommand
        {
            get
            {
                if (_addNewFavoriteIngredientCommand == null)
                {
                    _addNewFavoriteIngredientCommand = new RelayCommand(() => AddNewFavoriteIngredient());
                }

                return _addNewFavoriteIngredientCommand;
            }
        }

        #endregion

        #region Methods

        //Crasher når "App.db.SaveChanges();" køres.  

        private void AddNewStockIngredient()
        {
            StockQuantity StockIngredientToBeAdded = new StockQuantity() { IngredientID = StockIngredient.Ingredient.ID, Quantity = StockIngredient.Quantity, UserID = App.CurrentUser.ID };

            App.db.StockQuantities.Add(StockIngredientToBeAdded);
            App.db.SaveChanges();
        }

        private void AddNewFavoriteIngredient()
        {
            throw new NotImplementedException();
        }

        private List<Uri> CreateUri()
        {
            List<Uri> returnList = new List<Uri>()
            {
                new Uri("Views/InventoryPage.xaml", UriKind.Relative),
                new Uri("Views/MealPlanPage.xaml", UriKind.Relative),
                new Uri("Views/RecipeSearchPage.xaml", UriKind.Relative),
                new Uri("Views/SettingsPage.xaml", UriKind.Relative),
                new Uri("Views/ShoppingListPage.xaml", UriKind.Relative)
            };
            return returnList;
        }

        private void SaveNewStockIngredientName(Ingredient ingredient)
        {
            StockIngredient.Ingredient = ingredient;
            //App.db.SaveChanges();
            RaisePropertyChanged("StockIngredient");
            Console.WriteLine("Der er givet et navn");
        }

        //For alle in/decrement gælder det at de ikke må være 0> og >(eks)1000
        private void IncrementShopAhead()
        {
            App.CurrentUser.ShopAhead++;
            App.db.SaveChanges();
            RaisePropertyChanged("ShopAhead");
        }

        private void DecrementShopAhead()
        {
            App.CurrentUser.ShopAhead--;
            App.db.SaveChanges();
            RaisePropertyChanged("ShopAhead");
        }

        private void IncrementPersonsInHousehold()
        {
            App.CurrentUser.PersonsInHouseHold++;
            App.db.SaveChanges();
            RaisePropertyChanged("PersonsInHouseHold");
        }

        private void DecrementPersonsInHousehold()
        {
            App.CurrentUser.PersonsInHouseHold--;
            App.db.SaveChanges();
            RaisePropertyChanged("PersonsInHouseHold");
        }

        //Fix: Det skal sikres at man ikke kan tilføje den samme ingrediens flere gange.
        private void AddIngredientToUnwantedIngredients(Ingredient ingredient)
        {
            BlacklistIngredient IngredientToBeAdded = new BlacklistIngredient() { IngredientID = ingredient.ID, UserID = App.CurrentUser.ID };
            App.db.BlacklistIngredients.Add(IngredientToBeAdded);
            App.db.SaveChanges();
        }

        //Fix: Metoden skal også sikre mod at der er valgt et element i listboxen.
        private void RemoveIngredientFromUnwantedIngredients()
        {
            App.db.BlacklistIngredients.RemoveRange(App.db.BlacklistIngredients.Where(bli => bli.Id == SelectedBlackListIngredient.Id && bli.UserID == App.CurrentUser.ID));
            App.db.SaveChanges();
        }

        #endregion
    }
}
