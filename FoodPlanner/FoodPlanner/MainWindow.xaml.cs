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

namespace FoodPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static FoodContext db;
        public static User CurrentUser { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            App.NavigationService = this.Frame.NavigationService;

            db = new FoodContext();
            CurrentUser = db.Users.First();

            var test = new Search();
            test.ShowDialog();
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            db.Dispose();
        }

        //TODO: remove these click handlers.
        private void openSearch_Click(object sender, RoutedEventArgs e)
        {
            var openSearch = new Search();
            openSearch.Show();
        }

        private void openFoodplan_Click(object sender, RoutedEventArgs e)
        {
            var openFoodplan = new FoodPlanPage();
            //openFoodplan.Show();
        }

        private void openInventory_Click(object sender, RoutedEventArgs e)
        {
            var openInventory = new InventoryWindow();
            openInventory.Show();

        }

        private void openRecommendedRecipes_Click(object sender, RoutedEventArgs e)
        {
            new RecommendedRecipesWindow().Show();
        }

    }
}
