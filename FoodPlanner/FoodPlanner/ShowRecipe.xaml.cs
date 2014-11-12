using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
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
    /// Interaction logic for ShowRecipe.xaml
    /// </summary>
    public partial class ShowRecipe : Window
    {
        private string ImageCache(Recipe recipe)
        {
            WebClient client = new WebClient();
            string path = Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString() + "/imageCache";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (!File.Exists(path + "/" + recipe.ID + ".jpg"))
                client.DownloadFile(recipe.Image, path + "/" + recipe.ID + ".jpg");

            return path + "/" + recipe.ID + ".jpg";
        }

        public ShowRecipe(Recipe recipe)
        {
            InitializeComponent();

            showIngredients.ItemsSource = App.db.RecipeIngredients.Where(ri => ri.Recipe.ID == recipe.ID).ToList();
            showSteps.Text = recipe.RecipesPreparation.Preparation;

            imageSource.DataContext = ImageCache(recipe);
        }
    }
}
