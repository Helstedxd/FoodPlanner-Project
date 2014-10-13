using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner.Classes
{
    abstract class Storage
    {
        protected List<Food> items = new List<Food>();


        public Storage(Food food)
        {
            items.Add(food);
        }
    }

    class ShoppingList : Storage
    {
        public ShoppingList(Food food)
            : base(food)
        {

        }
    }

    class Inventory : Storage
    {
        public Inventory(Food food)
            : base(food)
        {

        }
    }
}
