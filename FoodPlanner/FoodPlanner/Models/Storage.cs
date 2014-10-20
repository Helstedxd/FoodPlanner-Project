using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner.Models
{
    abstract class Storage
    {
        protected List<Food> items = new List<Food>();

        public Storage(List<Food> food)
        {
            items.AddRange(food);
        }

        public Storage(Food food)
        {
            items.Add(food);
        }

        public void AddItem(Food item)
        {
            items.Add(item);
        }
    }

    class ShoppingList : Storage
    {
        public ShoppingList(Food food) : base(food) { }
        public ShoppingList(List<Food> food) : base(food) { }
    }

    class Inventory : Storage
    {
        public Inventory(Food food) : base(food) { }
        public Inventory(List<Food> food) : base(food) { }
    }
}
