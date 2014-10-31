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
    /// Interaction logic for RecommendedRecipesWindow.xaml
    /// </summary>
    public partial class RecommendedRecipesWindow : Window
    {
        public RecommendedRecipesWindow()
        {
            InitializeComponent();

            var q3 = from ri in MainWindow.db.RecipeIngredients
                     join ii in MainWindow.db.InventoryIngredients on ri.IngredientID equals ii.IngredientID
                     select new { recipeID = ri.RecipeID, recipe = ri.Recipe, riq = ri.Quantity, iiq = ii.Quantity } into c
                     group c by c.recipeID into g
                     select g;
                     //select new { riq = g /*ri.Quantity, iiq = ii.Quantity*/ };

                   // select new { lol.Key }; //produces flat sequence
            //select ri.Recipe;

            var l3 = q3.ToList();

            foreach (var group in l3) {
                //var x = group.FirstOrDefault();
                Recipe r = group.First().recipe;
                int ingredientCount = r.RecipeIngredients.Count();
                decimal totalPercent = 0;
                foreach (var x in group) { 
                   // decimal matchPercent = x.
                
                }
             
            
            }


        }
    }
}
