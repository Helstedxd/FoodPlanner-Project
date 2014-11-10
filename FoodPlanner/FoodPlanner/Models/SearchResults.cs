﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner.Models
{
    class SearchResults
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
            private set
            {
                _ingredients = value;
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

        public SearchResults(Recipe recipe, int ingredients)
        {
            _recipe = recipe;
            _ingredients = ingredients;
        }
    }
}
