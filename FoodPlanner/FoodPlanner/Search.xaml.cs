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
            IQueryable<IGrouping<int,RecipeIngredient>> recipeIngredient = MainWindow.db.RecipeIngredients.Where(ri => recipes.Any(r => r.ID == ri.Recipe.ID) || ingredients.Any(i => i.ID == ri.Ingredient.ID)).GroupBy(ri => ri.RecipeID);

            MessageBox.Show((DateTime.Now - test).TotalMilliseconds.ToString());
            /*
            MessageBox.Show(recipes.Count().ToString());
            MessageBox.Show(ingredients.Count().ToString());
            MessageBox.Show(recipeIngredient.Count().ToString());
            */

            listResults.ItemsSource = recipeIngredient.ToList();




            /*
            try
            {
                IQueryable<IGrouping<int,SearchResults>> recipeIngredient = (from rec in MainWindow.db.Recipes
                                                              join ri in MainWindow.db.RecipeIngredients on rec.ID equals ri.RecipeID
                                                              join ing in MainWindow.db.Ingredients on ri.IngredientID equals ing.ID
                                                              where searchQuery.Any(s => ing.Name.Contains(s)) || searchQuery.Any(s => rec.Title.Contains(s))
                                                              select new SearchResults()
                                                              {
                                                                  RecipeID = ri.RecipeID,
                                                                  Recipe = ri.Recipe,
                                                                  RecipeQuantity = ri.Quantity,
                                                                  InventoryQuantity = ((from ii in MainWindow.db.InventoryIngredients where ii.IngredientID == ing.ID && ii.UserID == MainWindow.CurrentUser.ID select ii).Count() > 0 ? (from ii in MainWindow.db.InventoryIngredients where ii.IngredientID == ing.ID && ii.UserID == MainWindow.CurrentUser.ID select ii.Quantity).FirstOrDefault() : -1),
                                                                  IngredientCount = ri.Recipe.RecipeIngredients.Count()
                                                              } into c
                                                              group c by c.RecipeID into g
                                                              select g);

                listResults.ItemsSource = recipeIngredient.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }

            List<SearchResults> searchResults = new List<SearchResults>();

            List<string> searchQuery = searchBox.Text.Split(',').Select(s => s.Trim()).ToList();

            List<RecipeIngredient> recipeIngredient = (from rec in MainWindow.db.Recipes join ri in MainWindow.db.RecipeIngredients on rec.ID equals ri.RecipeID join ing in MainWindow.db.Ingredients on ri.IngredientID equals ing.ID where searchQuery.Any(s => ing.Name.Contains(s)) || searchQuery.Any(s => rec.Title.Contains(s)) select ri).Take(50).ToList();

            foreach (RecipeIngredient ri in recipeIngredient)
            {
                if (searchResults.Where(x => x.recipe.Title == ri.Recipe.Title).Count() == 0)
                {
                    searchResults.Add(new SearchResults(ri.Recipe, searchQuery.Any(s => ri.Ingredient.Name.Contains(s)), searchQuery.Any(s => ri.Recipe.Title.Contains(s))));
                }
                else
                {
                    if (    ) // kan eventuelt optimeres
                    {
                        searchResults.Where(x => x.recipe.Title == ri.Recipe.Title).Single().match++;
                    }
                }

                if (inventoryList.Where(ii => ii.IngredientID == ri.IngredientID).Count() == 1)
                {
                    if (inventoryList.Where(ii => ii.IngredientID == ri.IngredientID).First().Quantity >= ri.Quantity)
                    {
                        searchResults.Where(x => x.recipe.Title == ri.Recipe.Title).Single().fullMatch++;
                    }
                    else
                    {
                        searchResults.Where(x => x.recipe.Title == ri.Recipe.Title).Single().partialMatch++;
                    }
                }
            }

            searchList.ItemsSource = searchResults.OrderByDescending(x => x.fullMatch).ThenByDescending(x => x.partialMatch).ThenByDescending(x => x.match).ThenBy(x => x.recipe.Title);
            */
        }

        private void searchList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
            var showRecipe = new ShowRecipe(((SearchResults)searchList.SelectedItem).recipe);
            showRecipe.ShowDialog();
            */
        }
    }
}
