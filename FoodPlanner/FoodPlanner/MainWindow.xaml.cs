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
using FoodPlanner.Models;

namespace FoodPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //TODO: remove this 
            // Test code for displaying data
            using (var context = new FoodContext())
            {

                /*var newRecipe = new Recipe("SnickersBar", "noget hej", 300, "whatevers");

                var cf1 = new Food(new Ingredient("Chokolade"), 500);
                var cf2 = new Food(new Ingredient("Karamel"), 100);
                var cf3 = new Food(new Ingredient("Nødder"), 10);

                newRecipe.Ingredients.Add(cf1);
                newRecipe.Ingredients.Add(cf2);
                newRecipe.Ingredients.Add(cf3);

                context.Recipes.Add(newRecipe);
                context.SaveChanges();*/

                foreach (Recipe recipe in context.Recipes)
                {
                    Console.WriteLine(recipe.Title);
                    if (recipe.Ingredients != null)
                    {
                        foreach (Food food in recipe.Ingredients)
                        {
                            Console.WriteLine("  -" + food.Ingredient + " (" + food.Quantity + ")");
                        }
                    }
                }

            }
        }


    }
}