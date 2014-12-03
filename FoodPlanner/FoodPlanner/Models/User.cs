using FoodPlanner.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner.Models
{
    public partial class User
    {
        public virtual ObservableCollection<GraylistIngredient> GraylistIngredientsWithoutDiets
        {
            get
            {
                return new ObservableCollection<GraylistIngredient>(GraylistIngredients.Where(gi => !gi.IsFromDiet));
            }
        }

        public virtual ObservableCollection<BlacklistIngredient> BlacklistIngredientsWithoutDiets
        {
            get
            {
                return new ObservableCollection<BlacklistIngredient>(BlacklistIngredients.Where(bi => !bi.IsFromDiet));
            }
        }
    }
}
