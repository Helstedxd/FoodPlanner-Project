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
using System.Windows.Media;

namespace FoodPlanner.ViewModels
{
    public class RecipeViewModel : ObservableObject
    {

        #region Fields

        private string _statusText;
        private DateTime _activeDate;
        private Meal _meal = null;
        private ICommand _removeMealCommand;

        #endregion

        public Recipe Recipe { get; set; }
        public RecipeViewModel(Recipe recipe)
        {
            //Constructor if user is comming from search
            ActiveDate = DateTime.Now;
            Recipe = recipe;
        }

        public RecipeViewModel(Meal meal)
        {
            //Constructor if user is comming from mealplan
            Meal = meal;
            ActiveDate = meal.Date;
            Recipe = meal.Recipe;
        }

        #region Properties

        public Meal Meal
        {
            get
            {
                return _meal;
            }
            set
            {
                _meal = value;
                RaisePropertyChanged("Meal");
                RaisePropertyChanged("AddUpdateImage");
                RaisePropertyChanged("isMealSet");
                RaisePropertyChanged("AddUpdateMealCommand");
            }
        }

        public List<RecipeIngredient> RecipeIngredients
        {
            get
            {
                if (_meal != null)
                {
                    List<RecipeIngredient> returnList = new List<RecipeIngredient>();

                    // Scale the recipe ingredients quantity according to meal participants
                    Recipe.RecipeIngredients.ToList().ForEach(ri => returnList.Add(new RecipeIngredient()
                    {
                        Ingredient = ri.Ingredient,
                        Recipe = ri.Recipe,
                        //TODO: we should probably not round this, but display 2 decimals in xaml.
                        Quantity = Math.Round(ri.Quantity * ((decimal)_meal.Participants / (decimal)Recipe.Persons), 2),
                        IngredientID = ri.IngredientID,
                        RecipeID = ri.RecipeID
                    }));

                    return returnList;
                }
                return Recipe.RecipeIngredients.ToList();
            }
        }

        public string AddUpdateImage
        {
            get
            {
                if (Meal != null)
                {
                    return "../Images/reload.png";
                }
                return "../Images/plusIcon.png";
            }
        }

        public Visibility isMealSet
        {
            get
            {
                if (Meal != null)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }

        public DateTime ActiveDate
        {
            get { return _activeDate; }
            set { _activeDate = value; }
        }

        public string StatusText
        {
            get { return _statusText; }
            set { _statusText = value; RaisePropertyChanged("StatusText"); }
        }

        #endregion

        #region Methods
        private void AddMeal()
        {
            //Add meal to the users mealplan
            Meal newMeal = new Meal()
            {
                Recipe = this.Recipe,
                Date = ActiveDate,
                Participants = App.CurrentUser.PersonsInHouseHold,
                IsActive = true
            };

            App.CurrentUser.Meals.Add(newMeal);
            App.db.SaveChanges();
            Meal = newMeal;
        }

        private void UpdateMeal()
        {
            //Upadet the meal on the users mealplan
            Meal.Date = ActiveDate;
            if (_activeDate < DateTime.Now)
            {
                Meal.IsActive = false;
            }
            else
            {
                Meal.IsActive = true;
            }

            App.db.SaveChanges();
            StatusText = "Meal updated";
            RaisePropertyChanged("RecipeIngredients");
        }

        private void RemoveMeal()
        {
            //Remove a meal from the mealplan
            if (Meal != null)
            {
                App.db.Meals.Remove(Meal);
                App.db.SaveChanges();
                Meal = null;
            }
        }

        #endregion

        #region Commands
        public ICommand AddUpdateMealCommand
        {
            get
            {
                //TODO: Maybe save command in field.
                if (Meal == null)
                {
                    return new RelayCommand(() => AddMeal());
                }
                else
                {
                    return new RelayCommand(() => UpdateMeal());
                }
            }
        }

        public ICommand RemoveMealCommand
        {
            get
            {
                if (_removeMealCommand == null)
                {
                    _removeMealCommand = new RelayCommand(() => RemoveMeal());
                }
                return _removeMealCommand;
            }
        }
        #endregion
    }
}
