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
        public static FoodContext db { get; set; }
        public static User CurrentUser { get; set; }

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

            MainWindow mw = new MainWindow();
            Navigator.NavigationService = mw.Frame.NavigationService;
 
            mw.Show();
        }

        private void ApplicationExit(object sender, ExitEventArgs args)
        {
            App.db.Dispose();
        }

    }
}
