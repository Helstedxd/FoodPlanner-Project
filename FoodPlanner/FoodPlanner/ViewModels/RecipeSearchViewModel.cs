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

            PublicQuerys publicQuerys = new PublicQuerys();

            IQueryable<IGrouping<int, Result>> groupedRecipes = publicQuerys.search((from r in App.db.Meals where r.UserID == App.CurrentUser.ID && r.Date <= DateTime.Now select r.RecipeID).ToList());

            ObservableCollection<SearchResults> orderedListOfSearchResults = publicQuerys.addValuesToSearch(groupedRecipes);

            listOfSearchResults = orderedListOfSearchResults;
            RaisePropertyChanged("listOfSearchResults");
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

            ObservableCollection<SearchResults> orderedListOfSearchResults = publicQuerys.addValuesToSearch(groupedRecipes, searchQuery);

            listOfSearchResults = orderedListOfSearchResults;
            RaisePropertyChanged("listOfSearchResults");
        }

        public ObservableCollection<SearchResults> listOfSearchResults { get; private set; }

    }
}
