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
            MondayMeals = new ObservableCollection<Meal>();//ObservableCollection allows for updates in the view
            TuesdayMeals = new ObservableCollection<Meal>();
            WednesdayMeals = new ObservableCollection<Meal>();
            ThursdayMeals = new ObservableCollection<Meal>();
            FridayMeals = new ObservableCollection<Meal>();
            SaturdayMeals = new ObservableCollection<Meal>();
            SundayMeals = new ObservableCollection<Meal>();

            ActiveDate = DateTime.Now;
            if (ActiveDate.DayOfWeek == DayOfWeek.Sunday) //This is done in order to get the GetDdayDifference method to make sense when it's Sunday
            {
                ActiveDate = ActiveDate.AddDays(-1);
            }

            ShowMeals();
        }

        #region Properties


        private DateTime ActiveDate
        {
            get { return _activeDate; }
            set
            {
                _activeDate = value;

                // Set monday date as it is the reference.
                _mondayDate = ActiveDate;
                while (_mondayDate.DayOfWeek != DayOfWeek.Monday)
                {
                    _mondayDate = _mondayDate.AddDays(-1);
                }

                RaisePropertyChanged("WeekString");

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

        public int Week
        {
            get
            {
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
            FlushMeals();
            ShowMeals();
        }

        public void PreviousWeek()
        {
            ActiveDate = ActiveDate.AddDays(-7);
            FlushMeals();
            ShowMeals();
        }

        private void FlushMeals()
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

        /*        
          
        private bool IsMatchDateMeal(Meal mealToCompare)
        {
            bool match = false;
            if (mealToCompare.Date == ActiveDate.AddDays(-GetDdayDifference(mealToCompare.Date.DayOfWeek)).Date)
            {
                match = true;
            }
            return match;
        } 
         
        private string getStringDay(int day)
        {
            string stringDay;
            switch (day)
            {
                case 1:
                    stringDay = "Monday";
                    break;
                case 2:
                    stringDay = "Tuesday";
                    break;
                case 3:
                    stringDay = "Wednesday";
                    break;
                case 4:
                    stringDay = "Thursday";
                    break;
                case 5:
                    stringDay = "Friday";
                    break;
                case 6:
                    stringDay = "Saturday";
                    break;
                case 7:
                    stringDay = "Sunday";
                    break;
                default:
                    stringDay = "Error, out of reach";
                    break;
            }
            return stringDay;
        }

        public int getWeeksInYear(int year)
        {
            DateTimeFormatInfo format = DateTimeFormatInfo.CurrentInfo;
            DateTime weekYear = new DateTime(year, 12, 31);
            System.Globalization.Calendar calendar = format.Calendar;
            return calendar.GetWeekOfYear(weekYear, format.CalendarWeekRule, format.FirstDayOfWeek);
        }
        private int newWeek(DateTime moment, bool goesUp)
        {
            // DateTime moment = new DateTime(DateTime.Now.Year, 12, 27);

            if (goesUp)
            {
                moment = moment.AddDays(7);
            }

            else
            {
                moment = moment.AddDays(-7);
            }
            System.Globalization.Calendar calendar = CultureInfo.CurrentCulture.Calendar;
            System.Globalization.CalendarWeekRule rule = CalendarWeekRule.FirstFourDayWeek;
            int weekNumber = calendar.GetWeekOfYear(moment, rule, moment.DayOfWeek);
            return weekNumber;
        }
        private void updateArrows(Button butUp, Button butDown)
        {
            System.Globalization.Calendar calendar = CultureInfo.CurrentCulture.Calendar;
            DateTime moment = DateTime.Now;
            System.Globalization.CalendarWeekRule rule = CalendarWeekRule.FirstFourDayWeek;
            int weekNumber = calendar.GetWeekOfYear(moment, rule, moment.DayOfWeek);
            bool goUp;

            List<Button> buttonList = new List<Button> { butUp, butDown };
            if (buttonList[0].IsFocused)
            {
                goUp = true;
            }
            else
            {
                goUp = false;
            }

            string upDate = buttonList[0].Content.ToString(), downDate = buttonList[1].Content.ToString();//text from butDown and butUp
            string[] upWeek = upDate.Split(' '), downWeek = downDate.Split(' '); //Splits the sting into '^' 'week:' 'number'

            int futureDiff = Convert.ToUInt16(upWeek[2]) - weekNumber;
            int pastDiff = weekNumber - Convert.ToUInt16(downWeek[2]);

            DateTime futureWeek = DateTime.Now.AddDays(7 * futureDiff);
            DateTime pastWeek = DateTime.Now.AddDays(-7 * pastDiff);

            if (goUp)
            {
                butUp.Content = "^ week: " + (newWeek(futureWeek, true)).ToString(); //gets an updated 'number' and updates the text on the butUp
                butDown.Content = "v week: " + (newWeek(pastWeek, true)).ToString(); //gets an updated 'number' and updates the text on the butDown                
            }
            else if (!goUp)
            {
                butUp.Content = "^ week: " + (newWeek(futureWeek, false)).ToString(); //gets an updated 'number' and updates the text on the butUp
                butDown.Content = "v week: " + (newWeek(pastWeek, false)).ToString(); //gets an updated 'number' and updates the text on the butDown
            }
        }
        private string updateDate(string date, bool goesUp) 
        { 
            string[] split = date.Split('.'); //date 23.02.2014
            string result;
            int day = Convert.ToInt16(split[0]), month = Convert.ToInt16(split[1]), year = Convert.ToInt16(split[2]);
            DateTime moment = new DateTime(year, month, day);

            if (goesUp)
            {
                moment = moment.AddDays(7);
            }
            else
            {
                moment = moment.AddDays(-7);
            }
            return result = getStringDay(Convert.ToInt16(moment.DayOfWeek)) + "\n" + moment.Day + "." + moment.Month;
        }
        */
    }
}