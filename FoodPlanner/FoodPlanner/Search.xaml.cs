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
            #region NotYet
            /*
            List<int> itemsUsedBefore = StaticLists.ingredientFromLatestMeals.OrderByDescending(iflm => iflm.ingredientCount).Take(10).Select(i => i.ingredientID).ToList();

            IQueryable<IGrouping<int, Result>> recipeIngredients = from ri in App.db.RecipeIngredients
                                                                   join i in App.db.Ingredients on ri.IngredientID equals i.ID
                                                                   join r in App.db.Recipes on ri.RecipeID equals r.ID
                                                                   where itemsUsedBefore.Any(iflm => iflm == ri.IngredientID)
                                                                   group new Result()
                                                                   {
                                                                       recipe = ri.Recipe,
                                                                       ingredient = ri.Ingredient,
                                                                       quantity = ri.Quantity
                                                                   } by ri.RecipeID;

            List<SearchResults> results = new List<SearchResults>();

            foreach (IGrouping<int, Result> ri in recipeIngredients)
            {
                Recipe recipe = ri.FirstOrDefault().recipe;

                SearchResults result = new SearchResults(recipe);


                foreach (Result res in ri)
                {
                    result.addIngredient(res.ingredient);

                    if (StaticLists.inventoryList.Where(il => il.IngredientID == res.ingredient.ID).Count() != 0)
                    {
                        if (StaticLists.inventoryList.Where(il => il.IngredientID == res.ingredient.ID).First().Quantity >= res.quantity)
                        {
                            result.fullMatch++;
                        }
                        else
                        {
                            result.partialMatch++;
                        }
                    }

                    if (StaticLists.ingredientFromLatestMeals.Where(iflm => iflm.ingredientID == res.ingredient.ID).Count() != 0)
                    {
                        result.prevIngredients += StaticLists.ingredientFromLatestMeals.Where(iflm => iflm.ingredientID == res.ingredient.ID).Single().ingredientCount;
                    }

                    if (StaticLists.grayList.Where(gl => res.ingredient.ID == gl.ingredient.ID).Count() != 0)
                    {
                        result.setRating = StaticLists.grayList.Where(gl => res.ingredient.ID == gl.ingredient.ID).Single().rating;
                    }
                    else
                    {
                        //50 is the default value of nonrated items
                        result.setRating = 50;
                    }
                }

                results.Add(result);
            }

            listResults.ItemsSource = results
                          .OrderByDescending(res => res.fullMatch)
                          .ThenByDescending(res => res.partialMatch)
                          .ThenByDescending(res => res.getRating)
                          .ThenByDescending(res => res.prevIngredients)
                          .ThenByDescending(res => res.recipe.Title);
            */
            #endregion
        }

        private void startSearch_Click(object sender, RoutedEventArgs e)
        {
            List<string> searchQuery = searchBox.Text.Split(',').Select(s => s.Trim()).ToList();
            List<SearchResults> results = new List<SearchResults>();



            listResults.ItemsSource = StaticLists.blackList;
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
