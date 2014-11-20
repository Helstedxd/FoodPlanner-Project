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

namespace FoodPlanner.ViewModels {
    class SettingsViewModel : ObservableObject {
        #region Fields

        public User CurrentUser { get; set; }
        private ICommand _saveListItemCommand;
        public InventoryIngredient InventoryIngredient { get; set; }

        #endregion
        
        public SettingsViewModel() {
            InventoryIngredient = new InventoryIngredient();
            CurrentUser = App.CurrentUser;
        }

        #region Properties

        #endregion

        #region ICommands
        public ICommand SaveListItemCommand {
            get {
                if (_saveListItemCommand == null) {
                    _saveListItemCommand = new RelayCommand<Ingredient>(i => SaveChosenListItemFromAutoCompleteList(i));
                }

                return _saveListItemCommand;
            }
        }

        #endregion

        #region Methods

        private void SaveChosenListItemFromAutoCompleteList(Ingredient ingredient) {
            InventoryIngredient.Ingredient = ingredient;
        }

        #endregion
    }
}
