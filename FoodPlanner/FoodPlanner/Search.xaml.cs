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
            List<string> searchQuery = searchBox.Text.Split(',').Select(s => s.Trim()).ToList();

            try
            {
                List<SearchResults2> results = new List<SearchResults2>();

                List<IGrouping<int, RecipeIngredient>> test = (from ri in App.db.RecipeIngredients
                                                               join i in App.db.Ingredients on ri.IngredientID equals i.ID
                                                               join r in App.db.Recipes on ri.RecipeID equals r.ID
                                                               where searchQuery.Any(s => r.Title.Contains(s)) || searchQuery.Any(s => i.Name.Contains(s))
                                                               group ri by ri.RecipeID into rig
                                                               select rig).ToList();
                //select new { recipe = rig.FirstOrDefault().Recipe, ingredients = string.Join(", ", rig) });

                foreach (IGrouping<int, RecipeIngredient> q1 in test)
                {
                    SearchResults2 result = new SearchResults2(q1.FirstOrDefault().Recipe);

                    DateTime now = DateTime.Now;

                    foreach (RecipeIngredient ri in q1)
                    {
                        result.addIngredient(ri.Ingredient);
                    }

                    MessageBox.Show((DateTime.Now - now).Milliseconds.ToString() + ", " + result.numIngredients.ToString());

                    results.Add(result);
                }

                listResults.ItemsSource = results;

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            /*
            MessageBox.Show(test.Count().ToString());

            listResults.ItemsSource = test.ToList();
            */



            //List<Ingredient> test = MainWindow.db.Ingredients.Where(i => i.ID != -1).ToList();
            //List<Recipe> test2 = MainWindow.db.Recipes.Where(r => r.ID != -1).ToList();
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
