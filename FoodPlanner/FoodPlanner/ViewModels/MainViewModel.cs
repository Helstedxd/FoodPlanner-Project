using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FoodPlanner.Helper_Classes;

namespace FoodPlanner.ViewModels
{
    public class MainViewModel
    {
        //private ICommand _changePageCommand;
        private ICommand _goToInventoryCommand;
        private ICommand _goToShoppingListCommand;
        private ICommand _goToRecipesCommand;
        private ICommand _goToSettingsCommand;
		private ICommand _goToMealPlanCommand;

        private static Uri inventoryPageUri = new Uri("Views/InventoryPage.xaml", UriKind.Relative);
        private static Uri shoppingListPageUri = new Uri("Views/ShoppingListPage.xaml", UriKind.Relative);
        private static Uri recipesPageUri = new Uri("Views/RecipesPage.xaml", UriKind.Relative);
        private static Uri settingsPageUri = new Uri("Views/SettingsPage.xaml", UriKind.Relative);
        private static Uri MealPlanPageUri = new Uri("Views/MealPlanPage.xaml", UriKind.Relative);

        /*public ICommand ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {
                    /*_changePageCommand = new RelayCommand(
                        p => ChangeViewModel((IPageViewModel)p),
                        p => p is IPageViewModel);*

                    _changePageCommand = new RelayCommand(p => ChangePage((Uri)p), p => true);
                }

                return _changePageCommand;
            }
        }*/

        public ICommand GoToInventoryCommand
        {
            get
            {
                if (_goToInventoryCommand == null)
                {
                    _goToInventoryCommand = new RelayCommand(p => ChangePage(inventoryPageUri), p => true);
                }

                return _goToInventoryCommand;
            }
        }


        public ICommand GoToShoppingListCommand
        {
            get
            {
                if (_goToShoppingListCommand == null)
                {
                    _goToShoppingListCommand = new RelayCommand(p => ChangePage(shoppingListPageUri), p => true);
                }

                return _goToShoppingListCommand;
            }
        }


        public ICommand GoToRecipesCommand
        {
            get
            {
                if (_goToRecipesCommand == null)
                {
                    _goToRecipesCommand = new RelayCommand(p => ChangePage(recipesPageUri), p => true);
                }

                return _goToRecipesCommand;
            }
        }

        public ICommand GoToSettingsCommand
        {
            get
            {
                if (_goToSettingsCommand == null)
                {
                    _goToSettingsCommand = new RelayCommand(p => ChangePage(settingsPageUri), p => true);
                }

                return _goToSettingsCommand;
            }
        }
		
		public ICommand GoToMealPlanCommand
        {
            get
            {
                if (_goToMealPlanCommand == null)
                {
                    _goToMealPlanCommand = new RelayCommand(p => ChangePage(MealPlanPageUri), p => true);
                }

                return _goToMealPlanCommand;
            }
        }
        private void ChangePage(Uri pagePath)
        {
            //App.MainFrame.Navigate(new Views.InventoryPage());
            App.NavigationService.Navigate(pagePath);
        }


    }




}
