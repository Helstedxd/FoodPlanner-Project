using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : Window
    {
        private List<InventoryIngredient> inventoryList = MainWindow.db.InventoryIngredients.Where(ii => ii.UserID == MainWindow.CurrentUser.ID).ToList();

        public Search()
        {
            InitializeComponent();
        }

        private void startSearch_Click(object sender, RoutedEventArgs e)
        {
            List<string> searchQuery = searchBox.Text.Split(',').Select(s => s.Trim()).ToList();

            DateTime test = DateTime.Now;

            IQueryable<Recipe> recipes = MainWindow.db.Recipes.Where(r => searchQuery.Any(s => r.Title.Contains(s)));
            IQueryable<Ingredient> ingredients = MainWindow.db.Ingredients.Where(i => searchQuery.Any(s => i.Name.Contains(s)));
            IQueryable<IGrouping<int, RecipeIngredient>> recipeIngredient = MainWindow.db.RecipeIngredients.Where(ri => recipes.Any(r => r.ID == ri.RecipeID) || ingredients.Any(i => i.ID == ri.IngredientID)).GroupBy(ri => ri.RecipeID);
            List<Recipe> allRecipes = MainWindow.db.Recipes.Where(r => r.ID != -1).ToList();

            List<SearchResults> results = new List<SearchResults>();

            foreach (IGrouping<int, RecipeIngredient> recipeGroup in recipeIngredient)
            {
                SearchResults recipeResult = new SearchResults(allRecipes.Where(ar => ar.ID == recipeGroup.FirstOrDefault().RecipeID).FirstOrDefault());

                foreach (RecipeIngredient ingredient in recipeGroup)
                {
                    if (inventoryList.Where(il => il.IngredientID == ingredient.IngredientID).Count() != 0)
                    {
                        if (inventoryList.Where(il => il.IngredientID == ingredient.IngredientID).Sum(iq => iq.Quantity) >= ingredient.Quantity)
                        {
                            recipeResult.fullMatch++;
                        }
                        else
                        {
                            recipeResult.partialMatch++;
                        }
                    }
                }

                results.Add(recipeResult);
            }

            MessageBox.Show((DateTime.Now - test).TotalMilliseconds.ToString());

            listResults.ItemsSource = results.OrderByDescending(x => x.fullMatch).ThenByDescending(x => x.partialMatch).ThenByDescending(x => x.keyWordMatch).ThenBy(x => x.Recipe.Title);
        }

        private void listResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var showRecipe = new ShowRecipe(((SearchResults)listResults.SelectedItem).Recipe);
            showRecipe.ShowDialog();
        }
    }
}
