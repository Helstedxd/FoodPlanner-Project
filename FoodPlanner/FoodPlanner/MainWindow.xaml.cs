using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FoodPlanner.Models;
using System.Data.Entity.Core;

namespace FoodPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            Navigator.NavigationService = this.Frame.NavigationService;
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
            App.db.Dispose();
            Close();
            /*
            */
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.db.Dispose();
        }

        //TODO: remove these click handlers.
        private void openSearch_Click(object sender, RoutedEventArgs e)
        {
            var openSearch = new Search();
            openSearch.Show();
        }

        private void openFoodplan_Click(object sender, RoutedEventArgs e)
        {
            //openFoodplan.Show();
        }

        private void openRecommendedRecipes_Click(object sender, RoutedEventArgs e)
        {
            new RecommendedRecipesWindow().Show();
        }

    }
}
