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
            //set the listOfSearchResults to be an empty list
            this.listOfSearchResults = new ObservableCollection<SearchResults>();

            //initialize the publicQuerys so it can be used
            PublicQuerys publicQuerys = new PublicQuerys();

            //get some recipes based on earlier meals
            IQueryable<IGrouping<int, Result>> groupedRecipes = publicQuerys.search((from r in App.db.Meals where r.UserID == App.CurrentUser.ID && r.Date <= DateTime.Now select r.RecipeID).ToList());

            //add values and order the list by useing the addValuesToSearch method in the publicQuerys.
            ObservableCollection<SearchResults> orderedListOfSearchResults = publicQuerys.addValuesToSearch(groupedRecipes);

            //set list and use the RaisePropertyChanged to ensure that list view updates
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
            //split the userinput into an list of strings and strip unnecessary spaces at start and end
            List<string> searchQuery = _searchText.Split(',').Select(s => s.Trim()).ToList();

            //initialize the publicQuerys so it can be used
            PublicQuerys publicQuerys = new PublicQuerys();

            //get the recipes where the name of the ingredient contains a keyword, but only select the ID of the recipe
            List<int> recipeIDByIngredients = (from ri in App.db.RecipeIngredients
                                               join i in App.db.Ingredients on ri.IngredientID equals i.ID
                                               where searchQuery.Any(s => i.Name.Contains(s))
                                               select ri.RecipeID).ToList();

            //get the recipes where the name of the recipe contains a keyword, but only select the ID of the recipe
            List<int> recipeIDByRecipeName = (from ri in App.db.RecipeIngredients
                                              join r in App.db.Recipes on ri.RecipeID equals r.ID
                                              where searchQuery.Any(s => r.Title.Contains(s))
                                              select ri.RecipeID).ToList();

            //add the two lists together to form one list of all the recipe ID's
            recipeIDByIngredients.AddRange(recipeIDByRecipeName);

            //make the list unique, and remove the blacklisted recipes from the list.
            List<int> recipeIDs = recipeIDByIngredients.Distinct().Except(publicQuerys.blackList).ToList();

            //search for the found recipes
            IQueryable<IGrouping<int, Result>> groupedRecipes = publicQuerys.search(recipeIDs);

            //then add value and order them
            ObservableCollection<SearchResults> orderedListOfSearchResults = publicQuerys.addValuesToSearch(groupedRecipes, searchQuery);

            //set list and use the RaisePropertyChanged to ensure that list view updates
            listOfSearchResults = orderedListOfSearchResults;
            RaisePropertyChanged("listOfSearchResults");
        }

        //initialize the ObservableCollection that is to be shown in the view
        public ObservableCollection<SearchResults> listOfSearchResults { get; private set; }

    }
}
