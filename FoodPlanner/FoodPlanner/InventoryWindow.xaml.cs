using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;

namespace FoodPlanner
{
    /// <summary>
    /// Interaction logic for InventoryWindow.xaml
    /// </summary>
    public partial class InventoryWindow : Window
    {
        public InventoryWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CollectionViewSource userViewSource = ((CollectionViewSource)(this.FindResource("userViewSource")));
            MainWindow.db.Users.Load();
            userViewSource.Source = MainWindow.db.Users.Local;
            //TODO: bad hack

            //userViewSource.Source = MainWindow.db.Users.Where(u => u.ID == MainWindow.CurrentUser.ID).ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*var lol = new InventoryIngredient();
            lol.Ingredient = new Ingredient();
            lol.Ingredient.Name = "Noget mad";
            lol.Ingredient.Unit = "stk";
            lol.Quantity = 123;

            MainWindow.CurrentUser.InventoryIngredients.Add(lol);*/

            // Remove unlinked items from the database
            foreach (var inventoryItem in MainWindow.db.InventoryIngredients.Local.ToList())
            {
                if (inventoryItem.Ingredient == null)
                {
                    MainWindow.db.InventoryIngredients.Remove(inventoryItem);
                }
            }

            MainWindow.db.SaveChanges();

            // Refresh the grids so the database generated values show up. 
            this.inventoryIngredientsDataGrid.Items.Refresh();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string searchString = searchTextBox.Text;
            int quantity;

            if (!int.TryParse(quantityTextBox.Text, out quantity))
            {
                MessageBox.Show("Quantity not a number!", ":(");
                return;
            }

            var result = MainWindow.db.Ingredients.Where(i => i.Name.ToLower().Contains(searchString.ToLower()));

            Ingredient first = result.FirstOrDefault();
            if (first != null)
            {
                searchTextBox.Text = first.Name;
            }
            else
            {
                MessageBox.Show("Food not found!", ":(");
                return;
            }

            //if (MainWindow.CurrentUser.InventoryIngredients)

            var test = new InventoryIngredient(first, quantity);
            MainWindow.CurrentUser.InventoryIngredients.Add(test);

            //MessageBox.Show(string.Join(", ", result.Select(i => i.Name).ToArray()));
        }


    }
}
