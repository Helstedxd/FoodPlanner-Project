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

        public string Name { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime PurchaseDate { get; set; }

        protected bUnits unit = bUnits.g;

        public Food(string name, int quantity, DateTime expirationDate, DateTime purchaseDate)
        {
            this.Name = name;
            this.Quantity = quantity;
            this.ExpirationDate = expirationDate;
            this.PurchaseDate = purchaseDate;
        }

    }

    class LiquidFood : Food
    {
        public LiquidFood(string name, int quantity, DateTime expirationDate, DateTime purchaseDate)
            : base(name, quantity, expirationDate, purchaseDate)
        {
            unit = bUnits.ml;
        }
    }

    class SolidFood : Food
    {
        public SolidFood(string name, int quantity, DateTime expirationDate, DateTime purchaseDate)
            : base(name, quantity, expirationDate, purchaseDate)
        {
            unit = bUnits.g;
        }
    }

    class MiscFood : Food
    {
        public MiscFood(string name, int quantity, DateTime expirationDate, DateTime purchaseDate)
            : base(name, quantity, expirationDate, purchaseDate)
        {
            unit = bUnits.stk;
        }
    }
}
