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


        public Search()
        {
            InitializeComponent();
        }

        private void getRecommend_Click(object sender, RoutedEventArgs e)
        {
            PublicQuerys publicQuerys = new PublicQuerys();

            IQueryable<IGrouping<int, Result>> groupedRecipes = publicQuerys.search((from r in App.db.Meals where r.UserID == App.CurrentUser.ID && r.Date <= DateTime.Now select r.RecipeID).ToList());

            List<SearchResults> orderedListOfSearchResults = publicQuerys.addValuesToSearch(groupedRecipes);

            listResults.ItemsSource = orderedListOfSearchResults.OrderByDescending(res => res.fullMatch)
                                                                .ThenByDescending(res => res.partialMatch)
                                                                .ThenByDescending(res => res.getRating)
                                                                .ThenByDescending(res => res.prevIngredients)
                                                                .ThenByDescending(res => res.recipe.Title)
                                                                .ToList();

        }

        private void startSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(searchBox.Text))
            {
                PublicQuerys publicQuerys = new PublicQuerys();

                List<string> searchQuery = searchBox.Text.Split(',').Select(s => s.Trim()).ToList();

                List<int> recipeIDByIngredients = (from ri in App.db.RecipeIngredients
                                                   join i in App.db.Ingredients on ri.IngredientID equals i.ID
                                                   where searchQuery.Any(s => i.Name.Contains(s))
                                                   select ri.RecipeID).ToList();

                List<int> recipeIDByRecipeName = (from ri in App.db.RecipeIngredients
                                                  join r in App.db.Recipes on ri.RecipeID equals r.ID
                                                  where searchQuery.Any(s => r.Title.Contains(s))
                                                  select ri.RecipeID).ToList();

                recipeIDByIngredients.AddRange(recipeIDByRecipeName);

                List<int> recipeIDs = recipeIDByIngredients.Distinct().Except(publicQuerys.blackList).ToList();

                IQueryable<IGrouping<int, Result>> groupedRecipes = publicQuerys.search(recipeIDs);

                List<SearchResults> orderedListOfSearchResults = publicQuerys.addValuesToSearch(groupedRecipes, searchQuery);

                listResults.ItemsSource = orderedListOfSearchResults.OrderByDescending(res => res.fullMatch)
                                                                    .ThenByDescending(res => res.partialMatch)
                                                                    .ThenByDescending(res => res.getRating)
                                                                    .ThenByDescending(res => res.prevIngredients)
                                                                    .ThenByDescending(res => res.recipe.Title)
                                                                    .ToList();
            }
        }

        private void listResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var showRecipe = new ShowRecipe(((SearchResults)listResults.SelectedItem).recipe);
                showRecipe.ShowDialog();
            }

            catch (Exception ex) { }
        }

    }
}
