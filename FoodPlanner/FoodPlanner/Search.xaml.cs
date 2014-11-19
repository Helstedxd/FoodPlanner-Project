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
        private List<inventoryListGroupedByQuantity> inventoryList = (from ii in App.db.InventoryIngredients
                                                                      where ii.UserID == App.CurrentUser.ID
                                                                      group ii by ii.IngredientID into iig
                                                                      select new inventoryListGroupedByQuantity()
                                                                      {
                                                                          IngredientID = iig.FirstOrDefault().IngredientID,
                                                                          Quantity = iig.Sum(i => i.Quantity),
                                                                          Ingredient = iig.FirstOrDefault().Ingredient,
                                                                          ExpirationDate = iig.FirstOrDefault().ExpirationDate,
                                                                          PurchaseDate = iig.FirstOrDefault().PurchaseDate,
                                                                          User = iig.FirstOrDefault().User,
                                                                          UserID = iig.FirstOrDefault().UserID
                                                                      }).ToList();

        private static IQueryable<int> lastMeals = (from meals in App.db.Meals
                                                    where meals.UserID == App.CurrentUser.ID && meals.Date <= DateTime.Now
                                                    orderby meals.ID descending
                                                    select meals.RecipeID).Take(5);

        private List<LastMeal> ingredientFromLatestMeals = (from i in App.db.Ingredients
                                                            join ri in App.db.RecipeIngredients on i.ID equals ri.IngredientID
                                                            where lastMeals.Any(lm => lm == ri.RecipeID)
                                                            group i by i.ID into igrouped
                                                            select new LastMeal()
                                                            {
                                                                ingredientID = igrouped.FirstOrDefault().ID,
                                                                ingredientCount = igrouped.Count()
                                                            }).ToList();

        private List<Recipe> blackList = (from bl in App.db.BlacklistIngredients
                                          join ri in App.db.RecipeIngredients on bl.IngredientID equals ri.IngredientID
                                          where bl.UserID == App.CurrentUser.ID
                                          select ri.Recipe).ToList();

        private List<GrayList> grayList = (from gl in App.db.GraylistIngredients
                                           join i in App.db.Ingredients on gl.IngredientID equals i.ID
                                           where gl.UserID == App.CurrentUser.ID
                                           select new GrayList()
                                           {
                                               ingredient = i,
                                               rating = gl.IngredientValue
                                           }).ToList();

        public Search()
        {
            InitializeComponent();
        }

        private void getRecommend_Click(object sender, RoutedEventArgs e)
        {
            List<int> itemsUsedBefore = ingredientFromLatestMeals.OrderByDescending(iflm => iflm.ingredientCount).Take(10).Select(i => i.ingredientID).ToList();

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

                    if (inventoryList.Where(il => il.IngredientID == res.ingredient.ID).Count() != 0)
                    {
                        if (inventoryList.Where(il => il.IngredientID == res.ingredient.ID).First().Quantity >= res.quantity)
                        {
                            result.fullMatch++;
                        }
                        else
                        {
                            result.partialMatch++;
                        }
                    }

                    if (ingredientFromLatestMeals.Where(iflm => iflm.ingredientID == res.ingredient.ID).Count() != 0)
                    {
                        result.prevIngredients += ingredientFromLatestMeals.Where(iflm => iflm.ingredientID == res.ingredient.ID).Single().ingredientCount;
                    }

                    if (grayList.Where(gl => res.ingredient.ID == gl.ingredient.ID).Count() != 0)
                    {
                        result.setRating = grayList.Where(gl => res.ingredient.ID == gl.ingredient.ID).Single().rating;
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

        }

        private void startSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> searchQuery = searchBox.Text.Split(',').Select(s => s.Trim()).ToList();
                List<SearchResults> results = new List<SearchResults>();

                IQueryable<int> recipeIDs = (from ri in App.db.RecipeIngredients
                                             join i in App.db.Ingredients on ri.IngredientID equals i.ID
                                             join r in App.db.Recipes on ri.RecipeID equals r.ID
                                             where searchQuery.Any(s => i.Name.Contains(s)) || searchQuery.Any(s => r.Title.Contains(s))
                                             group ri by ri.RecipeID into searchRecipeID
                                             select searchRecipeID.FirstOrDefault().RecipeID);

                IQueryable<IGrouping<int, Result>> recipeIngredients = from ri in App.db.RecipeIngredients
                                                                       join i in App.db.Ingredients on ri.IngredientID equals i.ID
                                                                       join r in App.db.Recipes on ri.RecipeID equals r.ID
                                                                       where recipeIDs.Any(rid => rid == ri.RecipeID && blackList.Where(bl => bl.ID == rid).Count() == 0)
                                                                       group new Result()
                                                                       {
                                                                           recipe = ri.Recipe,
                                                                           ingredient = ri.Ingredient,
                                                                           quantity = ri.Quantity
                                                                       } by ri.RecipeID;


                foreach (IGrouping<int, Result> ri in recipeIngredients)
                {
                    Recipe recipe = ri.FirstOrDefault().recipe;

                    SearchResults result = new SearchResults(recipe);

                    if (searchQuery.Any(s => recipe.Title.ToLower().Contains(s.ToLower())))
                    {
                        result.keyWordMatch++;
                    }


                    foreach (Result res in ri)
                    {
                        result.addIngredient(res.ingredient);

                        if (inventoryList.Where(il => il.IngredientID == res.ingredient.ID).Count() != 0)
                        {
                            if (inventoryList.Where(il => il.IngredientID == res.ingredient.ID).First().Quantity >= res.quantity)
                            {
                                result.fullMatch++;
                            }
                            else
                            {
                                result.partialMatch++;
                            }
                        }

                        if (searchQuery.Any(s => res.ingredient.Name.ToLower().Contains(s.ToLower())))
                        {
                            result.keyWordMatch++;
                        }

                        if (ingredientFromLatestMeals.Where(iflm => iflm.ingredientID == res.ingredient.ID).Count() != 0)
                        {
                            result.prevIngredients += ingredientFromLatestMeals.Where(iflm => iflm.ingredientID == res.ingredient.ID).Single().ingredientCount;
                        }

                        if (grayList.Where(gl => res.ingredient.ID == gl.ingredient.ID).Count() != 0)
                        {
                            result.setRating = grayList.Where(gl => res.ingredient.ID == gl.ingredient.ID).Single().rating;
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
                                          .ThenByDescending(res => res.keyWordMatch)
                                          .ThenByDescending(res => res.getRating)
                                          .ThenByDescending(res => res.prevIngredients)
                                          .ThenByDescending(res => res.recipe.Title);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
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
