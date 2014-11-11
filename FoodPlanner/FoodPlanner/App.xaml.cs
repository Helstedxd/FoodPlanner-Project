using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using FoodPlanner.Models;

namespace FoodPlanner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //TODO: Implement proper NavigationService and maybe IOC.
        //public static Frame MainFrame { get; set; }
        public static NavigationService NavigationService { get; set; }
    
        public static FoodContext db { get; set; }
        public static User CurrentUser { get; set; }
    }
}
