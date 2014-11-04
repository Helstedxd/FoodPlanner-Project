using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner.Models
{
    class SearchResults
    {
        public Recipe Recipe { get; set; }

        public int keyWordMatch { get; set; }

        public int partialMatch { get; set; }

        public int fullMatch { get; set; }

        public SearchResults() { }
    }
}
