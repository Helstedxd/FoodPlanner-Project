using System.Windows.Controls;
using FoodPlanner.Models;

namespace FoodPlanner.Views
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public BlacklistIngredient SelectedBlackListIngredient { get; set; }
        
        public SettingsPage()
        {
            InitializeComponent();
        }
    }
}
