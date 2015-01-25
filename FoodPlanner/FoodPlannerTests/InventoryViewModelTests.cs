using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FoodPlanner.Models;
using FoodPlanner.ViewModels;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FoodPlannerTests {
    [TestClass]
    public class InventoryViewModelTests {
        [TestMethod]
        public void AddInventoryIngredient_CorrectIngrAdded_InventoryUpdated() {
            //arrange
            Ingredient expectedNewInvenIngredient = new Ingredient();
            InventoryIngredient inventoryIngredientTest = new InventoryIngredient(expectedNewInvenIngredient, 1);
            InventoryViewModel invVM = new InventoryViewModel();
            bool ingredientAdded;

            //act
            invVM.AddIngredientToInventory(expectedNewInvenIngredient, 1);
            
            if (FoodPlanner.App.CurrentUser.InventoryIngredients.Contains(inventoryIngredientTest)) {
                ingredientAdded = true;
            } else {
                ingredientAdded = false;
            }

            //assert
            Assert.IsTrue(ingredientAdded);
        }

        [TestMethod]
        public void RemoveInventoryIngredient_CorrectRemoved_InventoryUpdated() {
            //arrange
            InventoryViewModel invVM = new InventoryViewModel();
            Ingredient ingredient = new Ingredient();
            InventoryIngredient inventoryIngredient = new InventoryIngredient();
            inventoryIngredient.Ingredient = ingredient;
            bool inventoryIngredientRemoved = false;

            //act
            invVM.AddIngredientToInventory(ingredient);
            invVM.RemoveIngredientFromInventory(inventoryIngredient);

            if (!FoodPlanner.App.CurrentUser.InventoryIngredients.Contains(inventoryIngredient)) {
                inventoryIngredientRemoved = true;
            }
            //assert
            Assert.IsTrue(inventoryIngredientRemoved);
        }
    }
}
