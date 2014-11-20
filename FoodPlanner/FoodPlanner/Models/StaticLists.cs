using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner.Models
{
    static class StaticLists
    {
        public static List<inventoryListGroupedByQuantity> inventoryList = (from ii in App.db.InventoryIngredients
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

        public static List<Meal> lastMeals = (from meals in App.db.Meals
                                              where meals.UserID == App.CurrentUser.ID && meals.Date <= DateTime.Now
                                              orderby meals.ID descending
                                              select meals).ToList();

        public static List<LastMeal> ingredientFromLatestMeals = (from i in App.db.Ingredients
                                                                  join ri in App.db.RecipeIngredients on i.ID equals ri.IngredientID
                                                                  where lastMeals.Any(lm => lm.RecipeID == ri.RecipeID)
                                                                  group i by i.ID into igrouped
                                                                  select new LastMeal()
                                                                  {
                                                                      ingredientID = igrouped.FirstOrDefault().ID,
                                                                      ingredientCount = igrouped.Count()
                                                                  }).ToList();

        public static List<int> blackList = (from bl in App.db.BlacklistIngredients
                                             join ri in App.db.RecipeIngredients on bl.IngredientID equals ri.IngredientID
                                             where bl.UserID == App.CurrentUser.ID
                                             select ri.RecipeID).ToList();


        public static List<GrayList> grayList = (from gl in App.db.GraylistIngredients
                                                 join i in App.db.Ingredients on gl.IngredientID equals i.ID
                                                 where gl.UserID == App.CurrentUser.ID
                                                 select new GrayList()
                                                 {
                                                     ingredient = i,
                                                     rating = gl.IngredientValue
                                                 }).ToList();

    }
}
