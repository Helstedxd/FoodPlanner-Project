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
using System.Windows.Shapes;
using FoodPlanner.Models;

namespace FoodPlanner 
{
    /// <summary>
    /// Interaction logic for Foodplan.xaml
    /// </summary>
    public partial class Foodplan : Window 
    {
        public Foodplan() 
        {
            InitializeComponent();
        }

        private void Foodplan_Loaded(object sender, RoutedEventArgs e)
        {
            //setup of trvival data

            DateTime moment = DateTime.Now;
            int year = moment.Year,
                month = moment.Month,
                day = moment.Day,
                weekDay = Convert.ToInt16(moment.DayOfWeek);
            string dayString = getStringDay(weekDay);
            bool leapYear = DateTime.IsLeapYear(year);
            int daysInMonth = DateTime.DaysInMonth(year, month);

            //setCurrentDays(weekDay);

            System.Windows.Data.CollectionViewSource recipeViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("recipeViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // recipeViewSource.Source = [generic data source]
        }

        private string getStringDay(int day)
        {
            string stringDay;
            switch (day)
            {
                case 1:
                    stringDay  = "Mon";
                    break;
                case 2:
                    stringDay  = "Tur";
                    break;
                case 3:
                    stringDay  = "Wen";
                    break;
                case 4:
                    stringDay  = "Thu";
                    break;
                case 5:
                    stringDay  = "Fri";
                    break;
                case 6:
                    stringDay  = "Sat";
                    break;
                case 7:
                    stringDay  = "Sun";
                    break;
                default:
                    stringDay = "Error, out of reach"; 
                    break;
            }
            return stringDay;
        }

        private void setCurrentDays(int weekDay) 
        {
            List<TextBox> dateBoxes = new List<TextBox>(){dateBox1, dateBox2 ,dateBox3,dateBox4,dateBox5,dateBox6,dateBox7};
            weekDay = 1;
            for (int i = 0, j = weekDay - 1; i < 7; i++) 
            {
                if (j < 1) 
                {
                    j = 7;
                }
                else if (j == 8) {
                    j = 1;
                }
                dateBoxes[i].Text = getStringDay(j);
                //Add meal
                j++;
            }

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
                MessageBox.Show(e.Message+"\nHow is that exception even possible?!?", "What have you done??");
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
            MessageBox.Show("ERROR\n" + errorType + ":\nThe given ID is not " + faultType, "addMealToMeals: "+errorType);
        }

        private void foodPlanTest_Click(object sender, RoutedEventArgs e) 
        {
            DateTime moment = DateTime.Now;
            addMealToMeals(moment, MainWindow.db.Recipes.First(), 4);
        }

        private void showCurrentDay(int dayOfWeek) 
        {
            string day = getStringDay(dayOfWeek);
            FoodPlanPage FPP = new FoodPlanPage();

            List<Button> buttonList = new List<Button> { };
        }
    }
}
