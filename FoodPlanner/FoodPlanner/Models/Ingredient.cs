using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace FoodPlanner.Models
{
    public class Ingredient
    {
        public enum UnitType
        {
            g,
            ml,
            stk
        }

        protected Ingredient() { } // EF required constructor

        public int ID { get; set; } // EF PK

        [Index(IsUnique = true)]
        [StringLength(200)]
        public string Name { get; set; }

        public UnitType Unit { get; set; }

        public Ingredient(string name)
        {
            this.Name = name;
        }

        // Return the name of the ingredient when accessing the Ingredient object.
        public override string ToString()
        {
            return this.Name;
        }
    }

}
