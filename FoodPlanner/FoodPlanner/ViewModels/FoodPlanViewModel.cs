using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    public class FoodPlanViewModel
    {
        public FoodPlanViewModel()
        {
            activeDate = DateTime.Now;
        }

        private DateTime activeDate
        {
            get;
            set;
        }

        public string mondayString
        {
            get
            {
                return day(DayOfWeek.Monday);
            }
        }
        public string tuesdayString
        {
            get
            {
                return day(DayOfWeek.Tuesday);
            }
        }
        public string wednesdayString
        {
            get
            {
                return day(DayOfWeek.Wednesday);
            }
        }
        public string thursdayString
        {
            get
            {
                return day(DayOfWeek.Thursday);
            }
        }
        public string fridatString
        {
            get
            {
                return day(DayOfWeek.Friday);
            }
        }
        public string saturdayString
        {
            get
            {
                return day(DayOfWeek.Saturday);
            }
        }
        public string sundayString
        {
            get
            {
                return day(DayOfWeek.Sunday);
            }
        }


        public void weekUp()
        {
            activeDate = activeDate.AddDays(7);
        }
        public void weekDown()
        {
            activeDate = activeDate.AddDays(-7);
        }
        private string day(DayOfWeek day)
        {
            string result;
            int diff = activeDate.DayOfWeek - day;

            return result = activeDate.AddDays(-diff).Date.ToString();
        }

        /*        
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
        private void addMealToMeals(DateTime dateForMeal, Recipe recipeForMeal, int Participants)
        {
            Meal newMeal = new Meal();
            bool userIDsucces = false, mealIDsucces = false, participantsSucces = false, dateSucces = false;
            try
            {
                if (MainWindow.CurrentUser.ID > 0) // CurrentUser ID  
                {
                    newMeal.User = MainWindow.CurrentUser;
                    userIDsucces = true;
                }
                else
                {
                    IDexceptionBox("ArgumentOutOfRangeException", "valid (value is below 0)");
                }
            }
            catch (ArgumentNullException)
            {
                IDexceptionBox("ArgumentNullException", "valid");
            }
            catch (NotFiniteNumberException)
            {
                IDexceptionBox("NotFiniteNumberException", "a number");
            }
            catch (Exception)
            {
                IDexceptionBox("An unknown exception", "valid");
            }

            try
            {
                if (recipeForMeal.ID > 0)  // RecipeForMeal ID
                {
                    newMeal.Recipe = recipeForMeal;
                    mealIDsucces = true;
                    //newMeal.Recipe.ID = MainWindow.db.Recipes.Where(r => r.ID == 1);
                }
                else
                {
                    IDexceptionBox("ArgumentOutOfRangeException", "valid (value is below 0)");
                }
            }
            catch (ArgumentNullException)
            {
                IDexceptionBox("ArgumentNullException", "valid");
            }
            catch (NotFiniteNumberException)
            {
                IDexceptionBox("NotFiniteNumberException", "a number");
            }
            catch (Exception)
            {
                IDexceptionBox("An unknown exception", "valid");
            }

            try
            {
                newMeal.Date = dateForMeal;
                dateSucces = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\nHow is that exception even possible?!?", "What have you done??");
            }

            try
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
            catch (NotFiniteNumberException)
            {
                MessageBox.Show("ERROR\nNotFiniteNumberException:\nThe given participants value is not a number.", "addMealToMeals: NotFiniteNumberException");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\nHow is that exception even possible?!?", "What have you done??");
            }


            if (userIDsucces && mealIDsucces && participantsSucces && dateSucces) // If all previously assignments were succesful, the DB will be updated
            {
                MainWindow.db.Meals.Add(newMeal);
                MainWindow.db.SaveChanges();
            }
        }
        private void IDexceptionBox(string errorType, string faultType)
        {
            MessageBox.Show("ERROR\n" + errorType + ":\nThe given ID is not " + faultType, "addMealToMeals: " + errorType);
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