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
        public Result() { }
        public Recipe recipe { get; set; }
        public Ingredient ingredient { get; set; }
        public decimal quantity { get; set; }

    }

    public class SearchResults
    {
        private int _fullMatch = 0, _partialMatch = 0, _keyWordMatch = 0;
        private List<Ingredient> _ingredients;
        private Recipe _recipe;

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

        public void addIngredient(Ingredient ingredient)
        {
            _ingredients.Add(ingredient);
        }

        public SearchResults(Recipe recipe)
        {
            _recipe = recipe;
            _ingredients = new List<Ingredient>();
        }
    }

    class SearchResults1
    {
        private int _ingredients = 0, _fullMatch = 0, _partialMatch = 0, _keyWordMatch = 0;
        private Recipe _recipe;

        public Recipe recipe
        {
            get
            {
                return _recipe;
            }
            private set
            {
                _recipe = value;
            }
        }

        public int ingredients
        {
            get
            {
                return _ingredients;
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

        public string getMatchPercentage
        {
            get
            {
                return string.Format("{0}/{1}", _fullMatch, _ingredients);
            }
        }

        public SearchResults1(Recipe recipe, int ingredients)
        {
            _recipe = recipe;
            _ingredients = ingredients;
        }
    }
}
