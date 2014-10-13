using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner
{
    class Food
    {
        enum bUnit {none, g, ml, stk};

        //private int id = 0;
        private string foodName = null;
        private bUnit unit = bUnit.none;
        private int amount = -1;

        public Food()
        {

        }
    }
}
