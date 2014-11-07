using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using FoodPlanner.Models;

namespace FoodPlanner.ViewModels
{

    public class InventoryViewModel
    {

        public CollectionViewSource InventoryIngredientsCollectionViewSource { get; private set; }

        public InventoryViewModel()
        {
            InventoryIngredientsCollectionViewSource = new CollectionViewSource();

            InventoryIngredientsCollectionViewSource.Source = MainWindow.CurrentUser.InventoryIngredients; 
        }

      

    }
}
