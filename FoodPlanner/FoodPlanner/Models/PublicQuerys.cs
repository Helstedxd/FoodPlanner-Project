using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner.Models
{
    class PublicQuerys
    {
        #region Constructor
        public PublicQuerys() { }
        #endregion

            #region Fields
        //A query that if multiple of the same ingredient is found in the users inventory, combines the sum into a single item, and combine the quantities onto one.
        public IQueryable<inventoryListGroupedByQuantity> inventoryIQueryable = from ii in App.db.InventoryIngredients
                                                                                join i in App.db.Ingredients on ii.IngredientID equals i.ID
                                                                                where ii.UserID == App.CurrentUser.ID
                                                                                group ii by ii.IngredientID into iig
                                                                                select new inventoryListGroupedByQuantity()
                                                                                {
                                                                                    IngredientID = iig.FirstOrDefault().IngredientID,
                                                                                    Unit = iig.FirstOrDefault().Ingredient.Unit,
                                                                                    Quantity = iig.Sum(i => i.Quantity),
                                                                                    Ingredient = iig.FirstOrDefault().Ingredient,
                                                                                    ExpirationDate = iig.FirstOrDefault().ExpirationDate,
                                                                                    PurchaseDate = iig.FirstOrDefault().PurchaseDate,
                                                                                    User = iig.FirstOrDefault().User,
                                                                                    UserID = iig.FirstOrDefault().UserID
                                                                                };

        //Same as last query execpt it's a list not a IQueryable.
        public List<inventoryListGroupedByQuantity> inventoryList = (from ii in App.db.InventoryIngredients
                                                                     join i in App.db.Ingredients on ii.IngredientID equals i.ID
                                                                     where ii.UserID == App.CurrentUser.ID
                                                                     group ii by ii.IngredientID into iig
                                                                     select new inventoryListGroupedByQuantity()
                                                                     {
                                                                         IngredientID = iig.FirstOrDefault().IngredientID,
                                                                         Unit = iig.FirstOrDefault().Ingredient.Unit,
                                                                         Quantity = iig.Sum(i => i.Quantity),
                                                                         Ingredient = iig.FirstOrDefault().Ingredient,
                                                                         ExpirationDate = iig.FirstOrDefault().ExpirationDate,
                                                                         PurchaseDate = iig.FirstOrDefault().PurchaseDate,
                                                                         User = iig.FirstOrDefault().User,
                                                                         UserID = iig.FirstOrDefault().UserID
                                                                     }).ToList();

        //A query that get all the ingredients from earlier meals, and group them, where the ingredientCount is the numbers of times the specfic ingredient has been used.
        public List<LastMeal> ingredientsFromLastMeals = (from meals in App.db.Meals
                                                          join ri in App.db.RecipeIngredients on meals.RecipeID equals ri.RecipeID
                                                          join i in App.db.Ingredients on ri.IngredientID equals i.ID
                                                          where meals.UserID == App.CurrentUser.ID && meals.Date <= DateTime.Now
                                                          group i by i.ID into igrouped
                                                          select new LastMeal()
                                                          {
                                                              ingredientID = igrouped.FirstOrDefault().ID,
                                                              ingredientCount = igrouped.Count()
                                                          }).ToList();

        //A query that gets a list recipes where a blacklisted ingredient is used.
        public List<int> blackList = (from bl in App.db.BlacklistIngredients
                                      join ri in App.db.RecipeIngredients on bl.IngredientID equals ri.IngredientID
                                      where bl.UserID == App.CurrentUser.ID
                                      group ri by ri.RecipeID into ri
                                      select ri.FirstOrDefault().RecipeID).ToList();

        //get a list of graylisted ingredients.
        public List<GrayList> grayList = (from gl in App.db.GraylistIngredients
                                          join i in App.db.Ingredients on gl.IngredientID equals i.ID
                                          where gl.UserID == App.CurrentUser.ID
                                          group gl by gl.IngredientID into gl
                                          select new GrayList()
                                          {
                                              ingredient = gl.FirstOrDefault().Ingredient,
                                              rating = (gl.Sum(r => r.IngredientValue) / gl.Count())
                                          }).ToList();
        #endregion

        #region Methods
        public IQueryable<IGrouping<int, Result>> search(List<int> recipeIDs)
        {
            IQueryable<IGrouping<int, Result>> recipeIngredients = from ri in App.db.RecipeIngredients
                                                                   join i in App.db.Ingredients on ri.IngredientID equals i.ID
                                                                   join r in App.db.Recipes on ri.RecipeID equals r.ID
                                                                   where recipeIDs.Contains(ri.RecipeID)
                                                                   group new Result()
                                                                   {
                                                                       recipe = ri.Recipe,
                                                                       ingredient = ri.Ingredient,
                                                                       quantity = ri.Quantity
                                                                   } by ri.RecipeID;

            return recipeIngredients;
        }

        public ObservableCollection<SearchResults> addValuesToSearch(IQueryable<IGrouping<int, Result>> userInput, List<string> searchKeywords = null)
        {
            ObservableCollection<SearchResults> result = new ObservableCollection<SearchResults>();

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

                    if (ingredientsFromLastMeals.Where(iflm => iflm.ingredientID == res.ingredient.ID).Count() != 0)
                    {
                        searchResult.prevIngredients += ingredientsFromLastMeals.Where(iflm => iflm.ingredientID == res.ingredient.ID).Single().ingredientCount;
                    }

                    if (grayList.Where(gl => res.ingredient.ID == gl.ingredient.ID).Count() != 0)
                    {
                        searchResult.setRating = grayList.Where(gl => res.ingredient.ID == gl.ingredient.ID).SingleOrDefault().rating;
                    }
                    else
                    {
                        //50 is the default value of nonrated items
                        searchResult.setRating = 50;
                    }
                }

                result.Add(searchResult);
            }


            return new ObservableCollection<SearchResults>(result.OrderByDescending(res => res.percentageFullMatch).ThenByDescending(res => res.percentagePartialMatch).ThenByDescending(res => res.getRating).ThenByDescending(res => res.prevIngredients).ThenByDescending(res => res.recipe.Title));
        }
        #endregion
    }
}
