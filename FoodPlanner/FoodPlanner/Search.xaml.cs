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
        private List<inventoryListCombinedByQuantity> inventoryList = (from ii in App.db.InventoryIngredients
                                                                       where ii.UserID == App.CurrentUser.ID
                                                                       group ii by ii.IngredientID into iig
                                                                       select new inventoryListCombinedByQuantity()
                                                                       {
                                                                           IngredientID = iig.FirstOrDefault().IngredientID,
                                                                           Quantity = iig.Sum(i => i.Quantity),
                                                                           Ingredient = iig.FirstOrDefault().Ingredient,
                                                                           ExpirationDate = iig.FirstOrDefault().ExpirationDate,
                                                                           PurchaseDate = iig.FirstOrDefault().PurchaseDate,
                                                                           User = iig.FirstOrDefault().User,
                                                                           UserID = iig.FirstOrDefault().UserID
                                                                       }).ToList();


        public Search()
        {
            InitializeComponent();
        }

        private void startSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> searchQuery = searchBox.Text.Split(',').Select(s => s.Trim()).ToList();
                List<SearchResults2> results = new List<SearchResults2>();

                DateTime now = DateTime.Now;

                IQueryable<int> recipeIDs = (from ri in App.db.RecipeIngredients
                                             join i in App.db.Ingredients on ri.IngredientID equals i.ID
                                             join r in App.db.Recipes on ri.RecipeID equals r.ID
                                             where searchQuery.Any(s => i.Name.Contains(s)) || searchQuery.Any(s => r.Title.Contains(s))
                                             group ri by ri.RecipeID into rofl
                                             select rofl.FirstOrDefault().RecipeID);

                IQueryable<IGrouping<int, Result>> recipeIngredients = from ri in App.db.RecipeIngredients
                                                                       join i in App.db.Ingredients on ri.IngredientID equals i.ID
                                                                       join r in App.db.Recipes on ri.RecipeID equals r.ID
                                                                       where recipeIDs.Any(rid => rid == ri.RecipeID)
                                                                       group new Result()
                                                                       {
                                                                           recipe = ri.Recipe,
                                                                           ingredient = ri.Ingredient,
                                                                           quantity = ri.Quantity
                                                                       } by ri.RecipeID;

                foreach (IGrouping<int, Result> ri in recipeIngredients)
                {
                    Recipe recipe = ri.FirstOrDefault().recipe;

                    SearchResults2 result = new SearchResults2(recipe);

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

                    }

                    results.Add(result);
                }

                listResults.ItemsSource = results.OrderByDescending(res => res.fullMatch).ThenByDescending(res => res.partialMatch).ThenByDescending(res => res.keyWordMatch).ThenByDescending(res => res.recipe.Title).Take(150);

                MessageBox.Show("Our super optimized search algorithm took only " + (DateTime.Parse((DateTime.Now - now).ToString()).ToString("ss.fff")) + "s to process your request, for a total of " + results.Count() + " results", "Awsome", MessageBoxButton.YesNoCancel, MessageBoxImage.Hand);
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
                var showRecipe = new ShowRecipe(((SearchResults2)listResults.SelectedItem).recipe);
                showRecipe.ShowDialog();
            }

            catch (Exception ex) { }
        }
    }
}
