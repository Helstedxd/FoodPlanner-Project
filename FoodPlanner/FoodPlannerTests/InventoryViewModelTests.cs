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
            Ingredient expectedNewIngredient = new Ingredient();
            InventoryIngredient inventoryIngredient = new InventoryIngredient(expectedNewIngredient, 1);
            InventoryViewModel invVM = new InventoryViewModel();
            bool ingredientAdded;

            //act
            invVM.AddIngredientToInventory(expectedNewIngredient, 1);

            if (FoodPlanner.App.CurrentUser.InventoryIngredients.Contains(inventoryIngredient)) {
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
