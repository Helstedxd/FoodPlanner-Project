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
        private int _maximumAutoCompleteItems = 10;
        private string _lastSearchText = "";

        public InventoryWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CollectionViewSource userViewSource = ((CollectionViewSource)(this.FindResource("userViewSource")));
            MainWindow.db.Users.Load();
            userViewSource.Source = MainWindow.db.Users.Local;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
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
            Console.WriteLine("Inventory saved!");
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
          /*  Ingredient food = inventoryAutoCompleteBox.SelectedItem as Ingredient;
            if (food != null) {
                var newInventoryItem = new InventoryIngredient(food, 1);
                MainWindow.CurrentUser.InventoryIngredients.Add(newInventoryItem);
            }*/
        }

        private void inventoryAutoCompleteBox_Populating(object sender, PopulatingEventArgs e)
        {
            AutoCompleteBox acb = (sender as AutoCompleteBox);

            // Only query the database if the search string has changed
            // and a continues search string could change the previously fetched items.
            if (acb.SearchText.StartsWith(_lastSearchText, StringComparison.OrdinalIgnoreCase) &&
                acb.ItemsSource != null &&
                acb.ItemsSource.OfType<object>().Count() < _maximumAutoCompleteItems)
            {
                Console.WriteLine("Just avoided a unnecessary db lookup ;)");
            }
            else
            {
                // Cancel default population and query the database.
                e.Cancel = true;
                _lastSearchText = acb.SearchText;
                Console.WriteLine("Fetching data from db! " + DateTime.Now.ToLongTimeString());
                PopulateAutoCompleteBoxWithDataFromDatabase(acb);
            }
        }

        private void PopulateAutoCompleteBoxWithDataFromDatabase(AutoCompleteBox acb)
        {
            string originalSearchText = acb.SearchText;

            var foundIngredients = MainWindow.db.Ingredients
                .Where(i => i.Name.ToLower().Contains(originalSearchText.ToLower()))
                .Take(_maximumAutoCompleteItems)
                //.OrderBy(ii => ii.Ingredient.Name.IndexOf(originalSearchText, StringComparison.InvariantCultureIgnoreCase));
                .OrderBy(i => i.Name.ToLower().IndexOf(originalSearchText));

            // Populate the AutoCompleteBox if the search text has not changed.
            if (originalSearchText == acb.SearchText)
            {
                acb.ItemsSource = foundIngredients;
                acb.PopulateComplete();
            }
            else
            {
                Console.WriteLine("Search text changed before population...");
            }

        }

    }
}
