using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodPlanner.Models;
using MvvmFoundation.Wpf;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;

namespace FoodPlanner.ViewModels
{
    public class RecipeViewModel : ObservableObject
    {
        private ICommand _startDialogCommand;

        public Recipe Recipe { get; set; }
        public RecipeViewModel(Recipe recipe)
        {
            ActiveDate = DateTime.Now;
            this.Recipe = recipe;
        }

        #region Properties
        private DateTime _activeDate;
        private string _succesText = "", afterString;

        public DateTime ActiveDate
        {
            get 
            { 
                return _activeDate; 
            }
            set
            {
                _activeDate = value;
            }
        }

        public string SuccesText 
        {
            get
            {
                return _succesText + afterString;
            }
        }
        #endregion

        #region Methods
        private void AddMeal()
        {
            Meal newMeal = new Meal()
            {
                Recipe = this.Recipe,
                Date = ActiveDate,
                Participants = App.CurrentUser.PersonsInHouseHold,
                RecipeID = this.Recipe.ID,
                User = App.CurrentUser,
                UserID = App.CurrentUser.ID
            };
            App.CurrentUser.Meals.Add(newMeal);
            App.db.SaveChanges();
            afterString = "Meal added";
            RaisePropertyChanged("SuccesText");
        }
        #endregion

        #region Commands
        public ICommand StartDialogCommand
        {
            get
            {
                if (_startDialogCommand == null)
                {
                    _startDialogCommand = new RelayCommand(() => AddMeal());
                }
                return _startDialogCommand;
            }
        }
        #endregion
    }
}
