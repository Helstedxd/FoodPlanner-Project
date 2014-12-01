using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using FoodPlanner.Models;
using System.Data.Entity.Core;

namespace FoodPlanner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static FoodContext db { get; private set; }
        public static User CurrentUser { get; private set; }

        private void ApplicationStartup(object sender, StartupEventArgs args)
        {
            App.db = new FoodContext();

            try
            {
                App.CurrentUser = App.db.Users.First();
            }
            catch (EntityException ex)
            {
                //TODO: Handle
                //MessageBox.Show(ex.Message);
            }

            MainWindow mainWindow = new MainWindow();
            Navigator.NavigationService = mainWindow.Frame.NavigationService;

            Navigator.NavigationService.Source = new Uri(FoodPlanner.Properties.Settings.Default.StartPage, UriKind.Relative);
            mainWindow.Show();

            DateTime shopAhead = DateTime.Now.AddDays(CurrentUser.ShopAhead);

            //var meals = App.db.Meals.Where(m => m.UserID == CurrentUser.ID && m.IsActive && m.Date < shopAhead);

            var meals = (from m in db.Meals
                         join r in db.Recipes on m.RecipeID equals r.ID
                         where m.UserID == CurrentUser.ID && m.IsActive && m.Date < shopAhead
                         select m).ToList();

            foreach (Meal m in meals)
            {
                Console.WriteLine(m.Date + ": " + m.Recipe.Title);
            }
        }

        private void ApplicationExit(object sender, ExitEventArgs args)
        {
            App.db.SaveChanges();
            App.db.Dispose();
        }

    }
}
