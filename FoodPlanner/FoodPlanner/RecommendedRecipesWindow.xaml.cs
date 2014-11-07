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
    public class RecipeSearchResult
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

            /*var combined_query = 
                from ii in MainWindow.db.InventoryIngredients
                where ii.UserID == MainWindow.CurrentUser.ID
                group ii by ii.IngredientID into iig
                select new { IngredientID = iig.FirstOrDefault().IngredientID, TotalQuantity = iig.Sum(i => i.Quantity) } into iig
                join ri in MainWindow.db.RecipeIngredients on iig.IngredientID equals ri.IngredientID
                group new { Recipe = ri.Recipe, RecipeQuantity = ri.Quantity, InventoryQuantity = iig.TotalQuantity, IngredientCount = ri.Recipe.RecipeIngredients.Count() } by ri.RecipeID;
            */

            var InventoryIngredientsTotalQuantity =
                from ii in MainWindow.db.InventoryIngredients
                where ii.UserID == MainWindow.CurrentUser.ID
                group ii by ii.IngredientID into iig
                select new
                {
                    IngredientID = iig.FirstOrDefault().IngredientID,
                    TotalQuantity = iig.Sum(i => i.Quantity)
                };

            var RecipeIngredientsWithQuantityFromInventory =
                from iitq in InventoryIngredientsTotalQuantity
                join ri in MainWindow.db.RecipeIngredients on iitq.IngredientID equals ri.IngredientID
                group new
                {
                    Recipe = ri.Recipe,
                    RecipeQuantity = ri.Quantity,
                    InventoryQuantity = iitq.TotalQuantity,
                    IngredientCount = ri.Recipe.RecipeIngredients.Count()
                } by ri.RecipeID;

            List<RecipeSearchResult> searchResults = new List<RecipeSearchResult>();

            DateTime startTime = DateTime.Now;

            foreach (var group in RecipeIngredientsWithQuantityFromInventory)
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

                searchResults.Add(new RecipeSearchResult()
                {
                    Recipe = first.Recipe,
                    MatchPercentage = totalPercent / first.IngredientCount // average
                });
            }

            TimeSpan e = DateTime.Now - startTime;

            Console.WriteLine("Calculated Match Percentage for {0} recipes in {1}", RecipeIngredientsWithQuantityFromInventory.Count(), e);

            recommendedRecipesDataGrid.ItemsSource = searchResults.OrderByDescending(s => s.MatchPercentage);
        }

        private void recommendedRecipesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RecipeSearchResult r = (RecipeSearchResult)recommendedRecipesDataGrid.SelectedItem;
            var show = new ShowRecipe(r.Recipe);
            show.Show();
        }
    }
}
