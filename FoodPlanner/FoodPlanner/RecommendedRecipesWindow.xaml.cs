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
using System.Windows.Shapes;
using FoodPlanner.Models;

namespace FoodPlanner
{
    // TODO: merge with other search result stuff
    public class search_result_x
    {
        public Recipe Recipe { get; set; }
        public decimal MatchPercentage { get; set; }
    }

    /// <summary>
    /// Interaction logic for RecommendedRecipesWindow.xaml
    /// </summary>
    public partial class RecommendedRecipesWindow : Window
    {
        public RecommendedRecipesWindow()
        {
            InitializeComponent();

            var q4 = from ii in MainWindow.db.InventoryIngredients
                     where ii.UserID == MainWindow.CurrentUser.ID
                     group ii by ii.IngredientID into iig
                     select new { IngredientID = iig.FirstOrDefault().IngredientID, TotalQuantity = iig.Sum(i => i.Quantity) } into iig
                     join ri in MainWindow.db.RecipeIngredients on iig.IngredientID equals ri.IngredientID
                     group new { Recipe = ri.Recipe, RecipeQuantity = ri.Quantity, InventoryQuantity = iig.TotalQuantity, IngredientCount = ri.Recipe.RecipeIngredients.Count() } by ri.RecipeID;

            List<search_result_x> searchResults = new List<search_result_x>();

            DateTime startTime = DateTime.Now;

            foreach (var group in q4)
            {

                decimal totalPercent = 0;
                foreach (var g in group)
                {
                    if (g.InventoryQuantity >= g.RecipeQuantity)
                    {
                        totalPercent += 1;
                    }
                    else
                    {
                        totalPercent += g.InventoryQuantity / g.RecipeQuantity;
                    }
                }

                // All items in the group has the same IngredientCount and Recipe property so we just select the first.
                var first = group.First();

                searchResults.Add(new search_result_x()
                {
                    Recipe = first.Recipe,
                    MatchPercentage = totalPercent / first.IngredientCount // average
                });
            }

            TimeSpan e = DateTime.Now - startTime;

            Console.WriteLine("Calculated Match Percentage for {0} recipes in {1}", q4.Count(), e);

            recommendedRecipesDataGrid.ItemsSource = searchResults.OrderByDescending(s => s.MatchPercentage);

        }

        private void recommendedRecipesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            search_result_x r = (search_result_x)recommendedRecipesDataGrid.SelectedItem;
            var show = new ShowRecipe(r.Recipe);
            show.Show();
        }
    }
}
