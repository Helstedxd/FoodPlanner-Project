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
        private decimal _fullMatch = 0, _partialMatch = 0, _keyWordMatch = 0, _prevIngredients = 0, _rating = 0;
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
                return string.Format("{0}/{1}", Math.Round((getFullMatch * _ingredients.Count()), 2), _ingredients.Count());
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

        public decimal getFullMatch
        {
            get
            {
                return _fullMatch / _ingredients.Count();
            }
        }

        public decimal fullMatch
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

        public decimal partialMatch
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

        public decimal keyWordMatch
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

        public decimal prevIngredients
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
