using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner
{
    public partial class RecipeIngredient
    {
        private double _match = 0;
        public override string ToString()
        {
            if (!Convert.ToBoolean(this.Quantity))
            {
                return Convert.ToString(this.Ingredient.Name);
            }
            else
            {
                return Convert.ToString(this.Quantity + this.Ingredient.Unit + " " + this.Ingredient.Name);
            }
        }

        public double Match
        {
            get
            {
                return _match;
            }
            set
            {
                _match = value;
            }
        }


        public void setMatch(List<string> searchList)
        {
            double val = 0;
            /*
            foreach (string s in searchList)
            {
                if (this.Title.Contains(s))
                {
                    val++;
                }

            }
            */



            _match = val / searchList.Count;
        }

    }
}
