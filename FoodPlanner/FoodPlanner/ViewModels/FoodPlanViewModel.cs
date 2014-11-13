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


namespace FoodPlanner.ViewModels
{
    public class FoodPlanViewModel : ObservableObject
    {
        private ICommand _nextWeek,
                         _previousWeek;
        public FoodPlanViewModel()
        {
            MondayMeals = new ObservableCollection<Meal>();//ObservableCollection allows for updates in the view
            TuesdayMeals = new ObservableCollection<Meal>();
            WednesdayMeals = new ObservableCollection<Meal>();
            ThursdayMeals = new ObservableCollection<Meal>();
            FridayMeals = new ObservableCollection<Meal>();
            SaturdayMeals = new ObservableCollection<Meal>();
            SundayMeals = new ObservableCollection<Meal>();

            ActiveDate = DateTime.Now;
            //TestMethod();
            ShowMeals();
        }

        #region Propertie
        private DateTime _activeDate;
        private DateTime ActiveDate
        {
            get { return _activeDate; }
            set { 
                _activeDate = value;
                OnPropertyChanged("MondayString");
                OnPropertyChanged("TuesdayString");
                OnPropertyChanged("WednesdayString");
                OnPropertyChanged("ThursdayString");
                OnPropertyChanged("FridayString");
                OnPropertyChanged("SaturdayString");
                OnPropertyChanged("SundayString");
                OnPropertyChanged("WeekString");
            }
        }

        public string MondayString
        {
            get
            {
                return Day(DayOfWeek.Monday);
            }
        }
        public ObservableCollection<Meal> MondayMeals
        {
            get;
            private set;
        }
        public string TuesdayString
        {
            get
            {
                return Day(DayOfWeek.Tuesday);
            }
        }
        public ObservableCollection<Meal> TuesdayMeals
        {
            get;
            private set;
        }
        public string WednesdayString
        {
            get
            {
                return Day(DayOfWeek.Wednesday);
            }
        }
        public ObservableCollection<Meal> WednesdayMeals
        {
            get;
            private set;
        }
        public string ThursdayString
        {
            get
            {
                return Day(DayOfWeek.Thursday);
            }
        }
        public ObservableCollection<Meal> ThursdayMeals
        {
            get;
            private set;
        }
        public string FridayString
        {
            get
            {
                return Day(DayOfWeek.Friday);
            }
        }
        public ObservableCollection<Meal> FridayMeals
        {
            get;
            private set;
        }
        public string SaturdayString
        {
            get
            {
                return Day(DayOfWeek.Saturday);
            }
        }
        public ObservableCollection<Meal> SaturdayMeals
        {
            get;
            private set;
        }
        public string SundayString
        {
            get
            {
                return Day(DayOfWeek.Sunday);
            }
        }
        public ObservableCollection<Meal> SundayMeals
        {
            get;
            private set;
        }
        public string WeekString
        {
            get
            {
                DateTimeFormatInfo timeFormat = DateTimeFormatInfo.CurrentInfo;
                Calendar calendar = timeFormat.Calendar;
                return "Week " + calendar.GetWeekOfYear(ActiveDate, timeFormat.CalendarWeekRule, timeFormat.FirstDayOfWeek).ToString();
            }
        }
        #endregion

        #region Methods

        private void TestMethod()
        {//Disse meals er allerede lagt ind: AddDays(7) AddDays(9) AddDays(-5)
            List<Recipe> recipes = App.db.Recipes.Where(r => r.Title.Contains("beef")).ToList();
            DateTime sørensFøds = new DateTime(2014, 11, 10);
            AddMealToMeals(sørensFøds, recipes[9], 4);
            AddMealToMeals(sørensFøds.AddDays(1), recipes[5], 4);
            AddMealToMeals(sørensFøds.AddDays(2), recipes[6], 2);
            AddMealToMeals(sørensFøds.AddDays(6), recipes[7], 3);
        }

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
        private string Day(DayOfWeek day)
        {
            int difference = GetDdayDifference(day);
            string result;

            return result = ActiveDate.AddDays(-difference).Date.ToString("dddd\ndd/MM",CultureInfo.CreateSpecificCulture("en-US"));
        }
        private int GetDdayDifference(DayOfWeek day)
        {
            int difference;

            if (day == DayOfWeek.Sunday)
            {
                difference = Convert.ToInt16(ActiveDate.DayOfWeek) - 7;
            }
            else
            {
                difference = ActiveDate.DayOfWeek - day;
            }

            return difference;
        }
        private void ShowMeals()
        {
            int mondayDifference = GetDdayDifference(DayOfWeek.Monday), sundayDifference = GetDdayDifference(DayOfWeek.Sunday);
            DateTime mondayDate = ActiveDate.AddDays(-mondayDifference), sundayDate = ActiveDate.AddDays(-sundayDifference);
            mondayDate = mondayDate.AddHours(-mondayDate.Hour);
            mondayDate = mondayDate.AddMinutes(-mondayDate.Minute);
            mondayDate = mondayDate.AddSeconds(-mondayDate.Second);
            mondayDate = mondayDate.AddMilliseconds(-mondayDate.Millisecond);
            List<Meal> mealList = App.db.Meals.Where(m => m.Date >= mondayDate.Date && m.Date <= sundayDate.Date).ToList();
            foreach (Meal m in mealList)
            {
                if (m.Date.DayOfWeek == DayOfWeek.Monday)
                {
                    MondayMeals.Add(m);
                }
                else if (m.Date.DayOfWeek == DayOfWeek.Tuesday)
                {
                    TuesdayMeals.Add(m);
                }
                else if (m.Date.DayOfWeek == DayOfWeek.Wednesday)
                {
                    WednesdayMeals.Add(m);
                }
                else if (m.Date.DayOfWeek == DayOfWeek.Thursday)
                {
                    ThursdayMeals.Add(m);
                }
                else if (m.Date.DayOfWeek == DayOfWeek.Friday)
                {
                    FridayMeals.Add(m);
                }
                else if (m.Date.DayOfWeek == DayOfWeek.Saturday)
                {
                    SaturdayMeals.Add(m);
                }
                else if (m.Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    SundayMeals.Add(m);
                }
            }
        }

        private void AddMealToMeals(DateTime dateForMeal, Recipe recipeForMeal, int Participants)
        {
            Meal newMeal = new Meal();
            bool userIDsucces = false, mealIDsucces = false, participantsSucces = false, dateSucces = false;
            try//CurrentUser.ID
            {
                if (App.CurrentUser.ID > 0) // CurrentUser ID  
                {
                    newMeal.User = App.CurrentUser;
                    userIDsucces = true;
                }
                else
                {
                    IDexceptionBox("ArgumentOutOfRangeException", "valid (value is below 0)");
                }
            }
            catch (Exception)
            {
                IDexceptionBox("An unknown exception", "valid");
            }

            try//recipeForMeal.ID 
            {
                if (recipeForMeal.ID > 0)  // RecipeForMeal ID
                {
                    newMeal.Recipe = recipeForMeal;
                    mealIDsucces = true;
                    //newMeal.Recipe.ID = App.db.Recipes.Where(r => r.ID == 1);
                }
                else
                {
                    IDexceptionBox("ArgumentOutOfRangeException", "valid (value is below 0)");
                }
            }
            catch (Exception)
            {
                IDexceptionBox("An unknown exception", "valid");
            }
            try//newMeal.Date
            {
                newMeal.Date = dateForMeal;
                dateSucces = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "newMeal.Date error");
            }
            try//Participants
            {
                if (Participants > 0)
                {
                    newMeal.Participants = Participants;
                    participantsSucces = true;
                }
                else
                {
                    MessageBox.Show("ERROR\nInvalidParticipantsNumber:\nThe given participants value is not accepted. It must be above zero", "addMealToMeals: InvalidParticipantsNumber");
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Participants error");
            }
            if (userIDsucces && mealIDsucces && participantsSucces && dateSucces) // If all previously assignments were succesful, the DB will be updated
            {
                App.db.Meals.Add(newMeal);
                App.db.SaveChanges();
            }
        }
        private void IDexceptionBox(string errorType, string faultType)
        {
            MessageBox.Show("ERROR\n" + errorType + ":\nThe given ID is not " + faultType, "addMealToMeals: " + errorType);
        }
        #endregion

        #region Commands
        public ICommand NextWeekCommand
        {
            get
            {
                if (_nextWeek == null)
                {
                    _nextWeek = new RelayCommand(p => NextWeek());
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
                    _previousWeek = new RelayCommand(p => PreviousWeek());
                }

                return _previousWeek;
            }
        }

        private ICommand _goToRecipeCommand;
        public ICommand GoToRecipeCommand
        {
            get
            {
                if (_goToRecipeCommand == null)
                {
                    _goToRecipeCommand = new RelayCommand(p => GoToRecipe((Recipe)p));
                }

                return _goToRecipeCommand;
            }
        }

        private void GoToRecipe(Recipe recipe)
        {
            //TODO: fuck mvvm basically
            RecipeViewModel rvm = new RecipeViewModel(recipe);
            Views.RecipePage rp = new Views.RecipePage();
            rp.DataContext = rvm;
            App.NavigationService.Navigate(rp);
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