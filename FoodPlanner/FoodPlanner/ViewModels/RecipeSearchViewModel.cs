using FoodPlanner.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FoodPlanner.ViewModels
{
    public class RecipeSearchViewModel
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

        private List<int> blacklistedRecipes = (from bl in App.db.BlacklistIngredients
                                                join ri in App.db.RecipeIngredients on bl.IngredientID equals ri.IngredientID
                                                select ri.RecipeID).ToList();

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
                SearchRecipes(_searchText);
            }

        }

        private void SearchRecipes(string query)
        {
            listOfSearchResults.Clear();

            List<string> searchQuery = query.Split(',').Select(s => s.Trim()).ToList();

            IQueryable<int> recipeIDs = (from ri in App.db.RecipeIngredients
                                         join i in App.db.Ingredients on ri.IngredientID equals i.ID
                                         join r in App.db.Recipes on ri.RecipeID equals r.ID
                                         where searchQuery.Any(s => i.Name.Contains(s)) || searchQuery.Any(s => r.Title.Contains(s))
                                         group ri by ri.RecipeID into searchRecipeID
                                         select searchRecipeID.FirstOrDefault().RecipeID);

            IQueryable<IGrouping<int, Result>> recipeIngredients = from ri in App.db.RecipeIngredients
                                                                   join i in App.db.Ingredients on ri.IngredientID equals i.ID
                                                                   join r in App.db.Recipes on ri.RecipeID equals r.ID
                                                                   where recipeIDs.Any(rid => rid == ri.RecipeID && blacklistedRecipes.Where(bl => bl == rid).Count() == 0)
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

                }

                listOfSearchResults.Add(result);
            }
        }

        public ObservableCollection<SearchResults> listOfSearchResults { get; private set; }

    }
}
