using FoodPlanner.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner.ViewModels
{
    class RecipesViewModel
    {
        private List<InventoryIngredient> inventoryList = MainWindow.db.InventoryIngredients.Where(ii => ii.UserID == MainWindow.CurrentUser.ID).ToList();

        public RecipesViewModel()
        {
            this.listOfSearchResults = new ObservableCollection<SearchResults>();
        }

        public string SearchDefine
        {
            set
            {
                listOfSearchResults.Clear();

                List<string> searchQuery = value.Split(',').Select(s => s.Trim()).ToList();

                IQueryable<Recipe> recipes = MainWindow.db.Recipes.Where(r => searchQuery.Any(s => r.Title.Contains(s)));
                IQueryable<Ingredient> ingredients = MainWindow.db.Ingredients.Where(i => searchQuery.Any(s => i.Name.Contains(s)));
                IQueryable<IGrouping<int, RecipeIngredient>> recipeIngredient = MainWindow.db.RecipeIngredients.Where(ri => recipes.Any(r => r.ID == ri.RecipeID) || ingredients.Any(i => i.ID == ri.IngredientID)).GroupBy(ri => ri.RecipeID);

                List<Recipe> allRecipes = MainWindow.db.Recipes.ToList();
                List<Ingredient> ingredientsList = ingredients.ToList();

                foreach (IGrouping<int, RecipeIngredient> recipeGroup in recipeIngredient)
                {
                    SearchResults recipeResult = new SearchResults(allRecipes.Where(ar => ar.ID == recipeGroup.FirstOrDefault().RecipeID).FirstOrDefault(), recipeGroup.Count());

                    if (searchQuery.Any(s => recipeResult.recipe.Title.Contains(s)))
                    {
                        recipeResult.keyWordMatch++;
                    }

                    foreach (RecipeIngredient ingredient in recipeGroup)
                    {
                        if (ingredientsList.Any(iID => iID.ID == ingredient.IngredientID))
                        {
                            recipeResult.keyWordMatch++;
                        }

                        if (inventoryList.Where(il => il.IngredientID == ingredient.IngredientID).Count() != 0)
                        {
                            if (inventoryList.Where(il => il.IngredientID == ingredient.IngredientID).Sum(iq => iq.Quantity) >= ingredient.Quantity)
                            {
                                recipeResult.fullMatch++;
                            }
                            else
                            {
                                recipeResult.partialMatch++;
                            }
                        }
                    }

                    listOfSearchResults.Add(recipeResult);
                }


                List<Recipe> test2 = MainWindow.db.Recipes.Where(r => r.Title.Contains(value)).Take(20).ToList();

                foreach (Recipe r in test2)
                {
                    listOfSearchResults.Add(new SearchResults(r, 1) { fullMatch = 0, partialMatch = 0, keyWordMatch = 0 });
                }
            }
        }

        /*
        public string ImageCache
        {
            get
            {
                WebClient client = new WebClient();
                string path = Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString() + "/imageCache";

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                if (!File.Exists(path + "/" + _recipe.ID + ".jpg"))
                {
                    client.DownloadFileAsync(new Uri(recipe.Image), path + "/" + _recipe.ID + ".jpg");
                    return recipe.Image;
                }
                else
                {
                    return path + "/" + _recipe.ID + ".jpg";
                }
            }
        }
        */


        public ObservableCollection<SearchResults> listOfSearchResults{ get; private set; }

    }
}
