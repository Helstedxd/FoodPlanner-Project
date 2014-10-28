﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : Window
    {
        public Search()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource recipeViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("recipeViewSource")));
            recipeViewSource.Source = MainWindow.db.Recipes.ToList();
        }

        private void startSearch_Click(object sender, RoutedEventArgs e)
        {
            List<SearchResults> searchResults = new List<SearchResults>();
            List<InventoryIngredient> tmpinv = MainWindow.db.InventoryIngredients.Where(ii => ii.UserID == MainWindow.CurrentUser.ID).ToList();
            // && ii.IngredientID == ri.IngredientID

            List<string> searchQuery = searchBox.Text.Split(',').Select(s => s.Trim()).ToList();

            List<RecipeIngredient> recipeIngredient = (from rec in MainWindow.db.Recipes join ri in MainWindow.db.RecipeIngredients on rec.ID equals ri.RecipeID join ing in MainWindow.db.Ingredients on ri.IngredientID equals ing.ID where searchQuery.Any(s => ing.Name.Contains(s)) || searchQuery.Any(s => rec.Title.Contains(s)) select ri).ToList();

            foreach (RecipeIngredient ri in recipeIngredient)
            {
                if (searchResults.Where(x => x.recipe.Title == ri.Recipe.Title).Count() == 0)
                {
                    searchResults.Add(new SearchResults(ri.Recipe, searchQuery.Any(s => ri.Ingredient.Name.Contains(s)), searchQuery.Any(s => ri.Recipe.Title.Contains(s))));
                }
                else
                {
                    if (searchQuery.Any(s => ri.Ingredient.Name.Contains(s)))
                    {
                        searchResults.Where(x => x.recipe.Title == ri.Recipe.Title).Single().match++;
                    }
                }

                if (tmpinv.Where(ii => ii.IngredientID == ri.IngredientID).Count() == 1)
                {
                    if (tmpinv.Where(ii => ii.IngredientID == ri.IngredientID).First().Quantity >= ri.Quantity)
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
        }

    }
}
