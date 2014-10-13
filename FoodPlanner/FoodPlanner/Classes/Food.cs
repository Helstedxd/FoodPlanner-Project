using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner
{
    abstract class Food
    {
        protected enum bUnits { g, ml, stk };

        protected string foodName = null;
        protected bUnits unit = bUnits.g;
        protected int quantity = -1;
        protected DateTime expirationDate = DateTime.Now;
        protected DateTime purchaseDate = DateTime.Now;

        public Food(string Name, int Quantity, DateTime ExpirationDate, DateTime PurchaseDate)
        {
            foodName = Name;
            quantity = Quantity;
            expirationDate = ExpirationDate;
            purchaseDate = PurchaseDate;
        }
    }

    class LiquidFood : Food
    {
        public LiquidFood(string Name, int Quantity, DateTime ExpirationDate, DateTime PurchaseDate)
            : base(Name, Quantity, ExpirationDate, PurchaseDate)
        {
            unit = bUnits.ml;
        }
    }

    class SolidFood : Food
    {
        public SolidFood(string Name, int Quantity, DateTime ExpirationDate, DateTime PurchaseDate)
            : base(Name, Quantity, ExpirationDate, PurchaseDate)
        {
            unit = bUnits.g;
        }
    }

    class MiscFood : Food
    {
        public MiscFood(string Name, int Quantity, DateTime ExpirationDate, DateTime PurchaseDate)
            : base(Name, Quantity, ExpirationDate, PurchaseDate)
        {
            unit = bUnits.stk;
        }
    }
}
