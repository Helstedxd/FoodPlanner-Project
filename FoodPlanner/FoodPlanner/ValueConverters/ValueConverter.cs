using FoodPlanner.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Collections.ObjectModel;

namespace FoodPlanner.ValueConverters
{
    public class ValueConverter
    {
    }

    public class InventoryIngredientCominedQuantityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ReadOnlyObservableCollection<object> col = value as ReadOnlyObservableCollection<object>;

            if (col != null)
            {
                //decimal quantity = col.Sum(o => ((InventoryIngredient)o).Quantity);
                decimal quantity = 0;
                foreach (object o in col)
                {
                    if (o is InventoryIngredient)
                    {
                        quantity += ((InventoryIngredient)o).Quantity;
                    }
                    else
                    {
                        throw new NotSupportedException("Collection must only contain elements of type InventoryIngredient");
                    }
                }

                string unit = ((InventoryIngredient)col.First()).Ingredient.Unit;
                return quantity + " " + unit;
            }

            throw new NotSupportedException("Converter only supports items of type ReadOnlyObservableCollection<object>");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}