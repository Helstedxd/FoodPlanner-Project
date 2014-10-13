using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner.Classes
{
    abstract class Storage
    {
        private string test = null;
        public Storage(string input)
        {
            test = input;
        }
    }

    class ShoppingList : Storage
    {
        public ShoppingList(string test)
            : base(test)
        {

        }
    }

    class Inventory : Storage
    {
        public Inventory(string test)
            : base(test)
        {

        }
    }
}
