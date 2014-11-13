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

namespace FoodPlanner
{
    public class Navigator
    {

        #region Fields

        private static ICommand _goToInventoryCommand;
        private static ICommand _goToShoppingListCommand;
        private static ICommand _goToRecipeSearchCommand;
        private static ICommand _goToSettingsCommand;
        private static ICommand _goToMealPlanCommand;
        private static ICommand _goToRecipeCommand;

        #endregion

        #region Properties & Commands

        public static NavigationService NavigationService { private get; set; }

        public static ICommand GoToInventoryCommand
        {
            get
            {
                if (_goToInventoryCommand == null)
                {
                    _goToInventoryCommand = new RelayCommand(p => Navigator.Navigate(new InventoryPage()));
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
                    _goToShoppingListCommand = new RelayCommand(p => Navigator.Navigate(new ShoppingListPage()));
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
                    _goToRecipeSearchCommand = new RelayCommand(p => Navigator.Navigate(new RecipeSearchPage()));
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
                    _goToSettingsCommand = new RelayCommand(p => Navigator.Navigate(new SettingsPage()));
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
                    _goToMealPlanCommand = new RelayCommand(p => Navigator.Navigate(new MealPlanPage()));
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
                    _goToRecipeCommand = new RelayCommand(param =>
                    {
                        Recipe r = (Recipe)param;
                        if (r != null)
                        {
                            RecipeViewModel rvm = new RecipeViewModel((Recipe)p);
                            RecipePage rp = new RecipePage();
                            rp.DataContext = rvm;
                            Navigator.Navigate(rp);
                        }
                    });
                }
                return _goToRecipeCommand;
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

        #endregion

    }
}
