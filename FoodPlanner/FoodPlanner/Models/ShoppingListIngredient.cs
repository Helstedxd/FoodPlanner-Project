using MvvmFoundation.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner.Models
{
    public class ShoppingListIngredient : ObservableObject
    {
        private bool _checked;
        public bool Checked
        {
            get { return _checked; }
            set
            {
                //sets the ShoppingListIngredient to be checked then notify the property.
                _checked = value;
                RaisePropertyChanged("Checked");
            }
        }

        public InventoryIngredient InventoryIngredient { get; set; }


        public ShoppingListIngredient(InventoryIngredient inventoryIngredient)
        {
            this.InventoryIngredient = inventoryIngredient;
        }
    }
}
