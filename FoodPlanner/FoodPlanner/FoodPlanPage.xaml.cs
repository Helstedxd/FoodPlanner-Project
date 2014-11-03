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

namespace FoodPlanner {
    /// <summary>
    /// Interaction logic for FoodPlanPage.xaml
    /// </summary>
    public partial class FoodPlanPage : Page 
    {
        public FoodPlanPage() 
        {
            InitializeComponent();
        }

        private string getStringDay(int day) 
        {
            string stringDay;
            switch (day) 
            {
                case 1:
                    stringDay = "Mon";
                    break;
                case 2:
                    stringDay = "Tue";
                    break;
                case 3:
                    stringDay = "Wed";
                    break;
                case 4:
                    stringDay = "Thu";
                    break;
                case 5:
                    stringDay = "Fri";
                    break;
                case 6:
                    stringDay = "Sat";
                    break;
                case 7:
                    stringDay = "Sun";
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

        private void foodPlanTest_Click(object sender, RoutedEventArgs e) 
        {
            DateTime moment = DateTime.Now;
            addMealToMeals(moment, MainWindow.db.Recipes.First(), 4);
        }

        /*private void showCurrentDay(int dayOfWeek) 
        {
         //   string day = getStringDay();
            FoodPlanPage FPP = new FoodPlanPage();

            List<Button> buttonList = new List<Button> { butUp, butMon, butTue, butWed, butThu, butFri, butSat, butSun, butDown };
            List<Line> lineList = new List<Line> { upLine, monLine, tueLine, wedLine, thuLine, friLine, satLine, sunLine, downLine };

            for (int i = 0; i < buttonList.Count; i++) 
            {
                if (buttonList[i].Content.ToString() == day) 
                {
                    lineList[i].Visibility = Visibility.Hidden;
                }   
            }
        }*/

        private void buttonInFocus(object sender, RoutedEventArgs e)
        {
            List<Button> buttonList = new List<Button> { butUp, butMon, butTue, butWed, butThu, butFri, butSat, butSun, butDown };

            foreach (Button but in buttonList) 
            {
                if (but.IsFocused) 
                {
                    but.Background = Brushes.LightGray;
                }
                else 
                {
                    but.Background = Brushes.White;
                }
            }
            
        }

        private void updateArrows(object sender, RoutedEventArgs e) 
        {
            System.Globalization.Calendar calendar = CultureInfo.CurrentCulture.Calendar;
            DateTime moment = DateTime.Now;
            System.Globalization.CalendarWeekRule rule = CalendarWeekRule.FirstFourDayWeek;
            int weeNumber = calendar.GetWeekOfYear(moment, rule, moment.DayOfWeek);
            bool goUp;

            List<Button> buttonList = new List<Button> { butUp, butDown };
            if (buttonList[0].IsFocused) {
                goUp = true;
            }
            else
            {
                goUp = false;
            }

            string upDate = buttonList[0].Content.ToString(), downDate = buttonList[1].Content.ToString();//text from butDown and butUp
            string[] upWeek = upDate.Split(' '), downWeek = downDate.Split(' '); //Splits the sting into '^' 'week:' 'number'

            if (goUp) 
            {
                butUp.Content = "^ week: " + (newWeek(Convert.ToInt16(upWeek[2]), true)).ToString(); //gets an updated 'number' and updates the text on the butUp
                butDown.Content = "v week: " + (newWeek(Convert.ToInt16(downWeek[2]), true)).ToString(); //gets an updated 'number' and updates the text on the butDown                
            }
            else if (!goUp)
            {
                butUp.Content = "^ week: " + (newWeek(Convert.ToInt16(upWeek[2]), false)).ToString(); //gets an updated 'number' and updates the text on the butUp
                butDown.Content = "v week: " + (newWeek(Convert.ToInt16(downWeek[2]), false)).ToString(); //gets an updated 'number' and updates the text on the butDown
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) 
        {
            System.Globalization.Calendar calendar = CultureInfo.CurrentCulture.Calendar;
            DateTime moment = DateTime.Now;
            System.Globalization.CalendarWeekRule rule = CalendarWeekRule.FirstFourDayWeek;
            int weekNumber = calendar.GetWeekOfYear(moment, rule, moment.DayOfWeek);

            int upWeek = newWeek(weekNumber, true), downWeek = newWeek(weekNumber, false);
            butUp.Content = "^ week: " + upWeek.ToString();
            butDown.Content = "v week: " + downWeek.ToString();
        }
        private int newWeek(int currentWeek, bool goesUp) 
        {
            DateTime moment = new DateTime(DateTime.Now.Year, 12, 27);
            if (DateTime.IsLeapYear(moment.Year)) //It is impornant to know if its a leapyear. The ate of which the number of weeks depends on requires to know if it is a leapyear
            {
                moment.AddDays(-1);
                if (moment.DayOfWeek == DayOfWeek.Sunday || moment.DayOfWeek == DayOfWeek.Saturday) //53 weeks if the 26 december is a sunday or saturday
                {
                    if (goesUp) 
                    {
                        if (currentWeek == 53) 
                        {
                            currentWeek = 1;
                        }
                        else 
                        {
                            currentWeek++;
                        }
                    }
                    else 
                    {
                        if (currentWeek == 1) 
                        {
                            currentWeek = 53;
                        }
                        else 
                        {
                            currentWeek--;
                        }
                    }
                    return currentWeek;
                }
                else 
                {
                    if (goesUp) 
                    {
                        if (currentWeek == 52) 
                        {
                            currentWeek = 1;
                        }
                        else 
                        {
                            currentWeek++;
                        }
                    }
                    else 
                    {
                        if (currentWeek == 1) 
                        {
                            currentWeek = 52;
                        }
                        else {
                            currentWeek--;
                        }
                    }
                    return currentWeek;
                }
            }
            if (moment.DayOfWeek == DayOfWeek.Sunday) //53 weeks if the 27 december is a sunday
            {
                if (goesUp) 
                {
                    if (currentWeek == 53) 
                    {
                        currentWeek = 1;
                    }
                    else 
                    {
                        currentWeek++;
                    }
                }
                else {
                    if (currentWeek == 1) 
                    {
                        currentWeek = 53;
                    }
                    else 
                    {
                        currentWeek--;
                    }
                }
                return currentWeek;
            }
            else 
            {
                if (goesUp) 
                {
                    if (currentWeek == 52) 
                    {
                        currentWeek = 1;
                    }
                    else 
                    {
                        currentWeek++;
                    }
                }
                else 
                {
                    if (currentWeek == 1) 
                    {
                        currentWeek = 52;
                    }
                    else 
                    {
                        currentWeek--;
                    }
                }
                return currentWeek;
            }
        }
    }
}
