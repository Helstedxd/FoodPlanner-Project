﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using FoodPlanner.Models;
using System.Data.Entity.Core;

namespace FoodPlanner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static FoodContext db { get; private set; }
        public static User CurrentUser { get; private set; }

        private void ApplicationStartup(object sender, StartupEventArgs args)
        {
            // Create new context with entity framework
            App.db = new FoodContext();

            // Just select the first user, since we do not really support log-in yet.
            App.CurrentUser = App.db.Users.First();

            // Create the main window and tie the frame to the Navigator.
            MainWindow mainWindow = new MainWindow();
            Navigator.NavigationService = mainWindow.Frame.NavigationService;
            Navigator.NavigationService.Source = new Uri(FoodPlanner.Properties.Settings.Default.StartPage, UriKind.Relative);
           
            removePassedMeals();
            mainWindow.Show();
        }

        private void ApplicationExit(object sender, ExitEventArgs args)
        {
            // Save changes to database and close the connection
            App.db.SaveChanges();
            App.db.Dispose();
        }

        private void removePassedMeals()
        {
            DateTime today = DateTime.Now.AddDays(-1);

            List<Meal> meals = (from m in db.Meals
                                where m.UserID == CurrentUser.ID && m.IsActive && m.Date < today
                                select m).ToList();

            List<InventoryIngredient> inventoryIngredient = (from ii in db.InventoryIngredients
                                                             where ii.UserID == CurrentUser.ID
                                                             select ii).ToList();

            foreach (Meal m in meals)
            {
                db.Meals.Where(m2 => m2.ID == m.ID).SingleOrDefault().IsActive = false;

                foreach (RecipeIngredient ri in m.Recipe.RecipeIngredients)
                {
                    if (inventoryIngredient.Where(ii => ii.IngredientID == ri.IngredientID).Count() != 0)
                    {
                        decimal rest = ri.Quantity;
                        foreach (InventoryIngredient ii in inventoryIngredient.Where(ii => ii.IngredientID == ri.IngredientID))
                        {
                            if (ii.Quantity <= ri.Quantity)
                            {
                                db.InventoryIngredients.Remove(ii);
                                rest -= ii.Quantity;
                            }
                            else
                            {
                                db.InventoryIngredients.Where(i => i.ID == ii.ID).FirstOrDefault().Quantity -= rest;
                            }
                        }
                    }
                }
            }
            db.SaveChanges();
        }
    }
}
