using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner.Models
{
    class PublicQuerys
    {
        public PublicQuerys() { }

        public List<inventoryListGroupedByQuantity> inventoryList = (from ii in App.db.InventoryIngredients
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

        public List<LastMeal> lastMeals = (from meals in App.db.Meals
                                           join ri in App.db.RecipeIngredients on meals.RecipeID equals ri.RecipeID
                                           join i in App.db.Ingredients on ri.IngredientID equals i.ID
                                           where meals.UserID == App.CurrentUser.ID && meals.Date <= DateTime.Now
                                           group i by i.ID into igrouped
                                           select new LastMeal()
                                           {
                                               ingredientID = igrouped.FirstOrDefault().ID,
                                               ingredientCount = igrouped.Count()
                                           }).ToList();

        public List<int> blackList = (from bl in App.db.BlacklistIngredients
                                      join ri in App.db.RecipeIngredients on bl.IngredientID equals ri.IngredientID
                                      where bl.UserID == App.CurrentUser.ID
                                      select ri.RecipeID).ToList();

        public List<GrayList> grayList = (from gl in App.db.GraylistIngredients
                                          join i in App.db.Ingredients on gl.IngredientID equals i.ID
                                          where gl.UserID == App.CurrentUser.ID
                                          select new GrayList()
                                          {
                                              ingredient = i,
                                              rating = gl.IngredientValue
                                          }).ToList();

        public IQueryable<IGrouping<int, Result>> search(List<int> recipeIDs)
        {
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

            return recipeIngredients;
        }

        public List<SearchResults> addValuesToSearch(IQueryable<IGrouping<int, Result>> userInput, List<string> searchKeywords = null)
        {
            List<SearchResults> result = new List<SearchResults>();

            foreach (IGrouping<int, Result> ri in userInput)
            {
                Recipe recipe = ri.FirstOrDefault().recipe;

                SearchResults searchResult = new SearchResults(recipe);

                if (searchKeywords != null)
                {
                    if (searchKeywords.Any(s => recipe.Title.ToLower().Contains(s.ToLower())))
                    {
                        searchResult.keyWordMatch++;
                    }
                }


                foreach (Result res in ri)
                {
                    searchResult.addIngredient(res.ingredient);

                    if (inventoryList.Where(il => il.IngredientID == res.ingredient.ID).Count() != 0)
                    {
                        if (inventoryList.Where(il => il.IngredientID == res.ingredient.ID).First().Quantity >= res.quantity)
                        {
                            searchResult.fullMatch++;
                        }
                        else
                        {
                            searchResult.partialMatch += inventoryList.Where(il => il.IngredientID == res.ingredient.ID).First().Quantity / res.quantity;
                        }
                    }

                    if (searchKeywords != null)
                    {
                        if (searchKeywords.Any(s => res.ingredient.Name.ToLower().Contains(s.ToLower())))
                        {
                            searchResult.keyWordMatch++;
                        }
                    }

                    if (lastMeals.Where(iflm => iflm.ingredientID == res.ingredient.ID).Count() != 0)
                    {
                        searchResult.prevIngredients += lastMeals.Where(iflm => iflm.ingredientID == res.ingredient.ID).Single().ingredientCount;
                    }

                    if (grayList.Where(gl => res.ingredient.ID == gl.ingredient.ID).Count() != 0)
                    {
                        searchResult.setRating = grayList.Where(gl => res.ingredient.ID == gl.ingredient.ID).Single().rating;
                    }
                    else
                    {
                        //50 is the default value of nonrated items
                        searchResult.setRating = 50;
                    }
                }

                result.Add(searchResult);
            }


            return result;
        }
    }
}
