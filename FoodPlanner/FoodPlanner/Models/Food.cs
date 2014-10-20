using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner.Models
{

    interface IngredientWithQuantity
    {
        Ingredient Ingredient { get; set; }
        int Quantity { get; set; }
    }

    interface Storable
    {
        DateTime ExpirationDate { get; set; }
        DateTime PurchaseDate { get; set; }
    }

    public class Food : IngredientWithQuantity //TODO: rename an ingredient/food with  a quantity (ListedFood)
    {
        // A property named ID is treated as a PK by default
        public int ID { get; set; }

        // Naming it RecipeID makes it a foreign key into the Recipe table
        public int RecipeID { get; set; }
        public int IngredientID { get; set; }

        protected Food() { } // EF required constructor

        public virtual Ingredient Ingredient { get; set; }
        public int Quantity { get; set; }

        public Food(Ingredient ingredient, int quantity)
        {
            this.Ingredient = ingredient;
            this.Quantity = quantity;
        }

    }



    //TODO: reimplement this (i had to remove it for now..)
    /*
    class Ingredient { 
          public string Name { get; set; }
          public int unit;
    
    }

     class ContainedFood
    {
        protected enum bUnits { g, ml, stk };

      
        public Ingredient Ingredient { get; set; }
        public int Quantity { get; set; } 
        public DateTime ExpirationDate { get; set; }
        public DateTime PurchaseDate { get; set; }

        protected bUnits unit = bUnits.g;

        public ContainedFood(Ingredient ingredient, int quantity, DateTime expirationDate, DateTime purchaseDate)
        {
            this.Ingredient = ingredient;
            //this.Quantity = quantity;
            this.ExpirationDate = expirationDate;
            this.PurchaseDate = purchaseDate;
        }

    }


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
             //this.Quantity = quantity;
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
     */
}
