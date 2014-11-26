using System.Windows.Controls;
using FoodPlanner.Models;

namespace FoodPlanner.Views
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void DecimalUpDown_InputValidationError(object sender, Xceed.Wpf.Toolkit.Core.Input.InputValidationErrorEventArgs e) {

        }
    }
}
