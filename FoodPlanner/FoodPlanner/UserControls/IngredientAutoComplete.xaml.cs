using FoodPlanner.Models;
using MvvmFoundation.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FoodPlanner.UserControls
{
    /// <summary>
    /// Interaction logic for AutoComplete.xaml
    /// </summary>
    public partial class IngredientAutoComplete : UserControl, INotifyPropertyChanged
    {

        #region Fields

        // private int _maximumAutoCompleteItems = 10; //TODO: this might not be the right place.

        private List<Ingredient> _queriedIngredients;
        private string _searchText;
        private string _lastSearchText;

        #endregion

        public IngredientAutoComplete()
        {
            InitializeComponent();
            MaximumItems = 5; // Default value
        }

        #region Dependency Properties & Commands

        private static readonly DependencyProperty MaximumItemsProperty =
            DependencyProperty.Register("MaximumItems", typeof(int), typeof(IngredientAutoComplete)

        private static readonly DependencyProperty SelectItemCommandProperty =
            DependencyProperty.Register("SelectItemCommand", typeof(ICommand), typeof(IngredientAutoComplete));

        public int MaximumItems
        {
            get { return (int)GetValue(MaximumItemsProperty); }
            set { SetValue(MaximumItemsProperty, value); }
        }

        public ICommand SelectItemCommand
        {
            get { return (ICommand)GetValue(SelectItemCommandProperty); }
            set { SetValue(SelectItemCommandProperty, value); }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine(sender);
            ListBox listBox = sender as ListBox;
            if (SelectItemCommand != null && listBox != null)
            {
                SelectItemCommand.Execute(listBox.SelectedItem);
                SearchText = "";
            }
        }

        #endregion

        public IEnumerable<Ingredient> FoundIngredients
        {
            get
            {
                if (_queriedIngredients == null)
                {
                    _queriedIngredients = new List<Ingredient>();
                }
                return _queriedIngredients.Where(i => i.Name.ToLower().Contains(SearchText.ToLower()));
            }
        }

        public Visibility AutoCompleteListVisibility
        {
            get
            {
                if (FoundIngredients.Count() > 0)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }

        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                _searchText = value;
                RaisePropertyChanged("SearchText");
                RaisePropertyChanged("FoundIngredients");
                RaisePropertyChanged("AutoCompleteListVisibility");
                TryToRepopulateTheList();
            }
        }

        #region Methods

        private void TryToRepopulateTheList()
        {
            // Only query the database if the search string has changed
            // and a continues search string could change the previously fetched items.
            if (string.IsNullOrEmpty(_lastSearchText) ||
                !SearchText.StartsWith(_lastSearchText, StringComparison.OrdinalIgnoreCase) ||
                _queriedIngredients.Count() >= MaximumItems)
            {
                _lastSearchText = SearchText;
                if (SearchText == "")
                {
                    _queriedIngredients.Clear();
                    RaisePropertyChanged("FoundIngredients");
                    RaisePropertyChanged("AutoCompleteListVisibility");
                }
                else
                {
                    Console.WriteLine("Fetching data from db! " + DateTime.Now.ToLongTimeString());
                    PopulateListWithIngredientsFromDatabase();
                }
            }
            else
            {
                Console.WriteLine("Avoided an unnecessary db lookup");
            }
        }

        private void PopulateListWithIngredientsFromDatabase()
        {
            //TODO: this function should run asynchronously - and not block user interaction.
            // should maybe be an event with callback to remove propertychanged dependency
            string originalSearchText = SearchText;

            var foundIngredientsInDb = App.db.Ingredients
                .Where(i => i.Name.ToLower().Contains(originalSearchText.ToLower()))
                .Take(MaximumItems)
                .OrderBy(i => i.Name.ToLower().IndexOf(originalSearchText));

            // Populate the list if the search text has not changed.
            if (originalSearchText == SearchText)
            {
                _queriedIngredients = foundIngredientsInDb.ToList();
                RaisePropertyChanged("FoundIngredients");
                RaisePropertyChanged("AutoCompleteListVisibility");
            }
            else
            {
                Console.WriteLine("Search text changed before repopulation...");
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                PropertyChanged(this, e);
            }
        }

        #endregion

    }
}
