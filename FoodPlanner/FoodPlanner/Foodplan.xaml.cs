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

namespace FoodPlanner {
    /// <summary>
    /// Interaction logic for Foodplan.xaml
    /// </summary>
    public partial class Foodplan : Window {
        public Foodplan() {
            InitializeComponent();
        }
        private void Foodplan_Loaded(object sender, RoutedEventArgs e)
        {
            //setup of trvival data
            DateTime moment = new DateTime();
            int year = moment.Year;
            int month = moment.Month;
            int day = moment.Day;
            string dayString = getStringDay(day);
            bool leapYear = DateTime.IsLeapYear(year);
            int daysInMonth = DateTime.DaysInMonth(year, month);
        }
        private string getStringDay(int day){
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
                    stringDay = "Error"; 
                    break;
            }
            return stringDay;
        }
        private List<int> getCurrentDays() {
            List<int> dhdash = new List<int>();


            return dhdash;
    }



        private void randomMeth() {
        }
    }
}
