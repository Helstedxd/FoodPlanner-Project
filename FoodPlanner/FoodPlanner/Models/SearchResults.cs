using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner.Models
{
    class Result
    {
        #region Properties
        public Recipe recipe { get; set; }
        public Ingredient ingredient { get; set; }
        public decimal quantity { get; set; }
        #endregion

        #region Methods & Constructor
        public Result() { }
        #endregion
    }

    public class SearchResults
    {
        #region Fields
        private int _fullMatch = 0, _partialMatch = 0, _keyWordMatch = 0, _prevIngredients = 0;
        private decimal _rating = 0;
        private List<Ingredient> _ingredients;
        private Recipe _recipe;
        #endregion

        #region Properties
        public Recipe recipe
        {
            get
            {
                return _recipe;
            }
        }

        public string getMatchPercentage
        {
            get
            {
                return string.Format("{0}/{1}", _fullMatch, _ingredients.Count());
            }
        }

        public decimal setRating
        {
            set
            {
                _rating += value;
            }
        }

        public decimal getRating
        {
            get
            {
                return _rating / _ingredients.Count();
            }
        }

        public int fullMatch
        {
            get
            {
                return _fullMatch;
            }

            set
            {
                _fullMatch = value;
            }
        }

        public int partialMatch
        {
            get
            {
                return _partialMatch;
            }

            set
            {
                _partialMatch = value;
            }
        }

        public int keyWordMatch
        {
            get
            {
                return _keyWordMatch;
            }

            set
            {
                _keyWordMatch = value;
            }
        }

        public List<Ingredient> ingredient
        {
            get
            {
                return _ingredients;
            }
        }

        public int numIngredients
        {
            get
            {
                return _ingredients.Count();
            }
        }

        public int prevIngredients
        {
            get
            {
                return _prevIngredients;
            }
            set
            {
                _prevIngredients = value;
            }
        }
        #endregion

        #region Methods & Constructor
        public void addIngredient(Ingredient ingredient)
        {
            _ingredients.Add(ingredient);
        }

        public SearchResults(Recipe recipe)
        {
            _recipe = recipe;
            _ingredients = new List<Ingredient>();
        }
        #endregion
    }

    class LastMeal
    {
        public LastMeal() { }

        public int ingredientID { get; set; }

        public int ingredientCount { get; set; }
    }
}
