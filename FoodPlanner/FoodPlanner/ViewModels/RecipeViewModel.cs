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

        private DateTime _activeDate;
        private Meal _meal = null;
        private string _succesText = "", afterString;
        private Brush _succesTextColour = Brushes.Black;
        private ICommand _removeMealCommand;

        #endregion

        public Recipe Recipe { get; set; }
        public RecipeViewModel(Recipe recipe)
        {
            ActiveDate = DateTime.Now;
            this.Recipe = recipe;
        }

        public RecipeViewModel(Meal meal)
        {
            Meal = meal;
            ActiveDate = meal.Date;
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
                RaisePropertyChanged("Image");
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

                    Recipe.RecipeIngredients.ToList().ForEach(ri => returnList.Add(new RecipeIngredient()
                    {
                        Ingredient = ri.Ingredient,
                        Recipe = ri.Recipe,
                        Quantity = Math.Round(ri.Quantity * ((decimal)_meal.Participants / (decimal)Recipe.Persons), 2),
                        IngredientID = ri.IngredientID,
                        RecipeID = ri.RecipeID
                    }));

                    return returnList;
                }
                return Recipe.RecipeIngredients.ToList();
            }
        }

        public string Image
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

        public Brush SuccesTextColour
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
                Date = ActiveDate,
                Participants = App.CurrentUser.PersonsInHouseHold,
                IsActive = true
            };

            bool mealDublicate = false;
            DateTime morning = new DateTime(ActiveDate.Year, ActiveDate.Month, ActiveDate.Day, 0, 0, 0);
            DateTime night = new DateTime(ActiveDate.Year, ActiveDate.Month, ActiveDate.Day, 23, 59, 59);
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
                Meal = newMeal;
                //TODO: fuck this
                //afterString = "Meal added";
                //SuccesTextColour = _succesTextColour = System.Windows.Media.Brushes.Black;
            }
            else
            {
                afterString = "Meal was not added";
                _succesTextColour = System.Windows.Media.Brushes.Red;
            }
            RaisePropertyChanged("SuccesText");
            RaisePropertyChanged("SuccesTextColour");
        }

        private void UpdateMeal()
        {
            Meal.Date = ActiveDate;
            if (_activeDate < DateTime.Now)
            {
                Meal.IsActive = false;
            }
            else
            {
                Meal.IsActive = true;
            }

            afterString = "Meal updated";
            SuccesTextColour = Brushes.Black;
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
