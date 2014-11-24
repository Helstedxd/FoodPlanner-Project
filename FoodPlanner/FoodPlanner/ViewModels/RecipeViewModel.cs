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
            activeDate = DateTime.Now;
            this.Recipe = recipe;
        }

        #region Properties
        private DateTime _activeDate;
        private string _succesText = "", afterString;

        public DateTime activeDate
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
                Date = activeDate,
                Participants = App.CurrentUser.PersonsInHouseHold
            };

            bool mealDublicate = false;
            DateTime morning = new DateTime(activeDate.Year, activeDate.Month, activeDate.Day, 0, 0, 0);
            DateTime night = new DateTime(activeDate.Year, activeDate.Month, activeDate.Day, 23, 59, 59);
            List<Meal> mealList = App.db.Meals.Where(m => m.Date >= morning & m.Date <= night).ToList();
            foreach (Meal m in mealList)
            {
                if (m.Recipe == newMeal.Recipe)
                {
                    mealDublicate = true;
                }
            }

            if (!mealDublicate)
            {
                App.CurrentUser.Meals.Add(newMeal);
                App.db.SaveChanges();
                afterString = "Meal added";
            }
            else
            {
                afterString = "Meal was not added";
            }
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
