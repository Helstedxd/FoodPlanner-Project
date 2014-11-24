using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using FoodPlanner.Models;
using FoodPlanner.ViewModels;
using FoodPlanner.Views;
using System.Windows.Input;
using System.Windows.Controls;
using MvvmFoundation.Wpf;

namespace FoodPlanner
{
    public class Navigator
    {

        #region Fields

        private static NavigationService _navigationService;
        private static ICommand _goToInventoryCommand;
        private static ICommand _goToShoppingListCommand;
        private static ICommand _goToRecipeSearchCommand;
        private static ICommand _goToSettingsCommand;
        private static ICommand _goToMealPlanCommand;
        private static ICommand _goToRecipeCommand;
        private static ICommand _goBackCommand;

        #endregion

        #region Properties & Commands

        public static NavigationService NavigationService
        {
             get
            {
                return _navigationService;
            }
            set
            {
                _navigationService = value;
                _navigationService.Navigated += NavigationService_Navigated;
            }
        }

        public static ICommand GoToInventoryCommand
        {
            get
            {
                if (_goToInventoryCommand == null)
                {
                    _goToInventoryCommand = new RelayCommand(() => Navigator.Navigate(new InventoryPage()));
                }
                return _goToInventoryCommand;
            }
        }

        public static ICommand GoToShoppingListCommand
        {
            get
            {
                if (_goToShoppingListCommand == null)
                {
                    _goToShoppingListCommand = new RelayCommand(() => Navigator.Navigate(new ShoppingListPage()));
                }
                return _goToShoppingListCommand;
            }
        }

        public static ICommand GoToRecipeSearchCommand
        {
            get
            {
                if (_goToRecipeSearchCommand == null)
                {
                    _goToRecipeSearchCommand = new RelayCommand(() => Navigator.Navigate(new RecipeSearchPage()));
                }
                return _goToRecipeSearchCommand;
            }
        }

        public static ICommand GoToSettingsCommand
        {
            get
            {
                if (_goToSettingsCommand == null)
                {
                    _goToSettingsCommand = new RelayCommand(() => Navigator.Navigate(new SettingsPage()));
                }
                return _goToSettingsCommand;
            }
        }

        public static ICommand GoToMealPlanCommand
        {
            get
            {
                if (_goToMealPlanCommand == null)
                {
                    _goToMealPlanCommand = new RelayCommand(() => Navigator.Navigate(new MealPlanPage()));
                }
                return _goToMealPlanCommand;
            }
        }

        public static ICommand GoToRecipeCommand
        {
            get
            {
                if (_goToRecipeCommand == null)
                {
                    _goToRecipeCommand = new RelayCommand<Recipe>(recipe =>
                    {
                        RecipeViewModel rvm = new RecipeViewModel(recipe);
                        RecipePage rp = new RecipePage();
                        rp.DataContext = rvm;
                        Navigator.Navigate(rp);
                    });
                }
                return _goToRecipeCommand;
            }
        }

        public static ICommand GoBackCommand
        {
            get
            {
                if (_goBackCommand == null)
                {
                    _goBackCommand = new RelayCommand(() => NavigationService.GoBack());
                }
                return _goBackCommand;
            }
        }

        #endregion

        #region Methods

        private static void Navigate(Page page)
        {
            if (NavigationService != null)
            {
                NavigationService.Navigate(page);
            }
            else
            {
                //TODO: should this be handled?
                Console.WriteLine("Navigation Service not available!");
            }
        }
        public static void GoBack()
        {
            NavigationService.GoBack();
        }

        private static void NavigationService_Navigated(object sender, NavigationEventArgs e)
        {
            // Only allow back-navigation from the Recipe page
            if (e.Content.GetType() != typeof(FoodPlanner.Views.RecipePage))
            {
                _navigationService.RemoveBackEntry();
            }
        }

        #endregion

    }
}
