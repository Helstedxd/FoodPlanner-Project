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

        public InventoryViewModel()
        {
            InventoryIngredientsCollectionViewSource = new CollectionViewSource();



            var db = new FoodContext(); //TODO: this connection should not remain open like this.

            User CurrentUser = db.Users.First();
            InventoryIngredientsCollectionViewSource.Source = CurrentUser.InventoryIngredients; //db.InventoryIngredients.Local;

            // db.Dispose();

        }

        public CollectionViewSource InventoryIngredientsCollectionViewSource { get; set; }



    }
}
