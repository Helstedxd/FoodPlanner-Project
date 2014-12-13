using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using FoodPlanner.Models;
using MvvmFoundation.Wpf;


namespace FoodPlanner.ViewModels
{
    public class MealPlanViewModel : ObservableObject
    {

        #region Fields
        private ICommand _nextWeek;
        private ICommand _previousWeek;
        private DateTime _activeDate;
        private DateTime _mondayDate;
        #endregion

        public MealPlanViewModel()
        {
            // ObservableCollection allows for updates in the view
            MondayMeals = new ObservableCollection<Meal>();
            TuesdayMeals = new ObservableCollection<Meal>();
            WednesdayMeals = new ObservableCollection<Meal>();
            ThursdayMeals = new ObservableCollection<Meal>();
            FridayMeals = new ObservableCollection<Meal>();
            SaturdayMeals = new ObservableCollection<Meal>();
            SundayMeals = new ObservableCollection<Meal>();

            ActiveDate = DateTime.Now;

            ShowMeals();
        }

        #region Properties


        private DateTime ActiveDate
        {
            get { return _activeDate; }
            set
            {
                _activeDate = value;

                // Find first monday before activeDate
                _mondayDate = ActiveDate;
                while (_mondayDate.DayOfWeek != DayOfWeek.Monday)
                {
                    _mondayDate = _mondayDate.AddDays(-1);
                }

                // Update the properties that depend on ActiveDate or Monday
                RaisePropertyChanged("Week");
                RaisePropertyChanged("MondayDate");
                RaisePropertyChanged("TuesdayDate");
                RaisePropertyChanged("WednesdayDate");
                RaisePropertyChanged("ThursdayDate");
                RaisePropertyChanged("FridayDate");
                RaisePropertyChanged("SaturdayDate");
                RaisePropertyChanged("SundayDate");
            }
        }

        #region WeekDay Dates

        // Monday is used as reference for all dates
        public DateTime MondayDate
        {
            get
            {
                return _mondayDate;
            }
        }

        public DateTime TuesdayDate
        {
            get
            {
                return MondayDate.AddDays(1);
            }
        }

        public DateTime WednesdayDate
        {
            get
            {
                return MondayDate.AddDays(2);
            }
        }

        public DateTime ThursdayDate
        {
            get
            {
                return MondayDate.AddDays(3);
            }
        }

        public DateTime FridayDate
        {
            get
            {
                return MondayDate.AddDays(4);
            }
        }

        public DateTime SaturdayDate
        {
            get
            {
                return MondayDate.AddDays(5);
            }
        }

        public DateTime SundayDate
        {
            get
            {
                return MondayDate.AddDays(6);
            }
        }

        #endregion

        #region WeekDay Meal Collections

        public ObservableCollection<Meal> MondayMeals { get; private set; }

        public ObservableCollection<Meal> TuesdayMeals { get; private set; }

        public ObservableCollection<Meal> WednesdayMeals { get; private set; }

        public ObservableCollection<Meal> ThursdayMeals { get; private set; }

        public ObservableCollection<Meal> FridayMeals { get; private set; }

        public ObservableCollection<Meal> SaturdayMeals { get; private set; }

        public ObservableCollection<Meal> SundayMeals { get; private set; }

        #endregion

        // Gets the week number of ActiveDate
        public int Week
        {
            get
            {
                // TODO: GetWeekOfYear does not follow ISO 8601 (returns week 53 instead of 1)
                DateTimeFormatInfo timeFormat = DateTimeFormatInfo.CurrentInfo;
                Calendar calendar = timeFormat.Calendar;
                return calendar.GetWeekOfYear(ActiveDate, timeFormat.CalendarWeekRule, timeFormat.FirstDayOfWeek);
            }
        }

        #endregion

        #region Methods

        public void NextWeek()
        {
            ActiveDate = ActiveDate.AddDays(7);
            ClearMeals();
            ShowMeals();
        }

        public void PreviousWeek()
        {
            ActiveDate = ActiveDate.AddDays(-7);
            ClearMeals();
            ShowMeals();
        }

        private void ClearMeals()
        {
            MondayMeals.Clear();
            TuesdayMeals.Clear();
            WednesdayMeals.Clear();
            ThursdayMeals.Clear();
            FridayMeals.Clear();
            SaturdayMeals.Clear();
            SundayMeals.Clear();
        }

        private void ShowMeals()
        {
            //TODO: We could also just query the individual days in the property getter.
            List<Meal> mealList = App.db.Meals.Where(m => m.Date >= MondayDate.Date && m.Date <= SundayDate.Date).ToList();

            // List of day-collections indexed according to the DayOfWeek enum
            List<ObservableCollection<Meal>> MealDayCollections = new List<ObservableCollection<Meal>>() {
                SundayMeals,
                MondayMeals,
                TuesdayMeals,
                WednesdayMeals,
                ThursdayMeals,
                FridayMeals,
                SaturdayMeals
            };

            // Add meals to the correct day-list
            foreach (Meal m in mealList)
            {
                MealDayCollections[(int)m.Date.DayOfWeek].Add(m);
            }
        }

        #endregion

        #region Commands

        public ICommand NextWeekCommand
        {
            get
            {
                if (_nextWeek == null)
                {
                    _nextWeek = new RelayCommand(() => NextWeek());
                }

                return _nextWeek;
            }
        }
        public ICommand PreviousWeekCommand
        {
            get
            {
                if (_previousWeek == null)
                {
                    _previousWeek = new RelayCommand(() => PreviousWeek());
                }

                return _previousWeek;
            }
        }

        #endregion
    }
}