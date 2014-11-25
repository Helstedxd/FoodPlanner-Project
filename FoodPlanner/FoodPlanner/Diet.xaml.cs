using FoodPlanner.Models;
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
    /// Interaction logic for Diet.xaml
    /// </summary>
    public partial class Diet : Window
    {
        private List<BlacklistIngredient> bli = new List<BlacklistIngredient>();
        private List<GraylistIngredient> gli = new List<GraylistIngredient>();
        private void makeLists()
        {
            bli = App.db.BlacklistIngredients.Where(uid => uid.UserID == App.CurrentUser.ID).ToList();
            gli = App.db.GraylistIngredients.Where(uid => uid.UserID == App.CurrentUser.ID).ToList();
        }

        public Diet()
        {
            InitializeComponent();

            listDiets.ItemsSource = App.db.DietPresets.ToList();

            makeLists();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App.db.GraylistIngredients.Where(gli => gli.UserID == App.CurrentUser.ID && gli.IsFromDiet).ToList().ForEach(gli => App.db.GraylistIngredients.Remove(gli));
            App.db.BlacklistIngredients.Where(bli => bli.UserID == App.CurrentUser.ID && bli.IsFromDiet).ToList().ForEach(bli => App.db.BlacklistIngredients.Remove(bli));

            foreach (DietRule dr in ((DietPreset)listDiets.SelectedItem).DietRules)
            {
                if (dr.IngredientIsBlacklisted)
                {
                    if (bli.Where(b => b.IngredientID == dr.IngredientID).Count() >= 1)
                    {
                        App.db.BlacklistIngredients.RemoveRange(App.db.BlacklistIngredients.Where(bl => bl.IngredientID == dr.IngredientID));
                    }

                    BlacklistIngredient bi = new BlacklistIngredient() { UserID = App.CurrentUser.ID, IsFromDiet = true, IngredientID = dr.IngredientID };
                    App.db.BlacklistIngredients.Add(bi);
                }
                else
                {
                    if (gli.Where(b => b.IngredientID == dr.IngredientID).Count() >= 1)
                    {
                        App.db.GraylistIngredients.RemoveRange(App.db.GraylistIngredients.Where(gl => gl.IngredientID == dr.IngredientID));
                    }

                    GraylistIngredient gi = new GraylistIngredient() { UserID = App.CurrentUser.ID, IsFromDiet = true, IngredientID = dr.IngredientID, IngredientValue = dr.IngredientValue };
                    App.db.GraylistIngredients.Add(gi);
                }
            }
            App.db.SaveChanges();
            makeLists();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            App.db.GraylistIngredients.Where(gli => gli.UserID == App.CurrentUser.ID && gli.IsFromDiet).ToList().ForEach(gli => App.db.GraylistIngredients.Remove(gli));
            App.db.BlacklistIngredients.Where(bli => bli.UserID == App.CurrentUser.ID && bli.IsFromDiet).ToList().ForEach(bli => App.db.BlacklistIngredients.Remove(bli));
            App.db.SaveChanges();
            makeLists();
        }
    }
}
