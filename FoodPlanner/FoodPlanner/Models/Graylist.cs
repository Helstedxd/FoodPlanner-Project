using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner.Models
{
    class GrayList
    {
        #region Properties
        public Ingredient ingredient { get; set; }

        public int rating { get; set; }
        #endregion

        #region Methods & Constructor
        public GrayList() { }
        #endregion
    }
}
