using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace FoodPlanner
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : Window
    {

        public Search()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource recipeViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("recipeViewSource")));
            recipeViewSource.Source = MainWindow.db.Recipes.ToList();
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void doSearch()
        {
            List<string> searchQuery = searchBox.Text.Split(',').Select(s => s.Trim()).ToList();

            searchList.ItemsSource = (from rec in MainWindow.db.Recipes join ri in MainWindow.db.RecipeIngredients on rec.ID equals ri.RecipeID join ing in MainWindow.db.Ingredients on ri.IngredientID equals ing.ID where searchQuery.Any(s => ing.Name.Contains(s)) || searchQuery.Any(s => rec.Title.Contains(s)) select rec.Title).Distinct().ToList();
        }
    }
}
