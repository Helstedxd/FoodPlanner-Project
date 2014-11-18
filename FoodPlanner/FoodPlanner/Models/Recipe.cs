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
      /*  public TextTrimming TitleWithTrimming
        {
            get
            {
                return (TextTrimming)this.Title;
            }
        }*/
        public override string ToString()
        {
            return this.Title;
        }
    }
}
