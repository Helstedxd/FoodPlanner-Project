using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner.Models
{
    class inventoryListGroupedByQuantity
    {
        public int IngredientID { get; set; }

        public string Unit { get; set; }

        public decimal Quantity { get; set; }

        public Ingredient Ingredient { get; set; }

        public DateTime ExpirationDate { get; set; }

        public DateTime PurchaseDate { get; set; }

        public User User { get; set; }

        public int UserID { get; set; }

        public inventoryListGroupedByQuantity() { }
    }
}
