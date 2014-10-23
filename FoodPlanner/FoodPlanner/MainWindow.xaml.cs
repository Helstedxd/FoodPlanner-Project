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

namespace FoodPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FoodContext db;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void selectRecipe_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listIngredients.ItemsSource = ((Recipe)selectRecipe.SelectedItem).RecipeIngredients;
            cookingSteps.ItemsSource = ((Recipe)selectRecipe.SelectedItem).CookingSteps;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.db = new FoodContext();
            selectRecipe.ItemsSource = db.Recipes.ToList<Recipe>();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.db.Dispose();
        }
    }
}
