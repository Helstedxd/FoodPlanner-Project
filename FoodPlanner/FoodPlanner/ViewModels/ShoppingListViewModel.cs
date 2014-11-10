using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using FoodPlanner.Models;

namespace FoodPlanner.ViewModels {

    public class ShoppingListViewModel {

        public CollectionViewSource ShoppingListCollectionViewSource { get; set; }

        public ShoppingListViewModel() {

            ShoppingListCollectionViewSource = new CollectionViewSource();
            
        }
    }
}
