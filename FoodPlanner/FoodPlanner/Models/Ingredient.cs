using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner.Models
{
    public partial class Ingredient
    {
        public override string ToString()
        {
            return this.Name;
        }
    }
}
