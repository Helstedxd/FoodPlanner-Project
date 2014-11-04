using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner.Models
{
    public partial class CookingStep
    {
        public override string ToString()
        {
            return this.Step;
        }
    }
}
