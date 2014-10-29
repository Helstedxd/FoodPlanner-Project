using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for ShowRecipe.xaml
    /// </summary>
    public partial class ShowRecipe : Window
    {
        private BitmapImage ROFLMETHOD(string bgImage64)
        {
            byte[] binaryData = Convert.FromBase64String(bgImage64);

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = new MemoryStream(binaryData);
            bi.EndInit();

            return bi;
        }


        public ShowRecipe(Recipe recipe)
        {
            InitializeComponent();

            showIngredients.ItemsSource = MainWindow.db.RecipeIngredients.Where(ri => ri.Recipe.ID == recipe.ID).ToList();
            showSteps.ItemsSource = recipe.CookingSteps.ToList();

            imageSource.Source = ROFLMETHOD(System.Text.Encoding.UTF8.GetString(recipe.Image));
        }
    }
}
