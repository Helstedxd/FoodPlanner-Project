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

            setCurrentDays(weekDay);
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

            for (int i = 0, j = weekDay - 1; i < 7; i++) {
                if (j < 1) 
                {
                    j = 7;
                }
                else if (j == 8) {
                    j = 1;
                }
                dateBoxes[i].Text = getStringDay(j);
                j++;
            }

        }



        private void randomMeth() 
        {
        }
    }
}
