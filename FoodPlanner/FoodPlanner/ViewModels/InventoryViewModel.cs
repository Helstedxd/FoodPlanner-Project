using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using FoodPlanner.Models;
using System.Windows.Input;
using System.Collections.ObjectModel;
using MvvmFoundation.Wpf;
using System.ComponentModel;

namespace FoodPlanner.ViewModels
{

    public class InventoryViewModel : ObservableObject
    {

        #region Fields

        private ICommand _saveInventoryCommand;
        private ICommand _addIngredientToInventory;
        private int _selectedSortIndex;

        #endregion

        public InventoryViewModel()
        {
            // Clone the collection to avoid wierd error..
            InventoryIngredients = new ObservableCollection<InventoryIngredient>(App.CurrentUser.InventoryIngredients);

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(InventoryIngredients);

            // Group by Ingredient 
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Ingredient");
            view.GroupDescriptions.Add(groupDescription);

            // Sort Descriptions and Names to display in combobox
            //TODO: this should probably be key-value pairs or something, instead of two arrays.
            SortDescriptions = new List<SortDescription>() {
                new SortDescription("Ingredient.Name", ListSortDirection.Ascending),
                new SortDescription("ExpirationDate", ListSortDirection.Ascending),
                new SortDescription("Quantity", ListSortDirection.Descending)
            };

            SortDescriptionNames = new ObservableCollection<string>() {
                "Name",
                "Expiration Date",
                "Quantity"
            };

            SelectedSortIndex = 0;
        }

        #region Properties

        public List<SortDescription> SortDescriptions { get; private set; }
        public ObservableCollection<string> SortDescriptionNames { get; private set; }

        public int SelectedSortIndex
        {
            get { return _selectedSortIndex; }
            set
            {
                _selectedSortIndex = value;
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(InventoryIngredients);
                foreach (SortDescription sr in SortDescriptions)
                    view.SortDescriptions.Remove(sr);
                view.SortDescriptions.Add(SortDescriptions[_selectedSortIndex]);
            }
        }

        //public IEnumerable<IGrouping<int, InventoryIngredient>> InventoryIngredients { get; set; }
        public ObservableCollection<InventoryIngredient> InventoryIngredients { get; set; }

        #endregion

        #region Commands

        public ICommand AddIngredientToInventoryCommand
        {
            get
            {
                if (_addIngredientToInventory == null)
                {
                    _addIngredientToInventory = new RelayCommand<Ingredient>(i => AddIngredientToInventory(i));
                }
                return _addIngredientToInventory;
            }
        }

        private ICommand _removeIngredientFromInventoryCommand;
        public ICommand RemoveIngredientFromInventoryCommand
        {
            get
            {
                if (_removeIngredientFromInventoryCommand == null)
                {
                    _removeIngredientFromInventoryCommand = new RelayCommand<InventoryIngredient>(ii => RemoveIngredientFromInventory(ii));
                }
                return _removeIngredientFromInventoryCommand;
            }
        }

        #endregion

        #region Methods

        private void AddIngredientToInventory(Ingredient ingredient)
        {
            if (ingredient != null)
            {
                InventoryIngredient newInventoryIngredient = new InventoryIngredient(ingredient, 1);
                App.CurrentUser.InventoryIngredients.Add(newInventoryIngredient);
                //TODO: InventoryIngredients is a copy of App.CurrentUser.InventoryIngredients
                // It would be better if we only needed to update one of them...
                InventoryIngredients.Add(newInventoryIngredient);
            }
        }

        private void RemoveIngredientFromInventory(InventoryIngredient inventoryIngredient)
        {
            if (inventoryIngredient != null)
            {
                App.db.InventoryIngredients.Remove(inventoryIngredient);
                //TODO: InventoryIngredients is a copy of App.CurrentUser.InventoryIngredients
                // It would be better if we only needed to update one of them...
                InventoryIngredients.Remove(inventoryIngredient);
            }
        }

        #endregion

    }
}
