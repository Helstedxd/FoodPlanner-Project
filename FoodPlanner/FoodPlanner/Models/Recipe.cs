using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FoodPlanner.Models
{
    public partial class Recipe
    {
        public override string ToString()
        {
            return this.Title;
        }
    }
}
