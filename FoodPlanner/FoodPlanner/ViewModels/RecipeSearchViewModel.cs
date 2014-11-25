using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FoodPlanner.Models;
using MvvmFoundation.Wpf;

namespace FoodPlanner.ViewModels
{
    public class RecipeSearchViewModel : ObservableObject
    {
        public RecipeSearchViewModel()
        {
            this.listOfSearchResults = new ObservableCollection<SearchResults>();
        }

        private string _searchText;
        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                _searchText = value;
                SearchRecipes();
            }

        }

        private void SearchRecipes()
        {
            listOfSearchResults.Clear();
            List<string> searchQuery = _searchText.Split(',').Select(s => s.Trim()).ToList();

            PublicQuerys publicQuerys = new PublicQuerys();

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

            List<SearchResults> orderedListOfSearchResults = publicQuerys.addValuesToSearch(groupedRecipes, searchQuery).OrderByDescending(res => res.fullMatch)
                                                                                                                        .ThenByDescending(res => res.partialMatch)
                                                                                                                        .ThenByDescending(res => res.getRating)
                                                                                                                        .ThenByDescending(res => res.prevIngredients)
                                                                                                                        .ThenByDescending(res => res.recipe.Title)
                                                                                                                        .ToList();

            foreach (SearchResults sr in orderedListOfSearchResults)
            {
                listOfSearchResults.Add(sr);
            }
        }

        public ObservableCollection<SearchResults> listOfSearchResults { get; private set; }

    }
}
