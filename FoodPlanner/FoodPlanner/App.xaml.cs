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

            var test = new Search();
            test.ShowDialog();

            MainWindow mainWindow = new MainWindow();
            Navigator.NavigationService = mainWindow.Frame.NavigationService;
            Navigator.GoToMealPlanCommand.Execute(null);

            mainWindow.Show();
        }

        private void ApplicationExit(object sender, ExitEventArgs args)
        {
            App.db.Dispose();
        }

    }
}
