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
using System.Data.Entity;

namespace FoodPlanner.ViewModels
{
    class SettingsViewModel : ObservableObject
    {
        #region Fields

        private ICommand _addIngredientToUnwantedIngredientsCommand,
            _removeingredientFromUnwantedIngredientsCommand,
            _incrementShopAheadCommand,
            _decrementShopAhead,
            _incrementPersonsInHouseholdCommand,
            _decrementPersonsInHouseholdCommand,
            _addNewStockIngredientCommand,
            _removeDietCommand,
            _chooseDietCommand,
            _addNewGreyedIngredientCommand,
            _SaveNewGreyedItemNameCommand,
            _removeStockIngredientCommand,
            _removeGreyListIngredientCommand;

        private User _currentUser;
        private WindowPick _selectedPage = new WindowPick(new Uri(Properties.Settings.Default.StartPage, UriKind.Relative), "");
        private StockQuantity _inventoryIngredient;
        private GraylistIngredient _greyListInventoryIngredient;
        string _ratedDublicateResult,
               _blackedDublicateResult,
               _currenStartUpPage;
        #endregion

        public SettingsViewModel()
        {
            StockIngredient = new StockQuantity();
            CurrentUser = App.CurrentUser;
            SelectedBlackListIngredient = new BlacklistIngredient();
            SelectedStockQuantityIngredient = new StockQuantity();
            SelectedGreyListIngredient = new GraylistIngredient();
            GreyListInventoryIngredient = new GraylistIngredient();
        }

        #region Properties

        public BlacklistIngredient SelectedBlackListIngredient { get; set; }

        public GraylistIngredient SelectedGreyListIngredient { get; set; }

        public StockQuantity SelectedStockQuantityIngredient { get; set; }

        public ObservableCollection<DietPreset> ListOfDiets
        {
            get
            {
                return new ObservableCollection<DietPreset>(App.db.DietPresets);
            }
        }

        public DietPreset SelectedDietPreset { get; set; }


        public int StartWindowIndex
        {
            get
            {
                int count = 1;
                foreach (WindowPick wp in UriList)
                {
                    if (wp.ViewPath.ToString() == Properties.Settings.Default.StartPage)
                    {
                        continue;
                    }
                    else
                    {
                        count++;
                    }
                }
                return count;
            }
        }
        public string RatedDublicate
        {
            get
            {
                return _ratedDublicateResult;
            }
        }
        public string BlackedDublicateResult
        {
            get
            {
                return _blackedDublicateResult;
            }
        }

        public string CurrenStartUpPage
        {
            get
            {
                _currenStartUpPage = GetStartUpPage();
                return _currenStartUpPage;
            }
        }

        public StockQuantity StockIngredient
        {
            get
            {
                return _inventoryIngredient;
            }
            set
            {
                _inventoryIngredient = value;
                RaisePropertyChanged("InventoryIngredient");
            }
        }

        public GraylistIngredient GreyListInventoryIngredient
        {
            get { return _greyListInventoryIngredient; }
            set
            {
                _greyListInventoryIngredient = value;
                RaisePropertyChanged("GreyListIngredient");
            }
        }

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

        public List<WindowPick> UriList
        {
            get
            {
                return CreateUri();
            }
        }

        public WindowPick SelectedPage
        {
            get
            {
                return _selectedPage;
            }
            set
            {
                _selectedPage = value;
                Properties.Settings.Default.StartPage = value.ViewPath.ToString();
                Properties.Settings.Default.Save();

            }
        }

        #endregion

        #region ICommands

        public ICommand RemoveDietCommand
        {
            get
            {
                if (_removeDietCommand == null)
                {
                    _removeDietCommand = new RelayCommand(() => RemoveDiet());
                }

                return _removeDietCommand;
            }
        }

        public ICommand ChooseDietCommand
        {
            get
            {
                if (_chooseDietCommand == null)
                {
                    _chooseDietCommand = new RelayCommand(() => ChooseDiet());
                }

                return _chooseDietCommand;
            }
        }


        public ICommand AddNewStockIngredientCommand
        {
            get
            {
                if (_addNewStockIngredientCommand == null)
                {
                    _addNewStockIngredientCommand = new RelayCommand<Ingredient>(i => AddNewStockIngredient(i));
                }

                return _addNewStockIngredientCommand;
            }
        }

        public ICommand RemoveStockIngredientCommand
        {
            get
            {
                if (_removeStockIngredientCommand == null)
                {
                    _removeStockIngredientCommand = new RelayCommand(() => RemoveStockIngredient());
                }

                return _removeStockIngredientCommand;
            }
        }

        public ICommand SaveNewGreyedIngredientNameCommand
        {
            get
            {
                if (_SaveNewGreyedItemNameCommand == null)
                {
                    _SaveNewGreyedItemNameCommand = new RelayCommand<Ingredient>(i => SaveNewGreyedIentName(i));
                }

                return _SaveNewGreyedItemNameCommand;
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

        public ICommand AddNewGryedIngredientCommand
        {
            get
            {
                if (_addNewGreyedIngredientCommand == null)
                {
                    _addNewGreyedIngredientCommand = new RelayCommand(() => AddNewGreyedIngredient());
                }

                return _addNewGreyedIngredientCommand;
            }
        }

        public ICommand RemoveGreyListIngredientCommand
        {
            get
            {
                if (_removeGreyListIngredientCommand == null)
                {
                    _removeGreyListIngredientCommand = new RelayCommand(() => RemoveGreylistIngredient());
                }

                return _removeGreyListIngredientCommand;
            }
        }
        #endregion

        #region Methods

        private string GetStartUpPage()
        {
            string result = "Pick a window";

            foreach (WindowPick wp in UriList)
            {
                if (Properties.Settings.Default.StartPage == wp.ViewPath.ToString())
                {
                    result = wp.Name;
                }
            }
            return result;
        }

        private void RemoveGreylistIngredient()
        {
            App.db.GraylistIngredients.RemoveRange(App.db.GraylistIngredients.Where(gli => gli.Id == SelectedGreyListIngredient.Id && gli.UserID == SelectedGreyListIngredient.UserID));
            App.db.SaveChanges();
        }

        private bool ListDublicate(int ID)
        {
            IQueryable<GraylistIngredient> grayList = App.db.GraylistIngredients.Where(gli => gli.UserID == App.CurrentUser.ID);
            IQueryable<BlacklistIngredient> blackList = App.db.BlacklistIngredients.Where(gli => gli.UserID == App.CurrentUser.ID);
            bool dublicat = false;

            foreach (GraylistIngredient gli in grayList)
            {
                if (gli.IngredientID == ID)
                {
                    dublicat = true;
                }
            }

            foreach (BlacklistIngredient bli in blackList)
            {
                if (bli.IngredientID == ID)
                {
                    dublicat = true;
                }
            }

            return dublicat;
        }

        private List<WindowPick> CreateUri()
        {
            List<WindowPick> returnList = new List<WindowPick>()
            {
                new WindowPick(new Uri("Views/MealPlanPage.xaml",     UriKind.Relative), "Food Plan"),
                new WindowPick(new Uri("Views/RecipeSearchPage.xaml", UriKind.Relative), "Search"),
                new WindowPick(new Uri("Views/ShoppingListPage.xaml", UriKind.Relative), "Shopping List"),
                new WindowPick(new Uri("Views/InventoryPage.xaml",    UriKind.Relative), "Inventory"),
                new WindowPick(new Uri("Views/SettingsPage.xaml",     UriKind.Relative), "Settings")
            };
            return returnList;
        }

        private void IncrementShopAhead()
        {
            App.CurrentUser.ShopAhead++;
            RaisePropertyChanged("ShopAhead");
        }

        private void DecrementShopAhead()
        {
            if (App.CurrentUser.ShopAhead > 0)
            {
                App.CurrentUser.ShopAhead--;
                RaisePropertyChanged("ShopAhead");
            }
        }

        private void IncrementPersonsInHousehold()
        {
            App.CurrentUser.PersonsInHouseHold++;
            RaisePropertyChanged("PersonsInHouseHold");
        }

        private void DecrementPersonsInHousehold()
        {
            if (App.CurrentUser.PersonsInHouseHold > 1)
            {
                App.CurrentUser.PersonsInHouseHold--;
                RaisePropertyChanged("PersonsInHouseHold");
            }
        }

        private void ChooseDiet()
        {
            RemoveDiet();

            foreach (DietRule dr in SelectedDietPreset.DietRules)
            {
                if (dr.IngredientIsBlacklisted)
                {
                    App.db.BlacklistIngredients.Add(new BlacklistIngredient() { UserID = App.CurrentUser.ID, IsFromDiet = true, IngredientID = dr.IngredientID });
                }
                else
                {
                    App.db.GraylistIngredients.Add(new GraylistIngredient() { UserID = App.CurrentUser.ID, IsFromDiet = true, IngredientID = dr.IngredientID, IngredientValue = dr.IngredientValue });
                }
            }

        }

        private void RemoveDiet()
        {
            App.db.GraylistIngredients.RemoveRange(App.db.GraylistIngredients.Where(gli => gli.UserID == App.CurrentUser.ID && gli.IsFromDiet));
            App.db.BlacklistIngredients.RemoveRange(App.db.BlacklistIngredients.Where(bli => bli.UserID == App.CurrentUser.ID && bli.IsFromDiet));
            App.db.SaveChanges();
        }

        private void AddNewStockIngredient(Ingredient ingredient)
        {
            StockQuantity StockIngredientToBeAdded = new StockQuantity() { IngredientID = ingredient.ID, Quantity = 0, UserID = App.CurrentUser.ID };

            App.db.StockQuantities.Add(StockIngredientToBeAdded);
            App.db.SaveChanges();
        }

        private void RemoveStockIngredient()
        {
            App.db.StockQuantities.RemoveRange(App.db.StockQuantities.Where(sq => sq.ID == SelectedStockQuantityIngredient.ID && sq.UserID == SelectedStockQuantityIngredient.UserID));
            App.db.SaveChanges();
        }

        private void AddNewGreyedIngredient()
        {
            GraylistIngredient IngredientToBeAdded = new GraylistIngredient()
            {
                IngredientID = GreyListInventoryIngredient.IngredientID,
                UserID = GreyListInventoryIngredient.UserID,
                IngredientValue = GreyListInventoryIngredient.IngredientValue
            };
            bool dublicat = ListDublicate(IngredientToBeAdded.IngredientID);

            if (!dublicat)
            {
                _ratedDublicateResult = "";
                App.db.GraylistIngredients.Add(IngredientToBeAdded);
                App.db.SaveChanges();
            }
            else
            {
                _ratedDublicateResult = "Item was not added (dublicate)";
            }
            RaisePropertyChanged("RatedDublicate");
        }

        private void SaveNewGreyedIentName(Ingredient ingredient)
        {
            GreyListInventoryIngredient.IngredientID = ingredient.ID;
            GreyListInventoryIngredient.Ingredient = ingredient;
            GreyListInventoryIngredient.UserID = App.CurrentUser.ID;
            RaisePropertyChanged("GreyListInventoryIngredient");
        }

        private void AddIngredientToUnwantedIngredients(Ingredient ingredient)
        {
            BlacklistIngredient IngredientToBeAdded = new BlacklistIngredient()
            {
                IngredientID = ingredient.ID,
                UserID = App.CurrentUser.ID
            };
            bool dublicat = ListDublicate(IngredientToBeAdded.IngredientID);

            if (!dublicat)
            {
                _blackedDublicateResult = "";
                App.db.BlacklistIngredients.Add(IngredientToBeAdded);
                App.db.SaveChanges();
            }
            else
            {
                _blackedDublicateResult = "Item was not added (dublicate)";
            }
            RaisePropertyChanged("BlackedDublicateResult");
        }

        private void RemoveIngredientFromUnwantedIngredients()
        {
            App.db.BlacklistIngredients.RemoveRange(App.db.BlacklistIngredients.Where(bli => bli.Id == SelectedBlackListIngredient.Id && bli.UserID == App.CurrentUser.ID));
            App.db.SaveChanges();
        }

        #endregion
    }
}