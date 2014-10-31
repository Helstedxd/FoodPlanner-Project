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

            //TODO: the grouping contains a lot of dublicate data, maybe this can be avoided...
            var q3 = from ri in MainWindow.db.RecipeIngredients
                     join ii in MainWindow.db.InventoryIngredients on ri.IngredientID equals ii.IngredientID
                     select new { RecipeID = ri.RecipeID, Recipe = ri.Recipe, RecipeQuantity = ri.Quantity, InventoryRecipe = ii.Quantity, IngredientCount = ri.Recipe.RecipeIngredients.Count() } into c
                     group c by c.RecipeID into g
                     select g;

            List<search_result_x> searchResults = new List<search_result_x>();

            DateTime startTime = DateTime.Now;

            foreach (var group in q3)
            {
                var first = group.First();

                decimal totalPercent = 0;
                foreach (var g in group)
                {
                    if (g.InventoryRecipe >= g.RecipeQuantity)
                    {
                        totalPercent += 1;
                    }
                    else
                    {
                        if (g.RecipeQuantity == 0)
                            Console.WriteLine("This should not be possible...");
                        totalPercent += g.InventoryRecipe / g.RecipeQuantity;
                    }

                }

                searchResults.Add(new search_result_x()
                {
                    Recipe = first.Recipe,
                    MatchPercentage = totalPercent / first.IngredientCount
                });
            }

            TimeSpan e = DateTime.Now - startTime;

            Console.WriteLine("Calculated Match Percentage for {0} recipes in {1}", q3.Count(), e);

            recommendedRecipesDataGrid.ItemsSource = searchResults.OrderByDescending(s => s.MatchPercentage);

        }
    }
}
