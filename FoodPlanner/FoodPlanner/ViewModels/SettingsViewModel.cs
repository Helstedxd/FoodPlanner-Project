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

        private ICommand _addIngredientToUnwantedIngredientsCommand;
        private ICommand _removeingredientFromUnwantedIngredientsCommand;
        private ICommand _incrementShopAheadCommand;
        private ICommand _decrementShopAhead;
        private ICommand _incrementPersonsInHouseholdCommand;
        private ICommand _decrementPersonsInHouseholdCommand;
        private ICommand _addNewStockIngredientCommand;
        private ICommand _removeDietCommand;
        private ICommand _chooseDietCommand;
        private ICommand _addNewGreyedIngredientCommand;
        private ICommand _SaveNewGreyedItemNameCommand;
        private ICommand _removeStockIngredientCommand;
        private ICommand _removeGreyListIngredientCommand;

        private KeyValuePair<string, Uri> _selectedStartUpPage;
        private StockQuantity _inventoryIngredient;
        private GraylistIngredient _greyListInventoryIngredient;
        private string _ratedDublicateResult;
        private string _blackedDublicateResult;
        private string _currenStartUpPage;
        #endregion

        public SettingsViewModel()
        {
            StockIngredient = new StockQuantity();
            GreyListInventoryIngredient = null;
            IngredientValue = 1;
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

        public string CurrentDiet
        {
            get
            {
                string currentDiet = "";
                if (CurrentUser.ChosenDiet != 0)
                {
                    currentDiet = App.db.DietPresets.Where(dp => dp.ID == CurrentUser.ChosenDiet).SingleOrDefault().DietName;
                }
                return "Current Diet: " + currentDiet;
            }
        }

        public int IngredientValue
        {
            get;
            set;
        }

        public DietPreset SelectedDietPreset { get; set; }

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
            get { return App.CurrentUser; }
        }

        public ObservableCollection<GraylistIngredient> GraylistIngredientsWithoutDiets
        {
            get
            {
                return new ObservableCollection<GraylistIngredient>(App.CurrentUser.GraylistIngredients.Where(gi => !gi.IsFromDiet));
            }
        }

        public ObservableCollection<BlacklistIngredient> BlacklistIngredientsWithoutDiets
        {
            get
            {
                return new ObservableCollection<BlacklistIngredient>(App.CurrentUser.BlacklistIngredients.Where(bi => !bi.IsFromDiet));
            }
        }

        public int PersonsInHouseHold
        {
            get
            {
                return App.CurrentUser.PersonsInHouseHold;
            }
            set
            {
                App.CurrentUser.PersonsInHouseHold = value;
                RaisePropertyChanged("PersonsInHouseHold");
            }
        }

        public int ShopAhead
        {
            get
            {
                return App.CurrentUser.ShopAhead;
            }
            set
            {
                App.CurrentUser.ShopAhead = value;
                RaisePropertyChanged("ShopAhead");

            }
        }

        public List<KeyValuePair<string, Uri>> PageList
        {
            get
            {
                return Navigator.Pages;
            }
        }

        public KeyValuePair<string, Uri> SelectedStartUpPage
        {
            get
            {
                if (_selectedStartUpPage.Equals(default(KeyValuePair<string, Uri>)))
                {
                    // Find the start-up page from settings.
                    foreach (KeyValuePair<string, Uri> page in PageList)
                    {
                        if (Properties.Settings.Default.StartPage == page.Value.ToString())
                        {
                            _selectedStartUpPage = page;
                        }
                    }
                }
                return _selectedStartUpPage;
            }
            set
            {
                _selectedStartUpPage = value;
                Properties.Settings.Default.StartPage = _selectedStartUpPage.Value.ToString();
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

        public ICommand AddNewGreydIngredientCommand
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

        private void RemoveGreylistIngredient()
        {
            if (SelectedGreyListIngredient != null)
            {
                App.db.GraylistIngredients.Remove(SelectedGreyListIngredient);
                App.db.SaveChanges();
                RaisePropertyChanged("GraylistIngredientsWithoutDiets");
            }
        }

        private bool ListDublicate(int ID, bool isGreyListObject)
        {
            IQueryable<GraylistIngredient> grayList = App.db.GraylistIngredients.Where(gli => gli.UserID == App.CurrentUser.ID);
            IQueryable<BlacklistIngredient> blackList = App.db.BlacklistIngredients.Where(gli => gli.UserID == App.CurrentUser.ID);
            bool dublicat = false;

            foreach (GraylistIngredient gli in grayList)
            {
                if (gli.IngredientID == ID)
                {
                    dublicat = true;
                    if (!isGreyListObject)
                    {
                        _blackedDublicateResult = "ERROR: Ingredient is Blacklisted";
                        RaisePropertyChanged("BlackedDublicateResult");
                    }
                    else
                    {
                        _ratedDublicateResult = "ERROR: The ingredient is Rated";
                        RaisePropertyChanged("RatedDublicate");
                    }
                }
            }

            if (dublicat == false)
            {
                foreach (BlacklistIngredient bli in blackList)
                {
                    if (bli.IngredientID == ID)
                    {
                        dublicat = true;
                        if (isGreyListObject)
                        {
                            _ratedDublicateResult = "ERROR: The ingreident is Rated";
                            RaisePropertyChanged("RatedDublicate");
                        }
                        else
                        {
                            _blackedDublicateResult = "ERROR: Ingreident is Blacklisted";
                            RaisePropertyChanged("BlackedDublicateResult");
                        }
                    }
                }
            }
            return dublicat;
        }

        private void IncrementShopAhead()
        {
            ShopAhead++;
        }

        private void DecrementShopAhead()
        {
            if (ShopAhead > 0)
            {
                ShopAhead--;
            }
        }

        private void IncrementPersonsInHousehold()
        {
            PersonsInHouseHold++;
        }

        private void DecrementPersonsInHousehold()
        {
            if (PersonsInHouseHold > 1)
            {
                PersonsInHouseHold--;
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

            CurrentUser.ChosenDiet = SelectedDietPreset.ID;
            App.db.SaveChanges();
            RaisePropertyChanged("CurrentDiet");
        }

        private void RemoveDiet()
        {
            App.db.GraylistIngredients.RemoveRange(App.db.GraylistIngredients.Where(gli => gli.UserID == App.CurrentUser.ID && gli.IsFromDiet));
            App.db.BlacklistIngredients.RemoveRange(App.db.BlacklistIngredients.Where(bli => bli.UserID == App.CurrentUser.ID && bli.IsFromDiet));

            CurrentUser.ChosenDiet = 0;
            App.db.SaveChangesAsync();
            RaisePropertyChanged("CurrentDiet");
        }

        private void AddNewStockIngredient(Ingredient ingredient)
        {
            StockQuantity StockIngredientToBeAdded = new StockQuantity() { IngredientID = ingredient.ID, Quantity = 0, UserID = App.CurrentUser.ID };

            App.db.StockQuantities.Add(StockIngredientToBeAdded);
            App.db.SaveChanges();
        }

        private void RemoveStockIngredient()
        {
            if (SelectedStockQuantityIngredient != null)
            {
                App.db.StockQuantities.Remove(SelectedStockQuantityIngredient);
                App.db.SaveChanges();
            }
        }

        private void AddNewGreyedIngredient()
        {
            if (GreyListInventoryIngredient != null)
            {
                bool dublicate = ListDublicate(GreyListInventoryIngredient.IngredientID, true);

                if (!dublicate)
                {
                    GreyListInventoryIngredient.IngredientValue = IngredientValue;
                    _ratedDublicateResult = "";
                    RaisePropertyChanged("RatedDublicate");
                    // App.db.GraylistIngredients.Add(IngredientToBeAdded);
                    App.CurrentUser.GraylistIngredients.Add(GreyListInventoryIngredient);
                    RaisePropertyChanged("GraylistIngredientsWithoutDiets");
                    App.db.SaveChanges();
                }
            }
        }

        private void SaveNewGreyedIentName(Ingredient ingredient)
        {
            GreyListInventoryIngredient = new GraylistIngredient()
            {
                IngredientID = ingredient.ID,
                Ingredient = ingredient,
                UserID = App.CurrentUser.ID
            };
            RaisePropertyChanged("GreyListInventoryIngredient");
        }

        private void AddIngredientToUnwantedIngredients(Ingredient ingredient)
        {
            BlacklistIngredient IngredientToBeAdded = new BlacklistIngredient()
            {
                IngredientID = ingredient.ID,
            };
            bool dublicate = ListDublicate(IngredientToBeAdded.IngredientID, false);

            if (!dublicate)
            {
                _blackedDublicateResult = "";
                RaisePropertyChanged("BlackedDublicateResult");
                App.CurrentUser.BlacklistIngredients.Add(IngredientToBeAdded);
                RaisePropertyChanged("BlacklistIngredientsWithoutDiets");
                App.db.SaveChanges();
            }
        }

        private void RemoveIngredientFromUnwantedIngredients()
        {
            if (SelectedBlackListIngredient != null)
            {
                App.db.BlacklistIngredients.Remove(SelectedBlackListIngredient);
                App.db.SaveChanges();
                RaisePropertyChanged("BlacklistIngredientsWithoutDiets");
            }
        }

        #endregion
    }
}