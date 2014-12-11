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
        private DateTime _activeDate;
        private Meal _meal = null;
        private string _succesText = "", afterString;
        private System.Windows.Media.Brush _succesTextColour = System.Windows.Media.Brushes.Black;
        private ICommand _startDialogCommand;
        private ICommand _removeMealCommand;

        public Recipe Recipe { get; set; }
        public RecipeViewModel(Recipe recipe)
        {
            activeDate = DateTime.Now;
            this.Recipe = recipe;
        }

        public RecipeViewModel(Meal meal)
        {
            Meal = meal;
            activeDate = meal.Date;
            this.Recipe = meal.Recipe;
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
                RaisePropertyChanged("getMealParticipants");
                RaisePropertyChanged("getImage");
                RaisePropertyChanged("isMealSet");
            }
        }

        public List<RecipeIngredient> getRecipeIngredients
        {
            get
            {
                if (_meal != null)
                {
                    List<RecipeIngredient> returnList = new List<RecipeIngredient>();

                    Recipe.RecipeIngredients.ToList().ForEach(ri => returnList.Add(new RecipeIngredient() { Ingredient = ri.Ingredient, Recipe = ri.Recipe, Quantity = Math.Round(ri.Quantity * ((decimal)_meal.Participants / (decimal)Recipe.Persons), 2), IngredientID = ri.IngredientID, RecipeID = ri.RecipeID }));

                    return returnList;
                }
                return Recipe.RecipeIngredients.ToList();
            }
        }

        public int getMealParticipants
        {
            get
            {
                if (Meal != null)
                {
                    return Meal.Participants;
                }
                return 0;
            }
            set
            {
                if (value > 0)
                {
                    Meal.Participants = value;
                }
            }
        }

        public string getImage
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

        public System.Windows.Media.Brush SuccesTextColour
        {
            get
            {
                return _succesTextColour;
            }
            set
            {
                _succesTextColour = value;
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
                SuccesTextColour = _succesTextColour = System.Windows.Media.Brushes.Black;
            }
            else
            {
                afterString = "Meal was not added";
                _succesTextColour = System.Windows.Media.Brushes.Red;
            }
            RaisePropertyChanged("SuccesText");
            RaisePropertyChanged("SuccesTextColour");
        }

        public void updateMeal()
        {
            Meal.Date = activeDate;
            /*
            if (activeDate < DateTime.Now)
            {
                Meal.IsActive = false;
            }
            else
            {
                Meal.IsActive = true;
            }
            */

            afterString = "Meal updated";
            SuccesTextColour = _succesTextColour = System.Windows.Media.Brushes.Black;
            App.db.SaveChanges();

            RaisePropertyChanged("SuccesText");
            RaisePropertyChanged("SuccesTextColour");
        }

        private void RemoveMeal()
        {
            if (Meal != null)
            {
                App.db.Meals.Remove(Meal);
                App.db.SaveChanges();
                Meal = null;
            }
        }

        #endregion

        #region Commands
        public ICommand StartDialogCommand
        {
            get
            {
                if (Meal == null)
                {
                    if (_startDialogCommand == null)
                    {
                        _startDialogCommand = new RelayCommand(() => AddMeal());
                    }
                    return _startDialogCommand;
                }
                else
                {
                    if (_startDialogCommand == null)
                    {
                        _startDialogCommand = new RelayCommand(() => updateMeal());
                    }
                    return _startDialogCommand;
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
