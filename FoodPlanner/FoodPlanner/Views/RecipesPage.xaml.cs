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

namespace FoodPlanner.Views
{
    /// <summary>
    /// Interaction logic for Recipes.xaml
    /// </summary>
    public partial class Recipes : Page
    {
        public Recipes()
        {
            InitializeComponent();
            List<SearchResults> testtest = new List<SearchResults>();
            List<Recipe> test2 = MainWindow.db.Recipes.Take(20).ToList();

            foreach (Recipe r in test2)
            {
                testtest.Add(new SearchResults(r, 1) { fullMatch=0, partialMatch=0, keyWordMatch=0 });
            }

            listBoxRecipes.ItemsSource = testtest;
        }
    }
}
