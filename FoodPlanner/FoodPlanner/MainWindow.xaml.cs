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

namespace FoodPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FoodContext db = new FoodContext();

        public MainWindow()
        {
            InitializeComponent();

            selectRecipe.ItemsSource = db.Recipes.ToList<Recipe>();
        }

        private void selectRecipe_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //List<RecipeIngredients> Ingredients = db.RecipeIngredients.Where(r => r.RecipeID == ((Recipes)selectRecipe.SelectedItem).ID).ToList<RecipeIngredients>();

            listIngredients.ItemsSource = ((Recipe)selectRecipe.SelectedItem).RecipeIngredients;
            cookingSteps.ItemsSource = ((Recipe)selectRecipe.SelectedItem).CookingSteps;
        }
    }
}
