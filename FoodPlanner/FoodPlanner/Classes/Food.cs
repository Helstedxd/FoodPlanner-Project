using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner
{
    abstract class Food
    {
        protected enum bUnits {g, ml, stk};

        protected string foodName = null;
        protected bUnits unit = bUnits.g;
        protected int quantity = -1;
        protected DateTime expirationDate = DateTime.Now;
        protected DateTime purchaseDate = DateTime.Now;

        public Food()
        {

        }
    }

    class LiquidFood : Food
    {
        public LiquidFood(){
            unit = bUnits.ml;
        }
    }

    class SolidFood : Food
    {
        public SolidFood()
        {
            unit = bUnits.g;
        }
    }

    class MiscFood : Food
    {
        public MiscFood()
        {
            unit = bUnits.stk;
        }
    }
}
