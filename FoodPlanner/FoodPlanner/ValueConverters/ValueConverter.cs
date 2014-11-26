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
                decimal quantity = col.Sum(o => ((InventoryIngredient)o).Quantity);
                string unit = ((InventoryIngredient)col.First()).Ingredient.Unit;
                return quantity + " " + unit;
            }

            throw new NotSupportedException("Converter only supports items of type ReadOnlyObservableCollection<object>");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Do the conversion from visibility to bool
            throw new NotImplementedException();
        }
    }


}